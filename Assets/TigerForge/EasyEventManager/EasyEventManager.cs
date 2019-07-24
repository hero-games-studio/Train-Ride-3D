using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace TigerForge
{
    /// <summary>
    /// Events management system.
    /// </summary>
    public class EventManager {

        private static Dictionary<string, UnityEvent> eventDictionary = new Dictionary<string, UnityEvent>();
        private static Dictionary<string, object> storage = new Dictionary<string, object>();
        private static Dictionary<string, object> sender = new Dictionary<string, object>();
        private static Dictionary<string, bool> paused = new Dictionary<string, bool>();

        /// <summary>
        /// Starts the listening to an event with the given name. If that event is detected, the callBack function is executed. 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="callBack"></param>
        public static void StartListening(string eventName, UnityAction callBack)
        {
            if (eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
            {
                thisEvent.AddListener(callBack);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(callBack);
                eventDictionary.Add(eventName, thisEvent);
                paused.Add(eventName, false);
            }
        }

        /// <summary>
        /// Starts the listening to an event with the given name and enabling the use of filters on events emission. If that event is detected, and optional filters are satisfied, the callBack function is executed.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="target"></param>
        /// <param name="callBack"></param>
        /// <param name="canUseName"></param>
        /// <param name="canUseTag"></param>
        /// <param name="canUseLayer"></param>
        public static void StartListening(string eventName, GameObject target, UnityAction callBack)
        {
            if (target == null)
            {
                Debug.LogError("The specified target is not a valid GameObject.");
                return;
            }

            StartListening(eventName, callBack);

            string newName = eventName + "__##name##" + target.name + "##" + "__##tag##" + target.tag + "##" + "__##layer##" + target.layer + "##";
            StartListening(newName, callBack);

        }

        /// <summary>
        /// Stop listening to the event with the given name. The callBack function must be specified.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="callBack"></param>
        public static void StopListening(string eventName, UnityAction callBack)
        {
            if (eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
            {
                thisEvent.RemoveListener(callBack);
            }
        }

        /// <summary>
        /// Emit an event with the given name.
        /// </summary>
        /// <param name="eventName"></param>
        public static void EmitEvent(string eventName)
        {
            if (isPaused(eventName)) return;

            if (eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
            {
                thisEvent.Invoke();
            }
        }

        /// <summary>
        /// Emit an event with the given name and save the sender.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="sender"></param>
        public static void EmitEvent(string eventName, object sender)
        {
            if (isPaused(eventName)) return;

            if (EventManager.sender.ContainsKey(eventName)) EventManager.sender[eventName] = sender; else EventManager.sender.Add(eventName, sender);

            EmitEvent(eventName);
        }

        /// <summary>
        /// Emit the event with the given name after the specified delay seconds.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="delay"></param>
        public static void EmitEvent(string eventName, float delay)
        {
            if (isPaused(eventName)) return;

            if (eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
            { 
                if (delay <= 0)
                {
                    thisEvent.Invoke();
                }
                else
                {
                    int d = (int)(delay * 1000);
                    DelayedInvoke(thisEvent, d);
                }
            }
        }

        /// <summary>
        /// Emit the event with the given name after the specified delay seconds and save the sender.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="delay"></param>
        /// <param name="sender"></param>
        public static void EmitEvent(string eventName, float delay, object sender)
        {
            if (isPaused(eventName)) return;

            if (EventManager.sender.ContainsKey(eventName)) EventManager.sender[eventName] = sender; else EventManager.sender.Add(eventName, sender);

            EmitEvent(eventName, delay);
        }

        /// <summary>
        /// Emit the event with the given name to listeners selected by the specified filter. Optionally, a delay and a sender can be specified.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="filter"></param>
        /// <param name="delay"></param>
        /// <param name="sender"></param>
        public static void EmitEvent(string eventName, string filter, float delay = 0f, object sender = null)
        {
            if (sender != null)
            {
                if (EventManager.sender.ContainsKey(eventName)) EventManager.sender[eventName] = sender; else EventManager.sender.Add(eventName, sender);
            }

            var data = filter.Split(';');
            var name = "";
            var tag = "";
            var layer = "";

            int counter = data.Length;

            foreach (string s in data)
            {
                var tmp = s.Split(':');
                if (tmp[0] == "name") name = "__##name##" + tmp[1] + "##";
                if (tmp[0] == "tag") tag = "__##tag##" + tmp[1] + "##";
                if (tmp[0] == "layer") layer = "__##layer##" + tmp[1] + "##";
            }

            int found = 0;

            foreach (KeyValuePair<string, UnityEvent> evnt in eventDictionary)
            {
                if (name != "" && evnt.Key.Contains(name)) found++;
                if (tag != "" && evnt.Key.Contains(tag)) found++;
                if (layer != "" && evnt.Key.Contains(layer)) found++;

                if (found == counter)
                {
                    //Debug.Log("OK chiamo: " + evnt.Key);
                    EmitEvent(evnt.Key, delay);
                    found = 0;
                }
            }

        }


        /// <summary>
        /// Stop all the listeners.
        /// </summary>
        public static void StopAll()
        {
            foreach (KeyValuePair<string, UnityEvent> evnt in eventDictionary)
            {
                evnt.Value.RemoveAllListeners();
            }
        }

        private static async void DelayedInvoke(UnityEvent thisEvent, int delay)
        {
            await Task.Delay(delay);
            thisEvent.Invoke();
        }

        /// <summary>
        /// Return true if there is at least one listener.
        /// </summary>
        /// <returns></returns>
        public static bool IsListening()
        {
            return eventDictionary.Count > 0;
        }

        /// <summary>
        /// Suspend the listening.
        /// </summary>
        public static void PauseListening()
        {
            SetPaused(true);
        }

        /// <summary>
        /// Suspend the listening of the event with the given name.
        /// </summary>
        /// <param name="eventName"></param>
        public static void PauseListening(string eventName)
        {
            SetPaused(eventName, true);
        }

        /// <summary>
        /// Restart the listening.
        /// </summary>
        public static void RestartListening()
        {
            SetPaused(false);
        }

        /// <summary>
        /// Restart the listening of the event with the given name.
        /// </summary>
        /// <param name="eventName"></param>
        public static void RestartListening(string eventName)
        {
            SetPaused(eventName, false);
        }

        /// <summary>
        /// Return true if the event with the given name has been paused.
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static bool isPaused(string eventName)
        {
            if (paused.ContainsKey(eventName)) return paused[eventName]; else return true;
        }

        private static void SetPaused(bool value)
        {
            Dictionary<string, bool> copy = new Dictionary<string, bool>();

            foreach (KeyValuePair<string, bool> eName in paused)
            {
                copy.Add(eName.Key, value);
            }

            paused = copy;
        }

        private static void SetPaused(string eventName, bool value)
        {
            Dictionary<string, bool> copy = new Dictionary<string, bool>();

            foreach (KeyValuePair<string, bool> eName in paused)
            {
                if (eName.Key == eventName) copy.Add(eName.Key, value); else copy.Add(eName.Key, eName.Value);
            }

            paused = copy;
        }

        /// <summary>
        /// Save data for the event with the given name.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="data"></param>
        public static void SetData(string eventName, object data)
        {
            if (storage.ContainsKey(eventName)) storage[eventName] = data; else storage.Add(eventName, data);
        }

        /// <summary>
        /// Return the data for the event with the given name (or null if nothing found).
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static object GetData(string eventName)
        {
            try
            {
                if (storage.ContainsKey(eventName)) return storage[eventName]; else return null;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Return the GameObject data for the event with the given name (or null if nothing found).
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static GameObject GetGameObject(string eventName)
        {
            try
            {
                if (storage.ContainsKey(eventName)) return (GameObject)storage[eventName]; else return null;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Return the integer data for the event with the given name (or 0 if nothing found).
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static int GetInt(string eventName)
        {
            try
            {
                if (storage.ContainsKey(eventName)) return (int)storage[eventName]; else return 0;
            }
            catch (System.Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Return the boolean data for the event with the given name (or false if nothing found).
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static bool GetBool(string eventName)
        {
            try
            {
                if (storage.ContainsKey(eventName)) return (bool)storage[eventName]; else return false;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Return the float data for the event with the given name (or 0 if nothing found).
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static float GetFloat(string eventName)
        {
            try
            {
                if (storage.ContainsKey(eventName)) return (float)storage[eventName]; else return 0f;
            }
            catch (System.Exception)
            {
                return 0f;
            }
        }

        /// <summary>
        /// Return the string data for the event with the given name (or "" if nothing found).
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static string GetString(string eventName)
        {
            try
            {
                if (storage.ContainsKey(eventName)) return (string)storage[eventName]; else return "";
            }
            catch (System.Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// Return the sender for the event with the given name (or null if nothing found).
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static object GetSender(string eventName)
        {
            try
            {
                if (sender.ContainsKey(eventName)) return sender[eventName]; else return null;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Clear all the data stored in memory. This method clear data only, whereas the listeners continue to work.
        /// </summary>
        public static void ClearData()
        {
            storage = new Dictionary<string, object>();
            sender = new Dictionary<string, object>();
        }

    }
    

}

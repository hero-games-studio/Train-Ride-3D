using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public PathCreator pathCreator2;
        public PathCreator pathCreator3;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        float distanceTravelled;
        bool check = false;

        void Start()
        {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
            if (pathCreator2 != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator2.pathUpdated += OnPathChanged;
            }
            if (pathCreator3 != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator3.pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
            if ((pathCreator != null) && (check == false))
            {
                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                //  transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);   We can delete this row because it looks not necessary for our game   
            }

            if ((pathCreator2 != null) && (Input.GetKey(KeyCode.LeftArrow)))
            {
                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator2.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                check = true;
            }

            if ((pathCreator3 != null) && (Input.GetKey(KeyCode.RightArrow)))
            {
                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator3.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                check = true;
            }

        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged()
        {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}
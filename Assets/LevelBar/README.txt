-------------Levelbar Plugin-------------

Dependencies: This plugin uses TextMeshPRO, make sure TextMeshPRO is installed before this plugin.

How to use:

1. Add the LevelBar prefab into the canvas of your scene.
2. Change values however you see fit.

Functionality:
    The LevelBar can be accessed as a singleton. LevelBar.Instance will give you the LevelBar object that is currently
in the active scene. (Don't have more than one)
    The value of the LevelBar must be updated through a script (functions are given below). The value will determine
the progress of the bar relative to the minimum and maximum values.
    Text colors are selected automatically depending on what is easiest to read compared to the color behind it.

///FUNCTIONS///
All visual updates are automatic.

//Value
- SetValue(float val) : Sets the value of the bar.
- AddValue(float val) : Adds the value to the current value.
- GetValue()          : Returns the current value as a float.
- UpdateMin(float m)  : Sets the minimum to the new value.
- UpdateMax(float m)  : Sets the maximum to the new value.


//Progress
- Completed()   : returns true if the bar is full, false if not.
- GetProgress() : returns the completion as a value between between 0 and 1. Multiplying by 100 will give percentage completion.

//Colors
- UpdateBG_Color(Color bgcol) : Updates the background color.
- UpdateFill_Color(Color fc)  : Updates the fill color.

//Texts
- UpdateLeftText(string tx)  : Updates the text on the left circle.
- UpdateRightText(string tx) : Updates the text on the right circle.

//Counter
- UpdateCounter_Color(Color fc)   :  Updates the color of the counter text.
- UpdateCounterText(string tx)    :  Updates the text of the counter.
- SetCounterActive(bool active)   :  Activate or disable the counter.

//Reset
- ResetBar()  : Resets the value to the minimum. Reloads references.
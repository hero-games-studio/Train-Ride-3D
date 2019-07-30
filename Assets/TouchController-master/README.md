# TouchController
  Basic touch controls with delegation method 
For usage firstly add this script to a gameobject because it uses update method for touch pooling.
After adding to a gameobject you can take it's instance to wherever you want and start using.

  How to use
Firstly you need to write your game touch logic in a method that takes a one TouchResult parameter

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
void yourMethod(TouchResult result)
{
  //You can write your game logic here
  //If you need other parameters to be passed you can simply call other functions from this method and reach the data you need.
  
  //With this code you can reach to touch began position till touch releases after that till next touch it wont be passed again
  Debug.Log(result.firstPressPosition);
}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


This parameter gives first press position of touch and touches itself. Then you need to add your method to TouchControl with addBehaviour function like
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
touchCotrolInstance.addBehaviour(TouchPhase.Began,yourMethod);
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

With this implementation your method will be triggered when touch controller detects a touch began. You can add multiple functions for every TouchPhase type like
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
touchCotrolInstance.addBehaviour(TouchPhase.Began,yourMethod_1);
touchCotrolInstance.addBehaviour(TouchPhase.Began,yourMethod_2);
touchCotrolInstance.addBehaviour(TouchPhase.Began,yourMethod_3);
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

And all of this will be called when a touch began happened

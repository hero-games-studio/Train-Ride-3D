using UnityEngine;
using UnityEditor;
 
public class AutoSnap : EditorWindow
{
    private Vector3 prevPosition;
    private bool doSnap = true;
    private float z_snapValue = 5;
    private float x_snapValue = 1.7f;
    private float y_snapValue = 0.2f;
 
    [MenuItem( "Edit/Auto Snap %_l" )]
 
    static void Init()
    {
       var window = (AutoSnap)EditorWindow.GetWindow( typeof( AutoSnap ) );
       window.maxSize = new Vector2( 200, 100 );
    }
 
    public void OnGUI()
    {
       doSnap = EditorGUILayout.Toggle( "Auto Snap", doSnap );
       x_snapValue = EditorGUILayout.FloatField( "X Snap Value", x_snapValue );
       y_snapValue = EditorGUILayout.FloatField( "Y Snap Value", y_snapValue );
       z_snapValue = EditorGUILayout.FloatField( "Z Snap Value", z_snapValue );
    }
 
    public void Update()
    {
       if ( doSnap
         && !EditorApplication.isPlaying
         && Selection.transforms.Length > 0
         && Selection.transforms[0].position != prevPosition )
       {
         Snap();
         prevPosition = Selection.transforms[0].position;
       }
    }
 
    private void Snap()
    {
       foreach ( var transform in Selection.transforms )
       {
         var t = transform.transform.position;
         t.x = RoundX( t.x );
         t.y = RoundY( t.y );
         t.z = RoundZ( t.z );
         transform.transform.position = t;
       }
    }
 
    private float RoundX( float input )
    {
       return x_snapValue * Mathf.Round( ( input / x_snapValue ) );
    }

    private float RoundY( float input )
    {
       return y_snapValue * Mathf.Round( ( input / y_snapValue ) );
    }

    private float RoundZ( float input )
    {
       return z_snapValue * Mathf.Round( ( input / z_snapValue ) );
    }
}
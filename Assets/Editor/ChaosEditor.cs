using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChaosKaleidoscope))]
[CanEditMultipleObjects] // This lets you randomize 100 objects at once!
public class ChaosEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default stuff (the sliders and floats)
        DrawDefaultInspector();

        ChaosKaleidoscope script = (ChaosKaleidoscope)target;

        GUILayout.Space(10);
        if (GUILayout.Button("GENERATE NEW PATTERN", GUILayout.Height(40)))
        {
            // If you have multiple objects selected, this hits them all
            foreach (var obj in targets)
            {
                ((ChaosKaleidoscope)obj).RandomizeRotation();
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class LazyGraph : EditorWindow
{
    private LazyGraphView graphView;

    [MenuItem("Tools/Lazy/Open Graph #g")]
    public static void OpenGraph()
    {
        var window = GetWindow<LazyGraph>();
        window.titleContent = new GUIContent("Lazy Graph");
        window.Show();
    }

    private void OnEnable()
    {
        graphView = new LazyGraphView
        {
            name = "Lazy Graph 2"
        };

        graphView.style.width = new StyleLength(new Length(100, LengthUnit.Percent));
        graphView.style.height = new StyleLength(new Length(100, LengthUnit.Percent));
        rootVisualElement.Add(graphView);
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(graphView);
    }

    private void OnGUI()
    {
        Debug.Log("ON gui");
        Handles.DrawWireCube(Vector3.zero, Vector3.one * 55);
    }
}

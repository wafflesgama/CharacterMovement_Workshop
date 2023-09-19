using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeTest : Node
{
    public string GUID;

    public override void OnUnselected()
    {
        Debug.Log("node unselected");
        base.OnUnselected();
    }
    public override void OnSelected()
    {
        Debug.Log("node selected");
        base.OnSelected();
    }
}

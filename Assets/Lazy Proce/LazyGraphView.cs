using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class LazyGraphView : GraphView
{
    public LazyGraphView()
    {
        //Must be this order to properly work
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());


        this.AddElement(CreateTestNode());
    }

    private Node CreateTestNode()
    {
        var node = new NodeTest
        {
            GUID = GUID.Generate().ToString(),
            title = "Teste Node"
        };

        node.SetPosition(new Rect(10,10,200,200));

        return node;
    }

}

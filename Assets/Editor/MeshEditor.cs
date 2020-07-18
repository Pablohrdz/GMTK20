using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshCreator))]
public class MeshEditor : Editor {

    MeshCreator creator;

    void OnSceneGUI()
    {
        if (creator.autoUpdate && Event.current.type == EventType.Repaint)
        {
            creator.UpdateMesh();
        }
    }

    void OnEnable()
    {
        creator = (MeshCreator)target;
    }
}

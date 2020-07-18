using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathPlacer))]
[RequireComponent(typeof(PathTransformUpdater))]
[RequireComponent(typeof(MeshCreator))]
[RequireComponent(typeof(ColliderCreator))]
public class PathCreator : MonoBehaviour {

    [HideInInspector]
    public Path path;
    public float initialSize = 40.0f;
    public float bezierLineWidth = 5.0f;

    public Color anchorCol = Color.red;
    public Color controlCol = Color.white;
    public Color segmentCol = Color.green;
    public Color selectedSegmentCol = Color.yellow;
    public float anchorDiameter = 1f;
    public float controlDiameter = .75f;
    public bool displayControlPoints = true;

    public void CreatePath()
    {
        path = new Path(transform.position, initialSize);
    }

    void Reset()
    {
        CreatePath();
    }
}

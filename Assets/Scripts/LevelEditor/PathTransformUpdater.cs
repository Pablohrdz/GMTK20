using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PathTransformUpdater : MonoBehaviour
{
    private Vector3 LastPosition;
    private PathCreator PathCreator;

    // Start is called before the first frame update
    void Start()
    {
        LastPosition = transform.position;
        PathCreator = GetComponent<PathCreator>();
    }

    private void Update()
    {
        if (transform.position != LastPosition)
        {
            bool autoSetControlPoints = PathCreator.path.AutoSetControlPoints;
            PathCreator.path.AutoSetControlPoints = false;

            Vector3 deltaPos = transform.position - LastPosition;

            for (int i = 0; i < PathCreator.path.NumPoints; i++)
            {
                Vector3 pointPos = PathCreator.path[i];
                PathCreator.path.MovePointEvenly(i, deltaPos);
            }

            LastPosition = transform.position;
            PathCreator.path.AutoSetControlPoints = autoSetControlPoints;
        }
    }

    private void OnValidate()
    {
        LastPosition = transform.position;
    }
}

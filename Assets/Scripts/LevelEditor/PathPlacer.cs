using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPlacer : MonoBehaviour {

    public float spacing = .1f;
    public float resolution = 1;
    public bool drawSpheres = false;

    private Transform LastGameObjectTransform;
    private PathCreator PathCreator;

    void Start () {
        PathCreator = GetComponent<PathCreator>(); // FindObjectOfType<PathCreator>().path.CalculateEvenlySpacedPoints(spacing, resolution);
        Vector2[] points = PathCreator.path.CalculateEvenlySpacedPoints(spacing, resolution);

        if (drawSpheres)
        {
            foreach (Vector2 p in points)
            {
                GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                g.transform.position = p;
                g.transform.localScale = Vector3.one * spacing * .5f;
            }
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera2D : MonoBehaviour
{

    public float DampTime = 0.15f;
    private Vector3 Velocity = Vector3.zero;
    public Transform Target;

    // Update is called once per frame
    void Update()
    {
        if (Target)
        {
            Vector3 point = GetComponent<Camera>().WorldToViewportPoint(Target.position);
            Vector3 delta = Target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref Velocity, DampTime);
        }

    }
}
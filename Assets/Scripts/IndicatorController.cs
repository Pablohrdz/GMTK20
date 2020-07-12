using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorController : MonoBehaviour
{
    public Transform airBar;
    public GameObject submarineGO; // find by tag?
    public Vector3 offset;
    SubmarineController submarine;

    private void Start()
    {
        submarine = submarineGO.GetComponent<SubmarineController>();
    }

    private void Update()
    {
        airBar.localScale = new Vector3(submarine.air / submarine.airMax, 1, 1);
        transform.position = submarineGO.transform.position + offset;
    }
}

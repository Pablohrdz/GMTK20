using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : Enemy
{
    void Update()
    {
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }
}

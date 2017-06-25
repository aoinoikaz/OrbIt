using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeCollider : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D col)
    {
        
        Debug.Log("Collision w screen bounds: " + col.gameObject.name);
    }
}

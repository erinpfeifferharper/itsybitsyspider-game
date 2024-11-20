using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollision : MonoBehaviour
{
    void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f);

        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Wall")
            {
                Destroy(gameObject);
                return;
            }
        }
    }
}

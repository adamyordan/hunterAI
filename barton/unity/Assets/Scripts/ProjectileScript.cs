using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb.velocity.magnitude < 0.01f)
        {
            Destroy(this.gameObject, 1.5f);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}

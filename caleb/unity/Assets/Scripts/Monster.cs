using System;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    public Action DestroyedCallback;
    public GameObject DeathEffect;

    void Update()
    {
        if (transform.position.y < -1f)
        {
            Die();
        }
    }

    public void Die()
    {
        DestroyedCallback();
        Destroy(Instantiate(DeathEffect, transform.position, transform.rotation), 1.0f);
        Destroy(gameObject);
    }
}

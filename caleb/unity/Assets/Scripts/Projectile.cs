using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int MaxDistance;
	public float Speed;

    Vector2Int startPos;

    void Start()
    {
        startPos = Helper.GridVector(transform.position);
    }

    void FixedUpdate()
    {
        Vector2 currentPos = Helper.FlatVector(transform.position);
        if (Vector2.Distance(startPos, currentPos) > (MaxDistance - 0.2))
        {
            Destroy(gameObject);
        }

        float moveDistance = Speed * Time.deltaTime;

        CheckCollision(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
    }

    void CheckCollision(float moveDistance)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit,
            moveDistance, LayerMask.GetMask("Default"),
            QueryTriggerInteraction.Collide))
        {
            Destroy(gameObject);
			IDamageable damageable = hit.collider.GetComponent<IDamageable>();
			if (damageable != null)
			{
				damageable.Die();
			}
		}
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

using UnityEngine;

public class Hunter : MonoBehaviour
{
    public Projectile Projectile;
    public GameObject ThinkingObj;
    public float Speed = 1.0f;

    public Quaternion targetRotation;
    public Vector2Int targetPosition;

    public GameObject shootPoint;
    readonly float EPSILON = 0.01f;
    

    void Start()
    {
        targetRotation = transform.rotation;
        targetPosition = Helper.GridVector(transform.position);
        shootPoint = transform.Find("HunterShootPoint").gameObject;
    }

    void FixedUpdate()
    {
        if (Quaternion.Angle(transform.rotation, targetRotation) > EPSILON)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10 * Speed * Time.deltaTime);
        }
        if (System.Math.Abs(transform.position.x - targetPosition.x) > EPSILON|| System.Math.Abs(transform.position.z - targetPosition.y) > EPSILON)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(targetPosition.x, transform.position.y, targetPosition.y), 10 * Speed * Time.deltaTime);
        }
    }

    public void Shoot(int distance, float speed)
    {
        Projectile bullet = Instantiate(Projectile, shootPoint.transform.position, shootPoint.transform.rotation);
        bullet.MaxDistance = distance;
        bullet.Speed = speed;
    }

    public void ShowThinking(bool show)
    {
        //ThinkingObj.SetActive(show);
    }

    public void Rotate(int angle)
    {
        targetRotation *= Quaternion.AngleAxis(angle, Vector3.up);
    }

    public void MoveForward()
    {
        var movement = transform.rotation * Vector3.forward;
        targetPosition += Helper.GridVector(movement);
    }
}

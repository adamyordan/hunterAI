using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject projectile;
    public GameObject shootPoint;
    public GameObject thinking;

    public void Shoot()
    {
        GameObject bullet = Instantiate(projectile, shootPoint.transform.position, Quaternion.identity) as GameObject;
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 1000f);
    }

    public void ShowThinking(bool show)
    {
        thinking.SetActive(show);
    }
}

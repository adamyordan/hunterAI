using UnityEngine;

public class HunterController : MonoBehaviour
{
    public bool IsEnabled;
    public int ProjectileDistance;
    public float ProjectileSpeed;
    public Hunter TheHunter;

    void Update()
    {
        if (IsEnabled)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                TheHunter.Rotate(-90);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                TheHunter.Rotate(90);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                TheHunter.MoveForward();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TheHunter.Shoot(ProjectileDistance, ProjectileSpeed);
            }
        }
    }
}

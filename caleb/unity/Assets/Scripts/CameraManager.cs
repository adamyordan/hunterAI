using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera MinimapCamera;
    public Camera MainCamera;

    GridManager gridManager;
    Vector3 velocityMain;
    Vector3 velocityMinimap;

    void Start()
    {
        gridManager = Object.FindObjectOfType<GridManager>();
    }

    void Update()
    {
        Vector2Int mapSize = gridManager.mapSize;
		float maxSize = Mathf.Max(mapSize.x, mapSize.y);

        MinimapCamera.transform.position = Vector3.SmoothDamp(
            MinimapCamera.transform.position,
            new Vector3(0, maxSize / 2 * 1.2f, 0),
            ref velocityMinimap, 0.5f);

        MainCamera.transform.position = Vector3.SmoothDamp(
            MainCamera.transform.position,
            new Vector3(maxSize * 0.7f, maxSize * 0.9f, maxSize * -0.8f),
            ref velocityMain, 0.5f);
    }
}

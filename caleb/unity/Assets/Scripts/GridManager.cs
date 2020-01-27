using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public Vector2Int mapSize;
    public bool generateInEditor;

    readonly string holderName = "Generated Grid";
    GameObject[,] grids;

    public void GenerateGrid(bool inEditor)
    {
        if (inEditor && !generateInEditor)
        {
            return;
        }

        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        GameObject holder = new GameObject(holderName);
        holder.transform.parent = transform;
        grids = new GameObject[mapSize.x, mapSize.y];
 
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = new Vector3(-mapSize.x / 2 + x, 0, -mapSize.y / 2 + y);
                GameObject newTile = Instantiate<GameObject>(tilePrefab, tilePosition, Quaternion.identity);
                newTile.transform.parent = holder.transform;
                newTile.transform.Find("Floor").localScale = Vector3.one * (0.9f);
                grids[x, y] = newTile;
            }
        }
    }

    public GameObject Grid(Vector2Int pos)
    {
        if (IsWithinGrid(pos))
        {
            Vector2Int index = TranslatePosition(pos);
            return grids[index.x, index.y];
        } else
        {
            return null;
        }
    }

    public Vector2Int TranslatePosition(Vector2Int pos)
    {
        return new Vector2Int(mapSize.x / 2 + pos.x, mapSize.y / 2 + pos.y);
    }

    public bool IsWithinGrid(Vector2Int pos)
    {
        Vector2Int index = TranslatePosition(pos);
        return index.x >= 0 && index.y >= 0 && index.x < mapSize.x && index.y < mapSize.y;
    }

    public void Highlight(List<Vector2Int> visions)
    {
        foreach (GameObject grid in grids) {
            grid.transform.Find("Floor").GetComponent<Renderer>().material.color = Color.white;
        }

        foreach (Vector2Int vision in visions)
        {
            Grid(vision).transform.Find("Floor").GetComponent<Renderer>().material.color = new Color(1f, 0.75f, 0.75f, 1f);
        }
    }

    public Vector2Int RandomPos()
    {
        int x = -mapSize.x / 2 + Random.Range(0, mapSize.x);
        int y = -mapSize.y / 2 + Random.Range(0, mapSize.y);
        return new Vector2Int(x, y);
    }
}

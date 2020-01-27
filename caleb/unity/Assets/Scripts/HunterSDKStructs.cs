using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class State
{
    public List<GridWithContent> Vision;
    public int MonsterCount;
    public Vector2Int MapSize;
    public Vector2Int HunterPosition;
    public int HunterRotation;
    public int HunterProjectileDistance;
}

[System.Serializable]
public class StepResponse
{
    public AgentAction action;
}

[System.Serializable]
public class AgentAction
{
    public string id;
}

[System.Serializable]
public class GridWithContent
{
	public Vector2Int Position;
	public GridContent Content;

    public GridWithContent(Vector2Int position, GridContent content)
    {
        this.Position = position;
        this.Content = content;
    }
}

public enum GridContent
{
    Empty = 0,
    Monster = 1,
}

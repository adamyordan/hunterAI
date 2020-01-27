using System.Collections.Generic;
using UnityEngine;


public class Simulator : MonoBehaviour
{
    public Monster MonsterPrefab;
    public Hunter HunterPrefab;

    public GridManager TheGridManager;

    public float SdkStepInterval;
    public string SdkHost = "http://127.0.0.1:5000/";
    public bool SdkAiEnabled;

    public bool HumanControlEnabled;

    public float HunterSpeed = 10.0f;
    public int HunterVisionRange = 3;
    public int HunterProjectileDistance = 2;
    public float HunterProjectileSpeed = 20.0f;
    public bool HunterShowVision = true;

    public int MaxMonster;
    public int InitialMonster;

    // Not editor parameter
    public Hunter TheHunter;

    HunterSDK<State, StepResponse> hunterSDK;
    HunterController hunterController;
    List<Monster> monsterObjs;
    List<Vector2Int> hunterVisionGrids;

    bool initiated;

    void Start()
    {
    }

    void Update()
    {
        if (initiated)
        {
            UpdateHunterVision(TheHunter.targetPosition, TheHunter.targetRotation);
        }
    }

    public void Initiate()
    {
        TheHunter = Instantiate(HunterPrefab, new Vector3(0f, 3f, 0f), Quaternion.identity);
        TheHunter.Speed = HunterSpeed;

        TheGridManager.GenerateGrid(false);
        hunterVisionGrids = new List<Vector2Int>();
        monsterObjs = new List<Monster>();

        for (int i = 0; i < InitialMonster; i++)
        {
            SpawnMonster();
        }

        if (HumanControlEnabled)
        {
            GameObject hunterCtlObj = new GameObject("HunterController");
            hunterCtlObj.transform.parent = transform;
            hunterController = hunterCtlObj.AddComponent<HunterController>();
            hunterController.ProjectileDistance = HunterProjectileDistance;
            hunterController.ProjectileSpeed = HunterProjectileSpeed;
            hunterController.IsEnabled = HumanControlEnabled;
            hunterController.TheHunter = TheHunter;
        }

        if (SdkAiEnabled)
        {
            hunterSDK = new HunterSDK<State, StepResponse>(this, SdkHost);
            hunterSDK.Initiate();
            InvokeRepeating("Step", 2.0f, SdkStepInterval);
        }

        initiated = true;
    }

    void Step()
    {
        if (SdkAiEnabled)
        {
            TheHunter.ShowThinking(true);
            State state = GetState();
            hunterSDK.Step(state, (stepResponse) =>
            {
                AgentAction action = stepResponse.action;
                ActuateAction(action);
                Helper.RunLater(this, () => TheHunter.ShowThinking(false), 0.1f);
            });
        }
    }

    public State GetState()
    {
        List<GridWithContent> vision = new List<GridWithContent>();
        foreach (Vector2Int pos in hunterVisionGrids)
        {
            GridContent content = GridContent.Empty;
            foreach (Monster monster in monsterObjs)
            {
                Vector2Int monsterPos = Helper.GridVector(monster.transform.position);
                if (monsterPos.Equals(pos))
                {
                    content = GridContent.Monster;
                    break;
                }
            }
            vision.Add(new GridWithContent(pos, content));
        }

        State state = new State();
        state.Vision = vision;
        state.MonsterCount = monsterObjs.Count;
        state.MapSize = TheGridManager.mapSize;
        state.HunterPosition = TheHunter.targetPosition;
        state.HunterRotation = Mathf.RoundToInt(TheHunter.targetRotation.eulerAngles.y);
        state.HunterProjectileDistance = HunterProjectileDistance;

        return state;
    }

    public void ActuateAction(AgentAction action)
    {
        print(action.id);
        if (action.id == "shoot")
        {
            TheHunter.Shoot(HunterProjectileDistance, HunterProjectileSpeed);
        }
        else if(action.id == "rotate_left")
        {
            TheHunter.Rotate(-90);
        }
        else if(action.id == "rotate_right")
        {
            TheHunter.Rotate(90);
        }
        else if (action.id == "move_forward")
        {
            TheHunter.MoveForward();
        }
    }

    public void SpawnMonster()
    {
        if (monsterObjs.Count < MaxMonster)
        {
            Vector2Int pos = GetRandomEmptyGrid();
            int rotation = Random.Range(0, 4) * 90;
            Monster monsterObj = Instantiate(MonsterPrefab, new Vector3(pos.x, 6, pos.y), Quaternion.Euler(0, rotation, 0));
            monsterObj.DestroyedCallback = () => monsterObjs.Remove(monsterObj);
            monsterObjs.Add(monsterObj);
        }
    }

    public Vector2Int GetRandomEmptyGrid()
    {
        Vector2Int pos;
        bool ok;
        do
        {
            pos = TheGridManager.RandomPos();
            if (TheHunter.targetPosition == pos)
            {
                ok = false;
            }
            else
            {
                foreach (Monster obj in monsterObjs)
                {
                    if (Helper.GridVector(obj.transform.position).Equals(pos))
                    {
                        ok = false;
                        break;
                    }
                }
                ok = true;
            }
        } while (!ok);
        return pos;
    }

    public void SetAIEnabled(bool isEnabled)
    {
        SdkAiEnabled = isEnabled;
    }

    public void UpdateHunterVision(Vector2 position, Quaternion rotation)
    {
        hunterVisionGrids.Clear();

        Vector2Int forward = Helper.GridVector(rotation * Vector3.forward);
        Vector2Int positionInt = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        for (int i = 0; i <= HunterVisionRange; i++)
        {
            Vector2Int straightPos = positionInt + (forward * i);
            if (TheGridManager.IsWithinGrid(straightPos))
            {
                hunterVisionGrids.Add(straightPos);

                for (int j = 1; j <= i; j++)
                {
                    Vector2Int leftPos = straightPos - new Vector2Int(j * forward.y, j * forward.x);
                    Vector2Int rightPos = straightPos + new Vector2Int(j * forward.y, j * forward.x);
                    if (TheGridManager.IsWithinGrid(leftPos))
                    {
                        hunterVisionGrids.Add(leftPos);
                    }
                    if (TheGridManager.IsWithinGrid(rightPos))
                    {
                        hunterVisionGrids.Add(rightPos);
                    }
                }
            }
        }

        if (HunterShowVision)
        {
            TheGridManager.Highlight(hunterVisionGrids);
        }
    }
}

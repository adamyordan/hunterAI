using UnityEngine;


public class SimulatorScript : MonoBehaviour
{

    public GameObject monster;
    public GameObject player;
    public float stepInterval;

    bool aiEnabled = true;
    HunterSDK hunterSDK;
    PlayerScript playerScript;


    void Start()
    {
		hunterSDK = gameObject.GetComponent<HunterSDK>();
        playerScript = player.GetComponent<PlayerScript>();
        hunterSDK.Initiate();
        InvokeRepeating("Step", stepInterval, stepInterval);
    }

    void Update()
    {

    }

    void Step()
    {
        if (aiEnabled)
        {
            playerScript.ShowThinking(true);
            GameObject monsterGameObject = GameObject.FindGameObjectWithTag("Monster");

            State state = new State();
            state.monster_visible = monsterGameObject != null
                && Vector3.Distance(player.transform.position, monsterGameObject.transform.position) <= 4.0f;

            hunterSDK.Step(state, (stepResponse) =>
            {
                AgentAction agentAction = stepResponse.action;
                if (agentAction.id == "shoot")
                {
                    playerScript.Shoot();
                }
                Helper.RunLater(this, () => playerScript.ShowThinking(false), 0.1f);
            });
        }
    }

    public void SpawnMonster()
    {
        GameObject monsterGameObject = GameObject.FindGameObjectWithTag("Monster");
        if (monsterGameObject == null)
        {
            Instantiate(monster, new Vector3(2, 6, 0), transform.rotation);
        }
    }

    public void SetAIEnabled(bool isEnabled)
    {
        aiEnabled = isEnabled;
    }
}

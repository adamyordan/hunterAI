using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InteractionManager : MonoBehaviour
{
    public Simulator TheSimulator;
    public GridManager TheGridManager;

    public GameObject StartPlaceholder;
    public InputField InputGridSize;
    public InputField InputInitialEnemy;
    public InputField InputMaxEnemy;
    public InputField InputSdkHost;
    public InputField InputSdkStepInterval;
    public InputField InputHunterVisionRange;
    public InputField InputHunterProjectileDistance;
    public Toggle ToggleShowHunterVision;
    public Button ButtonStartSimulation;

    public GameObject RuntimePlaceholder;
    public Toggle ToogleEnableAI;
    public Button ButtonSpawnMonster;
    public Button ButtonReset;

    public string DefaultGridSize;
    public string DefaultInitialEnemy;
    public string DefaultMaxEnemy;
    public string DefaultSdkHost = "http://127.0.0.1:5000/";
    public string DefaultSdkStepInterval = "0.1";
    public string DefaultHunterVisionRange;
    public string DefaultHunterProjectileDistance;
    public bool DefaultShowHunterVision;


    void Start()
    {
        ButtonStartSimulation.onClick.AddListener(() =>
        {
            TheSimulator.InitialMonster = int.Parse(InputInitialEnemy.text);
            TheSimulator.MaxMonster = int.Parse(InputMaxEnemy.text);
            TheSimulator.SdkHost = InputSdkHost.text;
            TheSimulator.SdkStepInterval = float.Parse(InputSdkStepInterval.text);
            TheSimulator.HunterVisionRange = int.Parse(InputHunterVisionRange.text);
            TheSimulator.HunterProjectileDistance = int.Parse(InputHunterProjectileDistance.text);
            TheSimulator.HunterShowVision = ToggleShowHunterVision.isOn;
            TheSimulator.Initiate();

            StartPlaceholder.SetActive(false);
            RuntimePlaceholder.SetActive(true);
        });

        InputGridSize.onEndEdit.AddListener(GenerateGrid);
        ToogleEnableAI.onValueChanged.AddListener(TheSimulator.SetAIEnabled);
        ButtonSpawnMonster.onClick.AddListener(TheSimulator.SpawnMonster);
        ButtonReset.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));

        InputGridSize.text = DefaultGridSize;
        InputInitialEnemy.text = DefaultInitialEnemy;
        InputMaxEnemy.text = DefaultMaxEnemy;
        InputSdkHost.text = DefaultSdkHost;
        InputSdkStepInterval.text = DefaultSdkStepInterval;
        InputHunterVisionRange.text = DefaultHunterVisionRange;
        InputHunterProjectileDistance.text = DefaultHunterProjectileDistance;
        ToggleShowHunterVision.isOn = DefaultShowHunterVision;

        StartPlaceholder.SetActive(true);
        RuntimePlaceholder.SetActive(false);

        GenerateGrid(InputGridSize.text);
    }

    void GenerateGrid(string text)
    {
        string[] splitted = text.Split(',');
        int x = int.Parse(splitted[0]);
        int y = int.Parse(splitted[1]);
        TheGridManager.mapSize = new Vector2Int(x, y);
        TheGridManager.GenerateGrid(false);
    }
}

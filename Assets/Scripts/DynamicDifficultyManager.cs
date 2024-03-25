using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class DynamicDifficultyManager : MonoBehaviour
{
    private GameObject[] NPCs;
    
    public GameObject[] xBridges,zBridges;

    public float npcSpeed = 3f;
    public float npcRadius = 5f;
    
    
    private float checkUserProgressTimer = 0f;
    private float hpCheckTimer = 0f;
    private float hpCheckInterval = 5f;
    private float checkUserProgressInterval = 5f;
    private int hpSpawnPeriod = 10;

    private bool userIsHighlySkilled;

    [SerializeField, Range(1, 10)]
    public int userSkillLevel = 5;
    
    public enum Difficulty
    {
        Easy = 1,
        Medium = 2,
        Hard = 3
    }

    public Difficulty difficulty = Difficulty.Easy;
    
    private int npcSpawnPeriod = 10;
    
    
    
    
    void ChangeNPCSpeed()
    {
        foreach (GameObject npc in NPCs)
        {
            npc.GetComponent<EnemyBehaviour>().ChangeSpeed(npcSpeed);
        }
    }
    
    
    void ChangeNPCRadius()
    {
        foreach (GameObject npc in NPCs)
        {
            npc.GetComponent<EnemyBehaviour>().ChangeHearingDistance(npcRadius);
        }
    }
    
    void ChangeNPCSpawnPeriod()
    {
        GameObject.FindObjectOfType<NPCSpawner>().ChangeSpawnPeriod(npcSpawnPeriod);
    }
    
    public void ChangeXBridgeSize(float size)
    {
        foreach (GameObject bridge in xBridges)
        {
            var currentScale = bridge.transform.localScale;
            bridge.transform.localScale = new Vector3(currentScale.x, currentScale.y, size);
            // var navMeshSurface = bridge.GetComponent<NavMeshSurface>();
            // navMeshSurface.BuildNavMesh();
        }
    }
    
    public void ChangeZBridgeSize(float size)
    {
        foreach (GameObject bridge in zBridges)
        {
            var currentScale = bridge.transform.localScale;
            bridge.transform.localScale = new Vector3(size, currentScale.y, currentScale.z);
            // var navMeshSurface = bridge.GetComponent<NavMeshSurface>();
            // navMeshSurface.BuildNavMesh();
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        NPCs = GameObject.FindGameObjectsWithTag("NPC");
        checkUserProgressInterval = 20f;
        hpCheckInterval = 20f;
        ChangeDifficulty();

    }

    // Update is called once per frame
    void Update()
    {
        hpCheckTimer += Time.deltaTime;
        checkUserProgressTimer += Time.deltaTime;
        
        if (hpCheckTimer > hpCheckInterval)
        {
            hpCheckTimer = 0f;
            CheckUserHP();
        }
        
        if (checkUserProgressTimer > checkUserProgressInterval)
        {
            checkUserProgressTimer = 0f;
            CheckUserProgress();
        }
        
    }

    private void CheckUserHP()
    {
        Debug.Log("Checking user HP");
    }

    private void CheckUserProgress()
    {
        if (userSkillLevel < 3)
        {
            difficulty = Difficulty.Easy;
        }else if (userSkillLevel < 7)
        {
            difficulty = Difficulty.Medium;
        }
        else
        {
            difficulty = Difficulty.Hard;
        }
        
        ChangeDifficulty();
    }
    
    void ChangeDifficulty()
    {
        print("Current Level: " + userSkillLevel + " Difficulty: " + difficulty);
        switch (difficulty)
        {
            case Difficulty.Easy:
                npcSpeed = 2.5f;
                npcRadius = 3.5f;
                npcSpawnPeriod = 60;
                hpSpawnPeriod = 10;
                ChangeXBridgeSize(10f);
                ChangeZBridgeSize(10f);
                break;
            case Difficulty.Medium:
                npcSpeed = 4.5f;
                npcRadius = 7f;
                npcSpawnPeriod = 30;
                hpSpawnPeriod = 20;
                ChangeXBridgeSize(5f);
                ChangeZBridgeSize(5f);
                break;
            case Difficulty.Hard:
                npcSpeed = 6.5f;
                npcRadius = 10f;
                npcSpawnPeriod = 10;
                hpSpawnPeriod = 30;
                ChangeXBridgeSize(2f);
                ChangeZBridgeSize(2f);
                break;
        }
        NPCs = GameObject.FindGameObjectsWithTag("NPC");
        ChangeNPCSpeed();
        ChangeNPCRadius();
        ChangeNPCSpawnPeriod();
        ChangeHPSpawnPeriod();
    }

    private void ChangeHPSpawnPeriod()
    {
        GameObject.FindObjectOfType<HealthPackSpawner>().ChangeSpawnPeriod(hpSpawnPeriod);
    }

    public void ChangePlayerSkillLevel(int delta, string cause)
    {
        userSkillLevel += delta;
        if (userSkillLevel < 1)
        {
            userSkillLevel = 1;
        }
        if (userSkillLevel > 10)
        {
            userSkillLevel = 10;
        }
        print($"Player Level is now {userSkillLevel} after reaching {cause}");
        
        //CheckUserProgress();
    }
}

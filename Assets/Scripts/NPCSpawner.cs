using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    
    public GameObject npcPrefab;
    public int numberOfNPCs = 10;
    public float spawnRate = 5;
    public GameObject spawnPoint;
    public float spawnRadius = 5;
    
    private float spawnTimer;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        var allNPCs = GameObject.FindGameObjectsWithTag("NPC");
        if (spawnTimer >= spawnRate && allNPCs.Length < numberOfNPCs)
        {
            var randomX = Random.Range(-spawnRadius, spawnRadius);
            var randomZ = Random.Range(-spawnRadius, spawnRadius);
            var spawnPosition = new Vector3(spawnPoint.transform.position.x + randomX, spawnPoint.transform.position.y, spawnPoint.transform.position.z + randomZ);
            Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
            spawnTimer = 0;
        }

        
    }

    public void ChangeSpawnPeriod(int npcSpawnPeriod)
    {
        spawnRate = npcSpawnPeriod;
    }
}

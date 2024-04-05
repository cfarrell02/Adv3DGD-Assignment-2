using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class PerlinGenerator : MonoBehaviour
{
    private float [,] map;
    [SerializeField, Range(10,100)]
    private int mapWidth = 10, mapHeight = 10;
    
    [SerializeField, Range(0,100)]
    private float blockSize = 10, blockHeight = 10, frequency = 1, scale = 1;
    
    public GameObject block;



    void InitArray()
    {
        map = new float[mapWidth, mapHeight];
        
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                float nx = x / mapWidth, ny = y / mapHeight;
                
                map[x, y] = Mathf.PerlinNoise(x*1.0f/frequency + 0.1f, y*1.0f/frequency + 0.1f);
                
            }
        }
    }

    void DisplayArray()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                float yVal = Mathf.Round(map[x, y] * blockHeight * scale);
                
                while(yVal % blockHeight != 0)
                {
                    yVal++;
                }
                
                GameObject newBlock = Instantiate(block, new Vector3(x * blockSize, yVal , y * blockSize), Quaternion.identity);
                newBlock.transform.localScale = new Vector3(blockSize, blockHeight, blockSize);
                newBlock.transform.parent = transform;
                
            }
        }
        
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        block.transform.localScale = new Vector3(blockSize, blockHeight, blockSize);
        InitArray();
        DisplayArray();
        
        var spawnPoint = GameObject.Find("SpawnPoint");
        //Place down the spawn point at the center of the map
        spawnPoint.transform.position = new Vector3(mapWidth * blockSize / 2, 0, mapHeight * blockSize / 2);
        
        var wps = GameObject.FindGameObjectsWithTag("Waypoint");
        for(int i = 0; i < wps.Length; i++)
        {
            var pos = new Vector3(Random.Range(0, mapWidth) * blockSize, 0, Random.Range(0, mapHeight) * blockSize);
            var ypos = Mathf.Round(map[(int)pos.x, (int)pos.z] * blockHeight * scale);
            while(ypos % blockHeight != 0)
            {
                ypos++;
            }
            
            wps[i].transform.position = new Vector3(pos.x, ypos+1, pos.z);
            
        }
        
        var royalAdvisor = GameObject.Find("Royal Advisor");
        var pos1 = new Vector3(Random.Range(0, mapWidth) * blockSize, 0, Random.Range(0, mapHeight) * blockSize);
        var ypos1 = Mathf.Round(map[(int)pos1.x, (int)pos1.z] * blockHeight * scale);
        while(ypos1 % blockHeight != 0)
        {
            ypos1++;
        }
        royalAdvisor.transform.position = new Vector3(pos1.x, ypos1+1, pos1.z);
        
        var castle = GameObject.Find("Castle");
        var pos2 = new Vector3(Random.Range(0, mapWidth) * blockSize, 0, Random.Range(0, mapHeight) * blockSize);
        var ypos2 = Mathf.Round(map[(int)pos2.x, (int)pos2.z] * blockHeight * scale);
        while(ypos2 % blockHeight != 0)
        {
            ypos2++;
        }
        castle.transform.position = new Vector3(pos2.x, ypos2+1, pos2.z);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
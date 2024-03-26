using System.Collections;
using System.Collections.Generic;
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
                
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        block.transform.localScale = new Vector3(blockSize, blockHeight, blockSize);
        InitArray();
        DisplayArray();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
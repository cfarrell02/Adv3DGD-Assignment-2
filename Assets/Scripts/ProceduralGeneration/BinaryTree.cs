using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class BinaryTree : MonoBehaviour
{
    public enum Direction
    {
        North = 1,
        South = 2,
        East = 3,
        West = 4
    }

    private Direction[,] grid;
    
    [SerializeField, Range(5,100)] 
    private int width = 10, height = 10, wallSize = 5, wallHeight = 4;
    
    
    public GameObject verticalWall, horizontalWall;
    private GameObject[,] gridObjectsH, gridObjectsV;
    private GameObject[] allObjectsInScene;
    
    public GameObject target, NPC;



    void Init()
    {
        height = width;
        
        verticalWall.transform.localScale = new Vector3(.1f, wallHeight, wallSize);
        horizontalWall.transform.localScale = new Vector3(wallSize, wallHeight, .1f);
        
        grid = new Direction[width, height];
        gridObjectsH = new GameObject[width+1, height+1];
        gridObjectsV = new GameObject[width+1, height+1];

        var ground = GameObject.Find("Ground");
        ground.transform.localScale = new Vector3((width) * wallSize, 1, (height) * wallSize);
        ground.transform.position = new Vector3(-(wallSize+.1f)/2, 0, -(wallSize+.1f)/2);

        var groundMesh = ground.GetComponent<NavMeshSurface>();
        groundMesh.RemoveData();
        groundMesh.BuildNavMesh();
        
        
        // var ceiling = GameObject.Find("Ceiling");
        // ceiling.transform.localScale = new Vector3((width) * wallSize, 1 + wallHeight, (height) * wallSize);
        // ceiling.transform.position = new Vector3(-(wallSize+.1f)/2, wallHeight, -(wallSize+.1f)/2);
        //


    }



    void DrawGrid()
    {
        for(int x = 0; x <= width; x++)
        {
            for(int y = 0; y <= height; y++)
            {
                if (y < height)
                {
                    float vWallSize = verticalWall.transform.localScale.z;
                    float xOffset = -(width * vWallSize / 2);
                    float yOffset = -(height * vWallSize / 2);
                    
                    gridObjectsV[x,y] = Instantiate(verticalWall, new Vector3(
                        -vWallSize/2 + x*wallSize + xOffset, wallSize/2,y*vWallSize + yOffset), Quaternion.identity);
                    
                    gridObjectsV[x,y].SetActive(true);
                    gridObjectsV[x,y].name = "VWall " + x + ", " + y;
                    gridObjectsV[x,y].tag = "Wall";
                }

                if (x < width)
                {
                    float hWallSize = horizontalWall.transform.localScale.x;
                    float xOffset = -(width * hWallSize / 2);
                    float yOffset = -(height * hWallSize / 2);
                    
                    
                    gridObjectsH[x,y] = Instantiate(horizontalWall, new Vector3(
                        x*hWallSize + xOffset, wallSize/2, -hWallSize/2 + y*wallSize + yOffset), Quaternion.identity);
                    
                    gridObjectsH[x,y].SetActive(true);
                    gridObjectsH[x,y].name = "HWall " + x + ", " + y;
                    gridObjectsH[x,y].tag = "Wall";
                    
                }
                
                
            }
        }
    }


    void GenerateMazeBinary()
    {
        for(int row = 0; row < height; row++)
        {
            for(int col = 0; col < width; col++)
            {
                float randomNum = Random.Range(0, 100);
                Direction direction = randomNum > 30 ? Direction.North : Direction.East;

                if (col == width - 1)
                {
                    if(row < height - 1) direction = Direction.North;
                    else direction = Direction.West;
                    
                }else if (row == height - 1)
                {
                    if (col < width - 1) direction = Direction.East;
                    else break;
                }
                
                grid[col, row] = direction;
            }
        }       
    }

    void DisplayGrid()
    {
        for(int row = 0; row < height; row++)
        {
            for(int col = 0; col < width; col++)
            {
                if(grid[col, row] == Direction.North)
                {
                    gridObjectsH[col, row+1].SetActive(false);
                }else if (grid[col, row] == Direction.East)
                {
                    gridObjectsV[col+1, row].SetActive(false);
                }
                    
            }
        }
    }

    void AddTarget()
    {
        float xOffset = -(width * wallSize ) / 2;
        float yOffset = -(height * wallSize ) / 2;
        
        GameObject t = Instantiate(target, new Vector3(
            xOffset, 1, yOffset), Quaternion.identity);
        t.name = "Target";
        
    }
    
    void AddNPC()
    {
        float xOffset = (width-1) * wallSize / 2;
        float yOffset = (height-1) * wallSize / 2;
        
        GameObject t = Instantiate(NPC, new Vector3(
            xOffset, 1, yOffset), Quaternion.identity);
        t.name = "NPC";
        
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        Init();
        DrawGrid();
        GenerateMazeBinary();
        DisplayGrid();
        AddTarget();
        AddNPC();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

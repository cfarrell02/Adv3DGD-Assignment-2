using UnityEngine;
using UnityEngine.AI;


public class EnemyBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject player;
    public float chaseDistance = 10;
    public float attackDistance = 2;
    public float attackRate = 1;
    private float attackTimer;
    private GameObject[] waypoints;
    private int currentWaypointIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
    }
    
    // Update is called once per frame
    void Update()
    {
        var distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < chaseDistance)
        {
            agent.SetDestination(player.transform.position);
            if (distance < attackDistance)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackRate)
                {
                    var health = player.GetComponent<Health>();
                    health.TakeDamage(10);
                    attackTimer = 0;
                }
            }
        }
        else if(waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].transform.position);
            if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].transform.position) < 4)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    currentWaypointIndex = 0;
                }
            }
            
        }
    }

    public void ChangeHearingDistance(float npcRadius)
    {
        chaseDistance = npcRadius;
    }
    
    public void ChangeSpeed(float npcSpeed)
    {
        agent.speed = npcSpeed;
    }
}

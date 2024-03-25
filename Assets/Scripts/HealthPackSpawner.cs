using UnityEngine;


public class HealthPackSpawner : MonoBehaviour
{
    public GameObject healthPackPrefab;
    public float spawnRate = 5f;
    public float spawnRadius = 5f;
    private float nextSpawnTime;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnRate;
            SpawnHealthPack();
        }
    }

    void SpawnHealthPack()
    {
        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        spawnPosition.y = .77f;
        Instantiate(healthPackPrefab, spawnPosition, Quaternion.identity);
    }

    public void ChangeSpawnPeriod(int hpSpawnPeriod)
    {
        spawnRate = hpSpawnPeriod;
    }
}

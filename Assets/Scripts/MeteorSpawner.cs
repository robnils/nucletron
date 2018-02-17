using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{

    public float timeBetweenSpawns;

    public float spawnDistance;

    public Meteor[] meteorPrefabs;

    float timeSinceLastSpawn;

    void FixedUpdate()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= timeBetweenSpawns)
        {
            timeSinceLastSpawn -= timeBetweenSpawns;
            SpawnNucleon();
        }
    }

    void SpawnNucleon()
    {

        Meteor prefab = meteorPrefabs[Random.Range(0, meteorPrefabs.Length)];
        Meteor spawn = Instantiate<Meteor>(prefab);

        spawn.transform.localPosition = Random.onUnitSphere * spawnDistance;
    }
}
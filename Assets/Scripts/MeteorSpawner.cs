using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{

    public float timeBetweenSpawns;

    public float spawnDistance;

    public Meteor[] meteors;

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
        Meteor prefab = meteors[Random.Range(0, meteors.Length)];
        Meteor spawn = Instantiate<Meteor>(prefab);
        spawn.transform.localPosition = Random.onUnitSphere * spawnDistance;
    }
}
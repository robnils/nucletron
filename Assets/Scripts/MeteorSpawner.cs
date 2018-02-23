using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{

    public float timeBetweenSpawns;

    public float spawnDistance;

    public Meteor[] meteors;

    public Transform player;

    float timeSinceLastSpawn;

    void FixedUpdate()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= timeBetweenSpawns)
        {
            timeSinceLastSpawn -= timeBetweenSpawns;
            SpawnMeteor();
        }
    }

    void SpawnMeteor()
    {
        Meteor prefab = meteors[Random.Range(0, meteors.Length)];
        Meteor spawn = Instantiate<Meteor>(prefab);
        //var offset = new Vector3(player.localPosition.x, 0, 0);
        var spawnPosition = player.localPosition + Random.onUnitSphere * spawnDistance;
        spawn.transform.localPosition = spawnPosition;
        //Debug.Log("Spawned meteor at: {}" + spawnPosition);
    }
}
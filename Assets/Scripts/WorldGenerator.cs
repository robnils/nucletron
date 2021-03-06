﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class WorldGenerator : MonoBehaviour {

    public Transform platform;
    public Transform finish;
    public Transform fire;

    private List<Vector3> directions;
    private List<Transform> platforms;
	private Dictionary<Vector3, int> platformPositionIndexMap;

    public int startingLevel;
    private int currentLevel;

	private Quaternion nintyDegrees;

	// Fire
	private Dictionary<Transform, bool> fires;
    public float spawnFireProbabilityBase;
	private float spawnFireProbability;
	private const float SPAWN_FIRE_PROBABILITY_MAX = 0.9f;
	private const float SPAWN_FIRE_PROBABILITY_STEP = 0.05f;

    // Platform path
    private const float DISTANCE_BETWEEN_PLATFORMS_MIN = 1.0f;
    private const float DISTANCE_BETWEEN_PLATFORMS_MAX = 2.5f;
    private const int PATH_LENGTH_MIN = 3;
    private const int PATH_LENGTH_MAX = 6; 
    private const int PATH_HEIGHT_MIN = 1;
    private const int PATH_HEIGHT_MAX = 2;
	private Vector3 currentDirection;
	private Vector3[] directions2d = { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

	/*
	private Dictionary<string, bool> vectorHorizontalMap = new Dictionary<Vector3, bool>
	{
		{ Vector3.left.ToString, false },
		{ Vector3.right.ToString, false },
		{ Vector3.forward.ToString, true },
		{ Vector3.back.ToString, true }
	};
	*/

    // Stairs
    private const int MIN_DISTANCE_BETWEEN_STEPS = 7;
    private const int MAX_DISTANCE_BETWEEN_STEPS = 10;
    private const int STEP_HEIGHT_MIN = 1;
    private const int STEP_HEIGHT_MAX = 3;
    private const int NUMBER_OF_STEPS_MIN = 3;
    private const int NUMBER_OF_STEPS_MAX = 5;
    private const int STAIRS_DIRECTION = 1; // +/- 1

    void Start () {
        Assert.IsNotNull(this.platform, "Platform prefab not defined");
        Assert.IsNotNull(this.finish, "Finish prefab not defined");
        Assert.IsNotNull(this.finish, "Finish platform prefab not defined");
        
		currentLevel = startingLevel; 
		nintyDegrees = Quaternion.Euler(0, 90, 0);
        BuildWorld(currentLevel);
    }

    private int GetRandomPlusMinus() {
        var i = Random.Range(1, 3);

        if (i == 1) {
            return 1;
        } else {
            return -1;
        }
    }

    public void ClearWorld() {
        Debug.Log("Platforms: " + platforms);
        foreach (var t in platforms) {
            Debug.Log("Deleting: " + t);
            Destroy(t.gameObject); 
        }

		foreach (var k in fires.Keys) {
			Debug.Log("Deleting: " + k);
            if (k != null) {
                Destroy(k.gameObject);
            }
		}
    }

	private Transform SpawnFire(Vector3 position) {		
		var prefab = Instantiate(fire, position, Quaternion.identity);
		fires.Add(prefab, true);
		return prefab;
    }

	private bool shouldRotate(Vector3 dir) {
		return (dir == Vector3.left || dir == Vector3.right);
			
	}
	/// <summary>
	/// Spawns fire with a given probability but never the first or last platforms, with increasing time
	/// to live the further the platform is from the beginning
	/// </summary>
	/// <returns>The fire.</returns>
	/// <param name="position">Position.</param>
	/// <param name="probability">Probability.</param>
	private Transform SpawnFire(Vector3 position, float probability) {
        if (Random.Range(0.0f, 1.0f) <= probability) {
			
			int posIndex = platformPositionIndexMap [position];
			if (posIndex >= 2 && posIndex <= platforms.Count) {
				var firePrefab = SpawnFire(position);

				try {
					if (shouldRotate(currentDirection)) {
						firePrefab.rotation = nintyDegrees;
					}
				} catch (System.Exception e) {
					Debug.LogError("Unable to determine direction from: " + currentDirection + ", ");
				}

				var fireScript = (Fire)firePrefab.GetChild(0).GetComponent(typeof(Fire));

				var rand = Random.Range (4.0f, 6.0f);
				var timeToLive = rand * (float)(posIndex) * 0.75f;
				fireScript.SetTimeToLive(timeToLive); 
				Debug.Log ("Spawned fire: " + position + ", " + timeToLive);
				return firePrefab;
			}
			Debug.Log ("Position not suitable for fire spawn: " + posIndex); 
        }
		return null;
    }

    public void NextLevel() {
        ClearWorld();
        currentLevel++;
        BuildWorld(currentLevel);
    }

    public void RegenerateCurrentLevel()
    {
        ClearWorld();
        BuildWorld(currentLevel);
    }

    private void UpdateLevelText() {
        GameObject levelObject = GameObject.Find("Level");
        var txt = levelObject.GetComponent<Text>();
        txt.text = "Level : " + currentLevel;
    }

	private void calculateSpawnFireProbability(int level) {
		if (spawnFireProbability < SPAWN_FIRE_PROBABILITY_MAX) {
			spawnFireProbability = spawnFireProbabilityBase + SPAWN_FIRE_PROBABILITY_STEP * (float)level;
		} else {
			spawnFireProbability = SPAWN_FIRE_PROBABILITY_MAX;
		}
	}

    public Transform BuildWorld(int level) {
		Debug.Log("Building level: " + currentLevel);

        UpdateLevelText();

		fires = new Dictionary<Transform, bool>();
        platforms = new List<Transform>();
        directions = new List<Vector3>();
		platformPositionIndexMap = new Dictionary<Vector3, int>();

		calculateSpawnFireProbability(currentLevel);

        // TODO prevent collisions by forcing new direction generation if it the platform collides
        var main = CreatePlatform(Vector3.zero, this.platform);

        //var startingPlatform = BuildStairs(main.transform, 1, 7, GetRandomPlusMinus());
        var startingDirection = Vector3.forward;
        var platform = main;
        for (int idx = 0; idx < level; idx++)
        {
            var pathLength = Random.Range(PATH_LENGTH_MIN, PATH_LENGTH_MAX + 1);
            platform = BuildPath(platform, startingDirection, pathLength);

            var stairLength = Random.Range(NUMBER_OF_STEPS_MIN, NUMBER_OF_STEPS_MAX + 1);
            int distBetweenSteps = Random.Range(MIN_DISTANCE_BETWEEN_STEPS, MAX_DISTANCE_BETWEEN_STEPS);
            platform = BuildStairs(platform, stairLength, distBetweenSteps, 1);

            /*
            pathLength = Random.Range(PATH_LENGTH_MIN, PATH_LENGTH_MAX + 1); ;
            platform = BuildPath(platform, startingDirection, 4);

            stairLength = Random.Range(NUMBER_OF_STEPS_MIN, NUMBER_OF_STEPS_MAX + 1);
            platform = BuildStairs(platform, stairLength, DISTANCE_BETWEEN_STEPS, GetRandomPlusMinus());
            */

        }
        var finish = CreateFinish(platform.transform);
        return finish;
    }



    public Transform BuildWorld() {
        return BuildWorld(0);
    }

    private Vector3 GetNextDirection(Vector3 currentDirection) {
        Vector3 newDirection;
        if (directions.Count < 3) {
            Debug.Log("No loop yet, directions.Count: " + directions.Count);
            newDirection = GetRandomDirection(currentDirection);            
            directions.Add(newDirection);
            return newDirection;
        }

        // If any one of the directions are different, we donn't have a loop
        var firstDirection = directions[0];
        for (int idx = 1; idx < directions.Count; idx++) {
            if (directions[idx] != firstDirection) {
                Debug.Log("Verge of loop but avoided, directions.Count: " + directions.Count);
                directions.Clear();
                newDirection = GetRandomDirection(currentDirection);            
                directions.Add(newDirection);
                return newDirection;
            }
        }

        // All directions are the same, so we need to make sure the next direction
        // is differennt to prevent a loop
        Debug.Log("Loop reached at directions.Count: " + directions.Count);
        do {
            newDirection = GetRandomDirection(currentDirection);            
        } while (newDirection != firstDirection);
        directions.Clear();
        directions.Add(newDirection);
        return newDirection;

    }

    private Transform BuildPath(Transform startPlatform, Vector3 startDirection, int depth) {
        currentDirection = startDirection;
        var platform = startPlatform;

        for (int idx = 0; idx < depth; idx++) {
            Debug.Log("Building: " + currentDirection);
            var height = Random.Range(PATH_HEIGHT_MIN, PATH_HEIGHT_MAX);
            var newPlatform = BuildInDirection(platform, currentDirection, this.platform, height, 2.0f);

            if (platforms.Count > 3) {
                SpawnFire(newPlatform.transform.localPosition, spawnFireProbability);
            }

            //var newDirection = GetRandomDirection(currentDirection);            
            var newDirection = GetNextDirection(currentDirection);
            currentDirection = newDirection;
            platform = newPlatform; 
        }
        return platform;
    }

    private Vector3 GetBackwardsDirectionFromPrevious(Vector3 previousDirection) {
        if (previousDirection == Vector3.back) {
            return Vector3.forward;
        } else if (previousDirection == Vector3.forward) {
            return Vector3.back;
        } else if (previousDirection == Vector3.left) {
            return Vector3.right;
        } else if (previousDirection == Vector3.right) {
            return Vector3.left;
        } else {
            Debug.LogError("Unexpected vector3 given," + previousDirection);
            return Vector3.zero;
        }
    }

    private Vector3 GetRandomDirection(Vector3 previousDirection) {
        var newDirection = directions2d[Random.Range(0, directions2d.Length)];
        while (true) {
            if (newDirection != GetBackwardsDirectionFromPrevious(previousDirection)) {
                break;
            }
            newDirection = directions2d[Random.Range(0, directions2d.Length)];
        }
        return newDirection;
    }

    private Transform BuildInDirection(Transform platform, Vector3 direction, Transform prefab,  int height, float variation) {
        var x = platform.localPosition.x;
        var y = platform.localPosition.y;
        var z = platform.localPosition.z;

        var width = platform.localScale.x;
        var distanceBetweenPlatforms = Random.Range(DISTANCE_BETWEEN_PLATFORMS_MIN, DISTANCE_BETWEEN_PLATFORMS_MAX);
        var newPosition = platform.localPosition + direction * (width + distanceBetweenPlatforms);
        newPosition.y += height;

        float xVariation = Random.Range(-variation, variation);
        float yVariation = Random.Range(-variation, variation);
        newPosition.x += xVariation;

        var sizeVariation = Random.Range(-1.0f, 1.0f); ;
        //newPosition.z += yVariation;
        var newPlatform = CreatePlatform(newPosition, prefab);
        newPlatform.transform.localScale += new Vector3(xVariation, 0, 0);

        platform = newPlatform;
        return platform;
    }

    private Transform BuildInDirection(Transform platform, Vector3 direction, Transform prefab, int height) {
        return BuildInDirection(platform, direction, prefab, height, 0.0f);
    }

    private Transform BuildInDirection(Transform platform, Vector3 direction, Transform prefab) {
        return BuildInDirection(platform, direction, prefab, 0, 0.0f);
    }

    private Transform BuildInDirection(Transform platform, Vector3 direction) {
        return BuildInDirection(platform, direction, this.platform);
    }

    private Transform CreatePlatform(Vector3 position, Transform prefab) {
        var newPlatform = Instantiate(prefab, position, Quaternion.identity);
        platforms.Add(newPlatform);
		platformPositionIndexMap.Add(position, platforms.Count - 1);
        return newPlatform;
    }

    private float widthOfPrefab(Transform prefab) {
        var mesh = prefab.GetComponent<MeshCollider>();
        var bounds = mesh.bounds;
        var size = bounds.size;
        var width = size.x * 0.5f;
        return width;
    }

    private Transform CreateFinish(Transform position) {
        var finalPlatform = BuildInDirection(position, Vector3.forward, this.finish);
        finalPlatform.Rotate(0, 90, 0);
        platforms.Add(finalPlatform);

        /*
        var finalPosition = finalPlatform.localPosition;
        var finish = Instantiate(this.finish, finalPosition, Quaternion.identity);
        finish.Rotate(0, 90, 0);

       
        var mesh = finish.GetComponent<MeshCollider>();
        var bounds = mesh.bounds;
        var size = bounds.size;
        var finishWidth = size.x * 0.5f;
       
        var finishWidth = widthOfPrefab(finish);
        var platformWidth = widthOfPrefab(platform);
        Debug.Log("platform width: " + platformWidth);

        var pillarPositionRight = finalPlatform.localPosition + Vector3.right * finishWidth + Vector3.back*platformWidth;
        var pillarPositionLeft = finalPlatform.localPosition + Vector3.left * finishWidth + Vector3.back * platformWidth;
        Instantiate(this.finishPillar, pillarPositionRight, Quaternion.identity);
        Instantiate(this.finishPillar, pillarPositionLeft, Quaternion.identity);
        */
        return finalPlatform;
    }

    private Transform BuildStairs(Transform platform, int numberOfSteps, int distanceBetweenSteps, int stairsDirection) {
        
        for (int idx = 0; idx < numberOfSteps; idx++) {
            var x = platform.localPosition.x;
            var y = platform.localPosition.y;
            var z = platform.localPosition.z;

            var xScale = platform.localScale.x;
            var yScale = platform.localScale.y;
            var zScale = platform.localScale.z;

            int stepHeight = Random.Range(STEP_HEIGHT_MIN, STEP_HEIGHT_MAX);
            var newPosition = new Vector3(x + (int)(xScale * 0.5f) + distanceBetweenSteps, (stairsDirection) * (y + stepHeight), z);

            var newPlatform = CreatePlatform(newPosition, this.platform);
            platform = newPlatform;
        }
        return platform;
    }
}



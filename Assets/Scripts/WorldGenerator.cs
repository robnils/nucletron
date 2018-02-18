using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {

    public Transform platform;
    private List<Transform> platforms;

    // Platform path
    private const float MAX_DISTANCE_BETWEEN_PLATFORMS = 3.0f;
    private const float MIN_DISTANCE_BETWEEN_PLATFORMS = 1.0f;

    // Stairs
    private const int DISTANCE_BETWEEN_STEPS = 7;
    private const int STEP_HEIGHT_MIN = 1;
    private const int STEP_HEIGHT_MAX = 3;
    private const int STAIRS_DIRECTION = 1; // +/- 1

    private Vector3[] directions2d = { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

    void Start () {
        platforms = new List<Transform>();
        //var main = GameObject.FindGameObjectWithTag("Main");
        var main = CreatePlatform(Vector3.zero);


        var startingPlatform = BuildStairs(main.transform, 1, 7);
        var startingDirection = Vector3.forward;

        var platform = BuildPath(startingPlatform, startingDirection, 5);
        platform = BuildStairs(platform, 3, DISTANCE_BETWEEN_STEPS);
        platform = BuildPath(platform, startingDirection, 4);
    }

    private Transform BuildPath(Transform startPlatform, Vector3 startDirection, int depth) {
        var current = startDirection;
        var platform = startPlatform;

        for (int idx = 0; idx < depth; idx++) {
            Debug.Log("Building: " + current);
            var newPlatform = BuildSomeDirection(platform, current);
            var newDirection = GetRandomDirection(current);            
            current = newDirection;
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
    
    private Transform BuildSomeDirection(Transform platform, Vector3 direction) {
        var x = platform.localPosition.x;
        var y = platform.localPosition.y;
        var z = platform.localPosition.z;

        var width = platform.localScale.x;
        var distanceBetweenPlatforms = Random.Range(MIN_DISTANCE_BETWEEN_PLATFORMS, MAX_DISTANCE_BETWEEN_PLATFORMS);
        //var newPosition = new Vector3(x + direction.x, y + direction.y, z + direction.z);
        var newPosition = platform.localPosition + direction * (width + distanceBetweenPlatforms);

        //var newPlatform = Instantiate(this.platform, newPosition, Quaternion.identity);
        //platforms.Add(newPlatform);
        var newPlatform = CreatePlatform(newPosition);
        platform = newPlatform;
        return platform;
    }

    private Transform CreatePlatform(Vector3 position) {
        var newPlatform = Instantiate(this.platform, position, Quaternion.identity);
        platforms.Add(newPlatform);
        return newPlatform;
    }

    private Transform BuildStairs(Transform platform, int numberOfSteps, int distanceBetweenSteps) {
        
        for (int idx = 0; idx < numberOfSteps; idx++) {
            var x = platform.localPosition.x;
            var y = platform.localPosition.y;
            var z = platform.localPosition.z;

            var xScale = platform.localScale.x;
            var yScale = platform.localScale.y;
            var zScale = platform.localScale.z;

            int stepHeight = Random.Range(STEP_HEIGHT_MIN, STEP_HEIGHT_MAX);
            var newPosition = new Vector3(x + (int)(xScale * 0.5f) + distanceBetweenSteps, (STAIRS_DIRECTION) * (y + stepHeight), z);

            //var newPlatform = Instantiate(this.platform, newPosition, Quaternion.identity);
            //platforms.Add(newPlatform);
            var newPlatform = CreatePlatform(newPosition);
            platform = newPlatform;
        }
        return platform;
    }
}



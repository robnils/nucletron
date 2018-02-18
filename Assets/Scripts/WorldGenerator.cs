﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {

    public Transform platform;
    private List<Transform> platforms;

    // Platform path
    private const float MAX_DISTANCE_BETWEEN_PLATFORMS = 3.0f;
    private const float MIN_DISTANCE_BETWEEN_PLATFORMS = 1.0f;
    private const int PATH_LENGTH_MIN = 3;
    private const int PATH_LENGTH_MAX = 6;

    // Stairs
    private const int DISTANCE_BETWEEN_STEPS = 7;
    private const int STEP_HEIGHT_MIN = 1;
    private const int STEP_HEIGHT_MAX = 3;
    private const int NUMBER_OF_STEPS_MIN = 4;
    private const int NUMBER_OF_STEPS_MAX = 12;
    private const int STAIRS_DIRECTION = 1; // +/- 1

    private Vector3[] directions2d = { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

    void Start () {
        platforms = new List<Transform>();
        BuildWorld();
    }

    private int GetRandomPlusMinus() {
        var i = Random.Range(1, 3);

        if (i == 1) {
            return 1;
        } else {
            return -1;
        }
    }
    private Transform BuildWorld() {
        var main = CreatePlatform(Vector3.zero);

        var startingPlatform = BuildStairs(main.transform, 1, 7, GetRandomPlusMinus());
        var startingDirection = Vector3.forward;

        int level = 1;
        for (int idx = 0; idx < level; idx++) {
            var pathLength = Random.Range(PATH_LENGTH_MIN, PATH_LENGTH_MAX + 1);
            var platform = BuildPath(startingPlatform, startingDirection, pathLength);

            var stairLength = Random.Range(NUMBER_OF_STEPS_MIN, NUMBER_OF_STEPS_MAX + 1);
            platform = BuildStairs(platform, stairLength, DISTANCE_BETWEEN_STEPS, GetRandomPlusMinus());

            pathLength = Random.Range(PATH_LENGTH_MIN, PATH_LENGTH_MAX + 1); ;
            platform = BuildPath(platform, startingDirection, 4);

            stairLength = Random.Range(NUMBER_OF_STEPS_MIN, NUMBER_OF_STEPS_MAX + 1);
            platform = BuildStairs(platform, stairLength, DISTANCE_BETWEEN_STEPS, GetRandomPlusMinus());
        }

        return platform;
    }

    private Transform BuildPath(Transform startPlatform, Vector3 startDirection, int depth) {
        var current = startDirection;
        var platform = startPlatform;

        for (int idx = 0; idx < depth; idx++) {
            Debug.Log("Building: " + current);
            var height = Random.Range(-2, 2);
            var newPlatform = BuildInDirection(platform, current, height);
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
    
    private Transform BuildInDirection(Transform platform, Vector3 direction, int height) {
        var x = platform.localPosition.x;
        var y = platform.localPosition.y;
        var z = platform.localPosition.z;

        var width = platform.localScale.x;
        var distanceBetweenPlatforms = Random.Range(MIN_DISTANCE_BETWEEN_PLATFORMS, MAX_DISTANCE_BETWEEN_PLATFORMS);
        var newPosition = platform.localPosition + direction * (width + distanceBetweenPlatforms);
        newPosition.y += height;
        
        var newPlatform = CreatePlatform(newPosition);
        platform = newPlatform;
        return platform;
    }

    private Transform BuildInDirection(Transform platform, Vector3 direction) {
        return BuildInDirection(platform, direction);
    }

    private Transform CreatePlatform(Vector3 position) {
        var newPlatform = Instantiate(this.platform, position, Quaternion.identity);
        platforms.Add(newPlatform);
        return newPlatform;
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

            var newPlatform = CreatePlatform(newPosition);
            platform = newPlatform;
        }
        return platform;
    }
}



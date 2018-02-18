﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {

    public Transform platform;
    private List<Transform> platforms;

    private const float MAX_DISTANCE_BETWEEN_PLATFORMS = 3.0f;
    private const float MIN_DISTANCE_BETWEEN_PLATFORMS = 1.0f;
    private Vector3[] directions2d = { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

    void Start () {
        platforms = new List<Transform>();
        var main = GameObject.FindGameObjectWithTag("Main");

        var platform = BuildStairs(main.transform, 1, 7);
        var startingDirection = Vector3.forward;
        BuildPath(startingDirection, 5);
    }

    private Vector3 BuildPath(Vector3 start, int depth) {
        var current = start;
        for (int idx = 0; idx < depth; idx++) {
            BuildSomeDirection(platform, current);
            var newDirection = getRandomDirection(current);            
            current = newDirection;
        }
        return current;
    }

    private Vector3 getBackwardsDirectionFromPrevious(Vector3 previousDirection) {
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

    private Vector3 getRandomDirection(Vector3 previousDirection) {
        var newDirection = directions2d[Random.Range(0, directions2d.Length)];
        while (true) {
            if (newDirection != getBackwardsDirectionFromPrevious(previousDirection)) {
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

        var newPlatform = Instantiate(this.platform, newPosition, Quaternion.identity);
        platforms.Add(newPlatform);
        platform = newPlatform;
        return platform;
    }

    private Transform BuildStairs(Transform platform, int numberOfSteps, int distanceBetweenSteps) {
        for (int idx = 0; idx < numberOfSteps; idx++) {
            var x = platform.localPosition.x;
            var y = platform.localPosition.y;
            var z = platform.localPosition.z;

            var xScale = platform.localScale.x;
            var yScale = platform.localScale.y;
            var zScale = platform.localScale.z;

            var newPosition = new Vector3(x + (int)(xScale * 0.5f) + distanceBetweenSteps, y + 1, z);

            var newPlatform = Instantiate(this.platform, newPosition, Quaternion.identity);
            platforms.Add(newPlatform);
            platform = newPlatform;
        }
        return platform;
    }
}



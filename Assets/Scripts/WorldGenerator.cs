using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {

    public Transform platform;
    private List<Transform> platforms;

    private const float MAX_DISTANCE_BETWEEN_PLATFORMS = 3.0f;
    private const float MIN_DISTANCE_BETWEEN_PLATFORMS = 1.0f;

    void Start () {
        platforms = new List<Transform>();
        var main = GameObject.FindGameObjectWithTag("Main");

        var platform = BuildStairs(main.transform, 1, 7);
        BuildSomeDirection(platform, Vector3.forward);
    }

    private Vector3 getRandomDirection(Vector3 previousDirection) {
        return Vector3.zero;
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



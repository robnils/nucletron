using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {

    public Transform platform;
    private List<Transform> platforms;

	void Start () {
        platforms = new List<Transform>();
        var main = GameObject.FindGameObjectWithTag("Main");

        buildStairs(main, 10, 7);

    }

    private void buildStairs(GameObject main, int numberOfSteps, int distanceBetweenSteps) {
        var platform = main.transform;
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
    }
}



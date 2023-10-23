using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    // Degrees per second
    private float rotationSpeed = 6.0f; // 360 degrees per minute / 60 seconds

    // Update is called once per frame
    void Update()
    {
        // Rotate the object around its local Y axis at rotationSpeed degrees per second
        transform.Rotate(0,0, rotationSpeed * -Time.deltaTime);
    }
}
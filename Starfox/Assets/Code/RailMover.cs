using UnityEngine;

public class RailMover : MonoBehaviour
{
    public float speed = 20.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move the rail by speed * Time.deltaTime on the z axis
        // Time.deltaTime is the seconds since last frame
        // Vector3.forward is (0, 0, 1)
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}

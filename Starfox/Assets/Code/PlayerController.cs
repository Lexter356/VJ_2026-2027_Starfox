using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 15.0f;
    public float left_range = -8.0f;
    public float right_range = 8.0f;
    public float up_range = 5.0f;
    public float down_range = -3.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int horizontal = 0; 
        int vertical = 0;

        if(Keyboard.current.aKey.isPressed && !Keyboard.current.dKey.isPressed)
        {
            horizontal = -1;
        }
        else if(Keyboard.current.dKey.isPressed && !Keyboard.current.aKey.isPressed)
        {
            horizontal = 1;
        }
        else
        {
            horizontal = 0;
        }

        if(Keyboard.current.wKey.isPressed && !Keyboard.current.sKey.isPressed)
        {
            vertical = 1;
        }
        else if(Keyboard.current.sKey.isPressed && !Keyboard.current.wKey.isPressed)
        {
            vertical = -1;
        }
        else
        {
            vertical = 0;
        }

        // Move the player by speed * Time.deltaTime on the x and y axis
        Vector3 pos = transform.localPosition;
        pos += new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, left_range, right_range);
        pos.y = Mathf.Clamp(pos.y, down_range, up_range);
        transform.localPosition = pos;
    }
}

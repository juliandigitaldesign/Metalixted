using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickReader : MonoBehaviour
{
    public Vector2 touchDirection = Vector2.zero;
    private void Start()
    {
        //Subscribe to the action in JoyStick.cs
        joystick.onJoyStickMoved += GetJoyStickDirection;
    }

    public void GetJoyStickDirection(Vector2 touchPosition)
    {
        //Touch direction updating every time joystick is moved.
        touchDirection = touchPosition;
        //Call player move function here to move player.
    }
}

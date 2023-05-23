using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestControll : MonoBehaviour
{
    [SerializeField]
    public Transform TargetTransform;

    [SerializeField]
    private float maxSpeed = 0.5f;
    [SerializeField]
    private float acceleration = 1f;

    private Vector3 velocity = new Vector3(0, 0, 0);

    void LockCursor()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    void UnlockCursor()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    void Start() { }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LockCursor();
        }
        if (Input.GetMouseButton(0))
        {
            velocity = new Vector3(
              Input.GetAxis("Mouse X") * acceleration,
              0,
              Input.GetAxis("Mouse Y") * acceleration
            );
            if (velocity.magnitude > maxSpeed)
            {
                velocity = velocity.normalized * maxSpeed;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            velocity = new Vector3(0, 0, 0);
            UnlockCursor();
        }
        TargetTransform.position += velocity;
    }
}

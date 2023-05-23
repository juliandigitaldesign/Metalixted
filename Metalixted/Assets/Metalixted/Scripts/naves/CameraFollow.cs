using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void Start()
    {
        //target = this.transform.parent.gameObject;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = target.GetComponent<Transform>().position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualMyCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<CinemachineVirtualCamera>().LookAt = transform.parent;
        this.GetComponent<CinemachineVirtualCamera>().Follow = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

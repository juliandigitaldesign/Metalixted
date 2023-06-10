using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class JoystickShooter : MonoBehaviour
{
    public RectTransform RectTransform;
    public RectTransform knob;

    public void Awake()
    {
        RectTransform = this.GetComponent<RectTransform>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float _shootVel;

    private void Update()
    {
        transform.Translate(0, 0, _shootVel * Time.deltaTime);
        Destroy(gameObject, 6.0f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
    private void OnCollisionStay(Collision collision)
    {
        Destroy(this.gameObject);
    }
    private void OnCollisionExit(Collision collision)
    {
        Destroy(this.gameObject);
    }

}

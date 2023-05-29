using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField]
    private float _shootVel;

    private void Update()
    {
        transform.Translate(0, 0, _shootVel * Time.deltaTime);
        Destroy(gameObject, 3.0f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("IMPACTO");
        Destroy(this.gameObject);
    }

}

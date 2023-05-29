using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject BalaInicio;
    bool shooterPress = false;
    float fireRate = 0.5f;
    private float lastShot = 0.0f;

    [SerializeField]
    private GameObject _prefab;

    private void Update()
    {
        if (shooterPress)
        { 
            FireShoot_ok();    
        }
        else
        {

        }
    }

    public void OnClick_ShooterOn()    
    {
        shooterPress = true;
    }
    public void OnClick_ShooterOff()
    {
        shooterPress = false;
    }
    public void FireShoot_ok()
    {
        if (Time.time > fireRate + lastShot)
        {
            Instantiate(_prefab, BalaInicio.transform.position, BalaInicio.transform.rotation);
            lastShot = Time.time;
        }
    }
}

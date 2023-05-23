using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class PlayerRed : MonoBehaviour
{

    public MonoBehaviour[] IgnoreCodes;

    private PhotonView photonview;

    // Start is called before the first frame update
    void Start()
    {
        photonview = GetComponent<PhotonView>();
        if (!photonview.IsMine) 
        {
            foreach (var codes in IgnoreCodes)
            {
                codes.enabled = false;
                this.GetComponentInChildren<Canvas>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

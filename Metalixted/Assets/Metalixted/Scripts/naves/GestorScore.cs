using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GestorScore : MonoBehaviourPun
{

    public PhotonView scoreRedPV;
    public PhotonView scoreBluePV;

    public Text scoreRed;
    public Text scoreBlue;

    private int countRed;
    private int countBlue;

    bool goal = false;

    Rigidbody rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        scoreRedPV = GetComponent<PhotonView>();
        scoreBluePV = GetComponent<PhotonView>();

        // Comprueba el marcador Actual
        scoreBluePV.RPC("scoreRedAll", RpcTarget.All, scoreBlue.text);
        scoreRedPV.RPC("scoreBlueAll", RpcTarget.All, scoreRed.text);
    }

    // Update is called once per frame
    void Update()
    {
        if (goal == true)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            goal = false;
        }
        else { }
    }

    void OnCollisionEnter(Collision collision)    
    {
        if (photonView.IsMine) 
        {
            if (collision.gameObject.name == "red_frame")
            {
                foreach (ContactPoint contact in collision.contacts)
                {
                    countRed += 1; 
                    scoreBlue.text = countRed.ToString();
                }

                scoreBluePV.RPC("scoreRedAll", RpcTarget.All, scoreBlue.text);

                this.transform.position = new Vector3(0, 1, 0);
                goal = true;
            }

            if (collision.gameObject.name == "blue_frame")
            {
                foreach (ContactPoint contact in collision.contacts)
                {
                    countBlue += 1;
                    scoreRed.text = countBlue.ToString();
                }

                scoreRedPV.RPC("scoreBlueAll", RpcTarget.All, scoreRed.text);

                this.transform.position = new Vector3(0, 2, 0);
                goal = true;
            }
        }        
    }

    [PunRPC]
    public void scoreRedAll(string tg)
    {
        scoreBlue.text = tg;
    }

    [PunRPC]
    public void scoreBlueAll(string tg)
    {
        scoreRed.text = tg;
    }

}

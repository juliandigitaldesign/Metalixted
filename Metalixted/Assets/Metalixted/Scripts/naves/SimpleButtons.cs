using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class SimpleButtons : MonoBehaviourPun
{
    
    public void MainMenu()
    {
        PhotonNetwork.LoadLevel(1);
    }
}

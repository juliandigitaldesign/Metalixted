using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NickNamePlayer : MonoBehaviour
{
    public TextMeshProUGUI _nickNamePlayerShow;


    // Start is called before the first frame update
    void Awake()
    {
        //_nickNamePlayerShow.text = MasterManager.GameSettings.NickName;
        string nickName = GetComponent<PhotonView>().Owner.NickName;
        _nickNamePlayerShow.text = nickName;
    }
}

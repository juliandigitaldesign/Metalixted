using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Unity.VisualScripting;

public class NickNamePut : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameSettings _gameSettings;

    public GameObject _canvasRooms;
    public TextMeshProUGUI _nickNamePut;
    public string _nickNameTake;
    public GameObject _testConnect;

    public void Awake()
    {
        _testConnect.SetActive(false);
    }

    public void OnClick_nickNamePut() 
    {
        _nickNameTake = _nickNamePut.text;
        _gameSettings._nickName = _nickNameTake;
        _testConnect.SetActive(true);
        _canvasRooms.SetActive(true);
    }

}

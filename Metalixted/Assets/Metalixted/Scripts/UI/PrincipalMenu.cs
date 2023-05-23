using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrincipalMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _principalMenu;
    [SerializeField]
    private GameObject _confirmLeaveRoom;

    public void OnClick_ButtonMenu()
    {
        if (!_principalMenu.activeSelf)
        {
            _principalMenu.SetActive(true);
        }
        else
        {
            _principalMenu.SetActive(false);
        }
    }

    public void OnClick_Continue()
    {
        _principalMenu.SetActive(false);
        _confirmLeaveRoom.SetActive(false);
    }

    public void OnClick_LeaveRoom()
    {
        _confirmLeaveRoom.SetActive(true);
    }

    public void OnClick_ComfirmLeaveRoom()
    {
        PhotonNetwork.LoadLevel(1);
    }
}

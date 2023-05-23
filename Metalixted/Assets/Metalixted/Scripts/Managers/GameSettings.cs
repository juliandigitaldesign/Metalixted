using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    private string _gameVersion = "1.0.2";
    public string GameVersion { get { return _gameVersion; } }
    [SerializeField]
    public string _nickName;

    public string NickName 
    {        
        get 
        {
            return _nickName;
        }
    }

}

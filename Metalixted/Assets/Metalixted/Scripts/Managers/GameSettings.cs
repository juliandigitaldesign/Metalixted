using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject
{

    [SerializeField]
    private string _gameVersion = "1.0.2";
    public string GameVersion { get { return _gameVersion; } }
    [SerializeField]
    private string _nickName = "Powaplus";
    
    public string NickName 
    {
        get 
        {
            int value = Random.Range(0, 9999);
            return _nickName + value.ToString();
        }
    }

}

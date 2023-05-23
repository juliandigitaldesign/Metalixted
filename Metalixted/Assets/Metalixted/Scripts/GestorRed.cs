using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Importante
using Photon.Pun;
using Photon.Realtime;

public class GestorRed : MonoBehaviourPunCallbacks
{

    public static GestorRed instacia;
    public GameObject mesh;

    public Text textPlayers;

    Color[] colors = new Color[2];
    private void Awake()
    {
        instacia = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        colors[0] = Color.blue;
        colors[1] = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            textPlayers = textPlayers.GetComponent<Text>();
            textPlayers.text = "Players Online:  " + PhotonNetwork.CurrentRoom.PlayerCount;
        }
        catch { }
        
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("ESTAS ONLINE");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom("NewRoom", new RoomOptions { MaxPlayers = 20 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-3.0f, 3.0f), 3f, Random.Range(-3.0f, -2.0f)), Quaternion.identity);
        //mesh = GameObject.Find("Armature_Mesh");
        //Renderer rend = mesh.GetComponent<Renderer>();
        //rend.material.color = colors[Random.Range(0, colors.Length)];
    }
}

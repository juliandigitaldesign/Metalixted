using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Importante
using Photon.Pun;
using Photon.Realtime;

public class GestorRedNaves : MonoBehaviourPunCallbacks
{

    public static GestorRedNaves instacia;
    //public GameObject colorMaterial;

    public Text textPlayers;

    //Color[] colors = new Color[2];
    private void Awake()
    {
        instacia = this;
        DontDestroyOnLoad(gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        //colors[0] = Color.blue;
        //colors[1] = Color.red;

        // desired behaviour, the result of GetComponentsInChildren can be cast directly
        GestorRedNaves[] me = (GestorRedNaves[])GetComponentsInChildrenDesired(typeof(GestorRedNaves));
        Debug.Log(me[0].name);

        // current behaviour, casting the result of GetComponentsInChildren causes an exception
        try {
            me = (GestorRedNaves[])GetComponentsInChildrenCurrent(typeof(GestorRedNaves));
            Debug.Log(me[0].name);
        } catch { }
        
    }

    // mockup of the current implementation of GetComponentsInChildren (obviously the real function does more than this one)
    // note that casting the result to a derived type causes an exception
    Component[] GetComponentsInChildrenCurrent(System.Type t)
    {
        Component[] returnArray = new Component[1];
        returnArray[0] = GetComponent(t);
        return returnArray;
    }

    // mockup of the desired implementation of GetComponentsInChildren (obviously the real function does more than this one)
    // note that casting the result to the derived type works as expected
    Component[] GetComponentsInChildrenDesired(System.Type t)
    {
        Component[] returnArray = (Component[])System.Array.CreateInstance(t, 1);
        returnArray[0] = GetComponent(t);
        return returnArray;
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
        PhotonNetwork.Instantiate("Ship", new Vector3(Random.Range(-3.0f, 3.0f), 3, Random.Range(-3.0f, -2.0f)), Quaternion.identity);

        /*
         colorMaterial = GameObject.Find("color");

        Component[] renderers = colorMaterial.GetComponentsInChildren(typeof(Renderer));
        foreach (Renderer childRenderer in renderers)
        {
            childRenderer.material.color = Color.blue;
        }

        GameObject vel1 = GameObject.Find("3velocidad1");
        GameObject vel2 = GameObject.Find("6velocidad2");

        vel1.SetActive(false);
        vel2.SetActive(false);
        */
    }
}

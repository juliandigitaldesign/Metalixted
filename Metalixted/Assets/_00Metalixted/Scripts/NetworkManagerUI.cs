using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject buttons;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private Button shutdownBtn;

    private void Awake()
    {
        hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost(); Onclick_Actions();
        });
        serverBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer(); Onclick_Actions();
        });        
        clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient(); Onclick_Actions();
        });
        shutdownBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown(); Onclick_ShutDown();
        });
    }

    void Onclick_Actions() 
    {
        shutdownBtn.GetComponent<Button>().gameObject.SetActive(true);
        buttons.SetActive(false);
    }

    void Onclick_ShutDown()
    {
        shutdownBtn.GetComponent<Button>().gameObject.SetActive(false);
        buttons.SetActive(true);
    }

}

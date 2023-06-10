using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using TMPro;
using Unity.Networking.Transport.Relay;
using UnityEngine.UI;

public class TestRelay : MonoBehaviour
{
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject cameraIntro;
    [SerializeField] private Button shutdownBtn;
    [SerializeField] private TMP_Text keyCode;
    [SerializeField] private TMP_InputField inputKeyCode;

    private void Awake()
    {
        shutdownBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown(); Onclick_ShutDown();
        });
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

    }

    public async void CreateRelay()
    {
        try {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            keyCode.text = joinCode.ToString();

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();

            Onclick_Actions();

        } catch(RelayServiceException e) 
        {
            Debug.Log(e);
        }
    }

    public async void JoinRelay(string joinCode) 
    {
        try
        {
            joinCode = inputKeyCode.text;

            JoinAllocation joinallocation =  await RelayService.Instance.JoinAllocationAsync(joinCode);


            RelayServerData relayServerData = new RelayServerData(joinallocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();

            Onclick_Actions();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    void Onclick_Actions()
    {
        shutdownBtn.GetComponent<Button>().gameObject.SetActive(true);
        cameraIntro.SetActive(false);
        buttons.SetActive(false);
    }

    void Onclick_ShutDown()
    {
        shutdownBtn.GetComponent<Button>().gameObject.SetActive(false);
        cameraIntro.SetActive(true);
        buttons.SetActive(true);
    }

}

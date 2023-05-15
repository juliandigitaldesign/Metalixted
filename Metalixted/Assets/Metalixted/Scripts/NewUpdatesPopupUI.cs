using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

namespace UpgradeSystem
{

    struct GameData
    {
        public string Description;
        public string Version;
        public string Url;
    }

    public class NewUpdatesPopupUI : MonoBehaviour
    {

        [Header("## UI References :")]
        [SerializeField] GameObject uiCanvas;
        [SerializeField] GameObject uiPlayButton;
        [SerializeField] Button uiUpdateButton;
        [SerializeField] TextMeshProUGUI uiDescriptionText;
        [SerializeField] TextMeshProUGUI uiLoadingText;

        [Space(20f)]
        [Header("## Settings :")]
        [SerializeField][TextArea(1, 5)] string jsonDataURL;

        static bool isAlreadyCheckedForUpdates = false;

        GameData latestGameData;

        [System.Obsolete]
        void Start()
        {
            if (!isAlreadyCheckedForUpdates)
            {
                StopAllCoroutines();
                StartCoroutine(CheckForUpdates());
            }
        }

        [System.Obsolete]
        IEnumerator CheckForUpdates()
        {
            UnityWebRequest request = UnityWebRequest.Get(jsonDataURL);
            request.chunkedTransfer = false;
            request.disposeDownloadHandlerOnDispose = true;
            request.timeout = 60;

            yield return request.Send();

                          

            if (request.isDone)
            {
                isAlreadyCheckedForUpdates = true;

                if (!request.isNetworkError)
                {
                    latestGameData = JsonUtility.FromJson<GameData>(request.downloadHandler.text);
                    if (!string.IsNullOrEmpty(latestGameData.Version) && !Application.version.Equals(latestGameData.Version))
                    {
                        // new update is available
                        uiLoadingText.text = "Descarga la actualización para poder continuar";
                        uiDescriptionText.text = latestGameData.Description;
                        ShowPopup();
                    } else if (Application.version.Equals(latestGameData.Version))
                    {
                        uiLoadingText.text = "Juego actualizado en su ultima versión.";
                        uiPlayButton.SetActive(true);
                    }
                }
            }

            request.Dispose();
        }

        void ShowPopup()
        {
            // Add buttons click listeners :
            uiUpdateButton.onClick.AddListener(() => {
                Application.OpenURL(latestGameData.Url);
                HidePopup();
            });

            uiCanvas.SetActive(true);
        }

        void HidePopup()
        {
            uiCanvas.SetActive(false);

            // Remove buttons click listeners :
            uiUpdateButton.onClick.RemoveAllListeners();
        }


        void OnDestroy()
        {
            StopAllCoroutines();
        }

    }

}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.FullSerializer;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(menuName = "Singletons/MasterManager")]
public class MasterManager : ScriptableObjectSingleton<MasterManager>
{
    [SerializeField]
    public GameSettings _gameSettings;

    public static GameSettings GameSettings { get { return Instance._gameSettings; } }

    [SerializeField]
    private List<NetworkedPrefab> _networkedPrefabs = new List<NetworkedPrefab>();

    public static GameObject NetworKInstantiate(GameObject obj, Vector3 position, Quaternion rotation) 
    {
        foreach (NetworkedPrefab networkedPrefab in Instance._networkedPrefabs) 
        {
            if (networkedPrefab.Prefab == obj)
            {
                if (networkedPrefab.Path != string.Empty)
                {
                    GameObject result = PhotonNetwork.Instantiate(networkedPrefab.Path, position, rotation);
                    return result;
                }
                else 
                {
                    Debug.LogError("Paht is empty for gameobject name " + networkedPrefab.Prefab);
                    return null;
                }
            }
        }

        return null;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void PopulateNetworkedPrefabs() 
    {
#if UNITY_EDITOR
        Instance._networkedPrefabs.Clear();

        GameObject[] results = Resources.LoadAll<GameObject>("");
        for (int i = 0; i < results.Length; i++)
        {
            if (results[i].GetComponent<PhotonView>() != null)
            {
                string path = AssetDatabase.GetAssetPath(results[i]);
                Instance._networkedPrefabs.Add(new NetworkedPrefab(results[i], path));
                Debug.Log(path);
            }
        }
#endif
    }
}

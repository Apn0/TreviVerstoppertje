using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkManager_Custom : NetworkManager {

    public void StartupHost() {
        if (!NetworkClient.active && !NetworkServer.active) {
            SetPort(); NetworkManager.singleton.StartHost();
        }
    }

    public void JoinGame() {
        if (!NetworkClient.active && !NetworkServer.active) {
            SetIPAddress();
            SetPort();
            NetworkManager.singleton.StartClient();
        }

    }
    void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }

    void SetIPAddress()
    {
        string ipAddress = GameObject.Find("IPInputField").transform.FindChild("Text").GetComponent<Text>().text;
        singleton.networkAddress = ipAddress;
    }

    void OnLevelWasLoaded(int level)
    {
        if(level == 0 )
        {
            StartCoroutine(SetupMenuSceneButtons());            
        }
        else
        {
            SetupOtherSceneButtons();
        }
    }

    IEnumerator SetupMenuSceneButtons()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject.Find("LanHostButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("LanHostButton").GetComponent<Button>().onClick.AddListener(StartupHost);

        GameObject.Find("ConnectButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ConnectButton").GetComponent<Button>().onClick.AddListener(JoinGame);
        Debug.Log("level0buttons");
    }

    void SetupOtherSceneButtons()
    {
        GameObject.Find("DisconnectButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("DisconnectButton").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);
    }
}

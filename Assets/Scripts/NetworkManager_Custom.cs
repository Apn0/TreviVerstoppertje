using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkManager_Custom : NetworkManager {

    public string playerName;
    public string ipAdress;

    public void StartupHost() {
        if (!NetworkClient.active && !NetworkServer.active) {
            SetPort();
            SetName();
            NetworkManager.singleton.StartHost();
        }
    }

    public void JoinGame() {
        if (!NetworkClient.active && !NetworkServer.active) {
            SetIPAddress();
            SetName();
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
        NetworkManager.singleton.networkAddress = ipAddress;
    }
    void SetName()
    {
        string playerName = GameObject.Find("NameInputField").transform.FindChild("Text").GetComponent<Text>().text;
        Debug.Log(playerName);
        CarryMeOver._playerName = playerName;
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

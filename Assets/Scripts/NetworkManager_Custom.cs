using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkManager_Custom : NetworkManager {

    public string playerName;
    private Button lanHostButton;
    private Button connectButton;
    private Button disconnectButton;
    private Text ipInputFieldText;
    private Text nameInputFieldText;
    private Text nameInputFieldPlaceholder;

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
        if (ipInputFieldText == null)
        {
            GameObject ipInputFieldObj = GameObject.Find("IPInputField");
            if (ipInputFieldObj != null)
            {
                ipInputFieldText = ipInputFieldObj.transform.Find("Text").GetComponent<Text>();
            }
        }

        if (ipInputFieldText != null)
        {
            NetworkManager.singleton.networkAddress = ipInputFieldText.text;
        }
    }

    void SetName()
    {
        if (nameInputFieldText == null)
        {
            GameObject nameInputFieldObj = GameObject.Find("NameInputField");
            if (nameInputFieldObj != null)
            {
                nameInputFieldText = nameInputFieldObj.transform.Find("Text").GetComponent<Text>();
            }
        }

        if (nameInputFieldText != null)
        {
            playerName = nameInputFieldText.text;
        }

        if (playerName == "") {
            playerName = PlayerPrefs.GetString("username");
            Debug.Log(playerName);
        }

        PlayerPrefs.SetString("username", playerName);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 0)
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

        GameObject lanHostButtonObj = GameObject.Find("LanHostButton");
        if (lanHostButtonObj != null)
        {
            lanHostButton = lanHostButtonObj.GetComponent<Button>();
            if (lanHostButton != null)
            {
                lanHostButton.onClick.RemoveAllListeners();
                lanHostButton.onClick.AddListener(StartupHost);
            }
        }

        GameObject connectButtonObj = GameObject.Find("ConnectButton");
        if (connectButtonObj != null)
        {
            connectButton = connectButtonObj.GetComponent<Button>();
            if (connectButton != null)
            {
                connectButton.onClick.RemoveAllListeners();
                connectButton.onClick.AddListener(JoinGame);
            }
        }

        GameObject ipInputFieldObj = GameObject.Find("IPInputField");
        if (ipInputFieldObj != null)
        {
            ipInputFieldText = ipInputFieldObj.transform.Find("Text").GetComponent<Text>();
        }

        GameObject nameInputFieldObj = GameObject.Find("NameInputField");
        if (nameInputFieldObj != null)
        {
            nameInputFieldText = nameInputFieldObj.transform.Find("Text").GetComponent<Text>();
            nameInputFieldPlaceholder = nameInputFieldObj.transform.Find("Placeholder").GetComponent<Text>();
        }

        playerName = PlayerPrefs.GetString("username");
        if (playerName != null && nameInputFieldPlaceholder != null)
        {
            nameInputFieldPlaceholder.text = playerName;
        }
    }

    void SetupOtherSceneButtons()
    {
        if (disconnectButton == null)
        {
            GameObject disconnectButtonObj = GameObject.Find("DisconnectButton");
            if (disconnectButtonObj != null)
            {
                disconnectButton = disconnectButtonObj.GetComponent<Button>();
            }
        }

        if (disconnectButton != null)
        {
            disconnectButton.onClick.RemoveAllListeners();
            disconnectButton.onClick.AddListener(NetworkManager.singleton.StopHost);
        }
    }
}

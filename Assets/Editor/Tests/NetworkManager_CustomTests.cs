using System;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[TestFixture]
public class NetworkManager_CustomTests
{
    private GameObject networkManagerObj;
    private NetworkManager_Custom networkManagerCustom;

    // Dummy UI Objects
    private GameObject ipInputFieldObj;
    private Text ipInputFieldText;

    private GameObject nameInputFieldObj;
    private Text nameInputFieldText;
    private Text nameInputFieldPlaceholder;

    [SetUp]
    public void SetUp()
    {
        // Clear PlayerPrefs before each test
        PlayerPrefs.DeleteAll();

        // Create the NetworkManager object
        networkManagerObj = new GameObject("NetworkManager");
        networkManagerCustom = networkManagerObj.AddComponent<NetworkManager_Custom>();

        // NetworkManager.singleton is normally set in Awake, but to ensure it's set:
        if (NetworkManager.singleton == null)
        {
            // Reflection might be needed if singleton setter is internal, but typically Awake sets it
            networkManagerObj.SendMessage("Awake");
        }

        // Setup dummy IPInputField
        ipInputFieldObj = new GameObject("IPInputField");
        GameObject ipTextObj = new GameObject("Text");
        ipTextObj.transform.SetParent(ipInputFieldObj.transform);
        ipInputFieldText = ipTextObj.AddComponent<Text>();

        // Setup dummy NameInputField
        nameInputFieldObj = new GameObject("NameInputField");

        GameObject nameTextObj = new GameObject("Text");
        nameTextObj.transform.SetParent(nameInputFieldObj.transform);
        nameInputFieldText = nameTextObj.AddComponent<Text>();

        GameObject placeholderObj = new GameObject("Placeholder");
        placeholderObj.transform.SetParent(nameInputFieldObj.transform);
        nameInputFieldPlaceholder = placeholderObj.AddComponent<Text>();
    }

    [TearDown]
    public void TearDown()
    {
        if (networkManagerObj != null)
        {
            GameObject.DestroyImmediate(networkManagerObj);
        }
        if (ipInputFieldObj != null)
        {
            GameObject.DestroyImmediate(ipInputFieldObj);
        }
        if (nameInputFieldObj != null)
        {
            GameObject.DestroyImmediate(nameInputFieldObj);
        }

        // Clean up any remaining objects
        var existingManager = GameObject.Find("NetworkManager");
        if (existingManager != null) GameObject.DestroyImmediate(existingManager);

        PlayerPrefs.DeleteAll();
    }

    [Test]
    public void StartupHost_SetsPortAndName()
    {
        // Arrange
        nameInputFieldText.text = "TestHostPlayer";

        // Act
        // Reflection to call StartupHost without triggering full Unity Networking (which might fail in batch mode)
        // Actually, since StartHost might throw in headless without proper setup, we'll try catching or we can just call the private methods if StartHost throws
        try
        {
            networkManagerCustom.StartupHost();
        }
        catch (Exception)
        {
            // Expected if NetworkServer fails to start in EditMode without proper initialization
            // We just care that the side effects (SetPort, SetName) happened before the failure.
        }

        // Assert
        Assert.AreEqual(7777, networkManagerCustom.networkPort, "Network port should be set to 7777");
        Assert.AreEqual("TestHostPlayer", networkManagerCustom.playerName, "Player name should be read from the UI Text");
        Assert.AreEqual("TestHostPlayer", PlayerPrefs.GetString("username"), "Player name should be saved to PlayerPrefs");
    }

    [Test]
    public void JoinGame_SetsIPAddressPortAndName()
    {
        // Arrange
        ipInputFieldText.text = "192.168.1.100";
        nameInputFieldText.text = "TestClientPlayer";

        // Act
        try
        {
            networkManagerCustom.JoinGame();
        }
        catch (Exception)
        {
            // Expected if NetworkClient fails to start in EditMode
        }

        // Assert
        Assert.AreEqual("192.168.1.100", networkManagerCustom.networkAddress, "Network address should be set to the IP text");
        Assert.AreEqual(7777, networkManagerCustom.networkPort, "Network port should be set to 7777");
        Assert.AreEqual("TestClientPlayer", networkManagerCustom.playerName, "Player name should be read from the UI Text");
        Assert.AreEqual("TestClientPlayer", PlayerPrefs.GetString("username"), "Player name should be saved to PlayerPrefs");
    }

    [Test]
    public void SetName_FallbackToPlayerPrefs_WhenInputIsEmpty()
    {
        // Arrange
        PlayerPrefs.SetString("username", "SavedPlayerName");
        nameInputFieldText.text = ""; // Empty input

        // Use reflection to call private method SetName
        MethodInfo setNameMethod = typeof(NetworkManager_Custom).GetMethod("SetName", BindingFlags.NonPublic | BindingFlags.Instance);

        // Act
        setNameMethod.Invoke(networkManagerCustom, null);

        // Assert
        Assert.AreEqual("SavedPlayerName", networkManagerCustom.playerName, "Player name should fall back to PlayerPrefs when input is empty");
    }

    [Test]
    public void SetIPAddress_FindsUIAndSetsAddress()
    {
        // Arrange
        ipInputFieldText.text = "10.0.0.5";
        MethodInfo setIPAddressMethod = typeof(NetworkManager_Custom).GetMethod("SetIPAddress", BindingFlags.NonPublic | BindingFlags.Instance);

        // Act
        setIPAddressMethod.Invoke(networkManagerCustom, null);

        // Assert
        Assert.AreEqual("10.0.0.5", networkManagerCustom.networkAddress, "Network address should be set from the found IPInputField");
    }
}

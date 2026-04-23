using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public class GameManagerTests
{
    private GameObject playerObject;
    private Player playerComponent;

    [SetUp]
    public void SetUp()
    {
        // Clear static state before each test
        ClearPlayersDictionary();

        playerObject = new GameObject("TestPlayerObject");
        playerComponent = playerObject.AddComponent<Player>();
    }

    [TearDown]
    public void TearDown()
    {
        if (playerObject != null)
        {
            Object.DestroyImmediate(playerObject);
        }

        // Clear static state after each test
        ClearPlayersDictionary();
    }

    private void ClearPlayersDictionary()
    {
        var playersField = typeof(GameManager).GetField("players", BindingFlags.NonPublic | BindingFlags.Static);
        if (playersField != null)
        {
            var playersDict = (Dictionary<string, Player>)playersField.GetValue(null);
            playersDict.Clear();
        }
    }

    [Test]
    public void RegisterPlayer_AddsPlayerToDictionaryAndSetsName()
    {
        string netID = "12345";
        string expectedPlayerID = "Player " + netID;

        GameManager.RegisterPlayer(netID, playerComponent);

        // Verify player is in dictionary
        Player retrievedPlayer = GameManager.GetPlayer(expectedPlayerID);
        Assert.IsNotNull(retrievedPlayer, "Player should be retrievable from GameManager.");
        Assert.AreEqual(playerComponent, retrievedPlayer, "Retrieved player should be the same as registered player.");

        // Verify player name is set correctly
        Assert.AreEqual(expectedPlayerID, playerObject.name, "Player object name should be set to PlayerID.");

        // Verify dictionary size
        var playersDict = GameManager.GetPlayers();
        Assert.AreEqual(1, playersDict.Count, "GameManager players dictionary should contain exactly 1 item.");
    }

    [Test]
    public void UnRegisterPlayer_RemovesPlayerFromDictionary()
    {
        string netID = "12345";
        string expectedPlayerID = "Player " + netID;

        // Register first
        GameManager.RegisterPlayer(netID, playerComponent);
        Assert.AreEqual(1, GameManager.GetPlayers().Count, "Should have 1 player after registration.");

        // Unregister
        GameManager.UnRegisterPlayer(expectedPlayerID);

        // Verify dictionary size
        Assert.AreEqual(0, GameManager.GetPlayers().Count, "Should have 0 players after unregistration.");
    }

    [Test]
    public void GetPlayer_ReturnsCorrectPlayer()
    {
        string netID = "12345";
        string expectedPlayerID = "Player " + netID;

        // Register
        GameManager.RegisterPlayer(netID, playerComponent);

        // Act
        Player retrievedPlayer = GameManager.GetPlayer(expectedPlayerID);

        // Assert
        Assert.AreEqual(playerComponent, retrievedPlayer, "GetPlayer should return the correct player instance.");
    }

    [Test]
    public void GetPlayer_ThrowsKeyNotFoundException_WhenPlayerDoesNotExist()
    {
        string invalidPlayerID = "Player 99999";

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => GameManager.GetPlayer(invalidPlayerID), "GetPlayer should throw KeyNotFoundException for an unregistered player ID.");
    }
}

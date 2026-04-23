using NUnit.Framework;
using UnityEngine;
using System.Reflection;
using System.Collections;

public class PlayerBoundaryTest
{
    [Test]
    public void TestRpcTakeDamageBoundary()
    {
        // 1. Arrange
        GameObject go = new GameObject("TestPlayer");
        Player player = go.AddComponent<Player>();

        // Setup dependencies
        FieldInfo disableOnDeathField = typeof(Player).GetField("disableOnDeath", BindingFlags.NonPublic | BindingFlags.Instance);
        disableOnDeathField.SetValue(player, new Behaviour[0]);

        FieldInfo playerGraphicsField = typeof(Player).GetField("playerGraphics", BindingFlags.NonPublic | BindingFlags.Instance);
        GameObject graphicsGo = new GameObject("Graphics");
        playerGraphicsField.SetValue(player, graphicsGo);

        // Ensure GameManager exists for Die()
        GameObject gmGo = new GameObject("GameManager");
        GameManager gm = gmGo.AddComponent<GameManager>();
        gm.matchSettings = new MatchSettings();

        // Setup network manager
        GameObject nmGo = new GameObject("NetworkManager");
        UnityEngine.Networking.NetworkManager nm = nmGo.AddComponent<UnityEngine.Networking.NetworkManager>();

        // maxHealth defaults to 100
        player.Setup();

        // 2. Act & Assert

        // Take 99 damage -> 1 health remaining, should not be dead
        player.RpcTakeDamage(99, "Attacker1");
        Assert.IsFalse(player.isDead, "Player should not be dead with 1 health.");

        // Reset and test exactly 0 boundary
        player.SetDefaults();
        player.RpcTakeDamage(100, "Attacker2");
        Assert.IsTrue(player.isDead, "Player should be dead when health is exactly 0.");

        // Reset and test negative health boundary
        player.SetDefaults();
        player.RpcTakeDamage(101, "Attacker3");
        Assert.IsTrue(player.isDead, "Player should be dead when health drops below 0.");

        // Cleanup
        GameObject.DestroyImmediate(graphicsGo);
        GameObject.DestroyImmediate(go);
        GameObject.DestroyImmediate(gmGo);
        GameObject.DestroyImmediate(nmGo);
    }
}

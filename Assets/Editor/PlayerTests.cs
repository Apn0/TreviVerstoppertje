using NUnit.Framework;
using UnityEngine;
using System.Reflection;

namespace Tests
{
    public class PlayerTests
    {
        private GameObject playerObj;
        private Player player;
        private GameObject graphicsObj;
        private GameObject gmObj;

        [SetUp]
        public void SetUp()
        {
            playerObj = new GameObject();
            player = playerObj.AddComponent<Player>();

            // Set required serialized fields via reflection
            typeof(Player).GetField("maxHealth", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(player, 100);
            typeof(Player).GetField("disableOnDeath", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(player, new Behaviour[0]);

            graphicsObj = new GameObject();
            typeof(Player).GetField("playerGraphics", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(player, graphicsObj);

            // Set up a mock GameManager to prevent NullReferenceException in Die -> Respawn
            gmObj = new GameObject();
            GameManager gm = gmObj.AddComponent<GameManager>();
            gm.matchSettings = new MatchSettings();
            gm.matchSettings.respawnTime = 3f;
            GameManager.instance = gm;
        }

        [TearDown]
        public void TearDown()
        {
            if (playerObj != null) Object.DestroyImmediate(playerObj);
            if (graphicsObj != null) Object.DestroyImmediate(graphicsObj);
            if (gmObj != null) Object.DestroyImmediate(gmObj);
            GameManager.instance = null;
        }

        [Test]
        public void RpcTakeDamage_ReducesHealth()
        {
            // Arrange
            player.Setup();

            // Act
            player.RpcTakeDamage(10, "TestShooter");

            // Assert
            int currentHealth = (int)typeof(Player).GetField("currentHealth", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(player);
            Assert.AreEqual(90, currentHealth);
            Assert.IsFalse(player.isDead);
        }

        [Test]
        public void RpcTakeDamage_WhenDead_DoesNothing()
        {
            // Arrange
            player.Setup();

            // Force player dead state (Property uses protected set, so reflection is needed)
            PropertyInfo isDeadProp = typeof(Player).GetProperty("isDead", BindingFlags.Public | BindingFlags.Instance);
            if (isDeadProp != null && isDeadProp.CanWrite)
            {
                isDeadProp.SetValue(player, true, null);
            }
            else
            {
                // Find backing field
                FieldInfo isDeadField = typeof(Player).GetField("_isDead", BindingFlags.NonPublic | BindingFlags.Instance);
                if (isDeadField != null)
                {
                    isDeadField.SetValue(player, true);
                }
            }

            // Act
            player.RpcTakeDamage(10, "TestShooter");

            // Assert
            int currentHealth = (int)typeof(Player).GetField("currentHealth", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(player);
            Assert.AreEqual(100, currentHealth);
            Assert.IsTrue(player.isDead);
        }

        [Test]
        public void RpcTakeDamage_CausesDeathWhenHealthReachesZero()
        {
            // Arrange
            player.Setup();

            // Act
            player.RpcTakeDamage(100, "TestShooter");

            // Assert
            int currentHealth = (int)typeof(Player).GetField("currentHealth", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(player);
            Assert.AreEqual(0, currentHealth);
            Assert.IsTrue(player.isDead);
        }

        [Test]
        public void RpcTakeDamage_NegativeDamage_IncreasesHealth()
        {
            // Arrange
            player.Setup();

            // Act
            player.RpcTakeDamage(-20, "TestShooter");

            // Assert
            int currentHealth = (int)typeof(Player).GetField("currentHealth", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(player);
            Assert.AreEqual(120, currentHealth);
            Assert.IsFalse(player.isDead);
        }
    }
}

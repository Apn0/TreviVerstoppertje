using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class HUDManagerTests {

    [Test]
    public void UpdatePlayerHealth_InitialCall_FindsSliderAndSetsValue() {
        // Arrange
        GameObject playerObj = new GameObject("Player");
        HUDManager hudManager = playerObj.AddComponent<HUDManager>();
        PlayerSetup playerSetup = playerObj.AddComponent<PlayerSetup>();

        GameObject uiInstance = new GameObject("PlayerUI");
        playerSetup.playerUIInstance = uiInstance;

        GameObject sliderObj = new GameObject("HPBar");
        sliderObj.transform.SetParent(uiInstance.transform);
        Slider slider = sliderObj.AddComponent<Slider>();
        slider.value = 100;

        // Act
        int expectedHealth = 75;
        hudManager.UpdatePlayerHealth(expectedHealth);

        // Assert
        Assert.AreEqual(expectedHealth, slider.value, "Slider value should be updated to the player's new health on the initial call.");

        // Cleanup
        GameObject.DestroyImmediate(playerObj);
        GameObject.DestroyImmediate(uiInstance);
    }

    [Test]
    public void UpdatePlayerHealth_SubsequentCall_UpdatesCachedSliderValue() {
        // Arrange
        GameObject playerObj = new GameObject("Player");
        HUDManager hudManager = playerObj.AddComponent<HUDManager>();
        PlayerSetup playerSetup = playerObj.AddComponent<PlayerSetup>();

        GameObject uiInstance = new GameObject("PlayerUI");
        playerSetup.playerUIInstance = uiInstance;

        GameObject sliderObj = new GameObject("HPBar");
        sliderObj.transform.SetParent(uiInstance.transform);
        Slider slider = sliderObj.AddComponent<Slider>();

        // Initial call to cache the slider
        hudManager.UpdatePlayerHealth(100);

        // Disconnect the original reference to ensure it uses the cached one
        // (If it searched again, it would fail since we removed it)
        sliderObj.transform.SetParent(null);

        // Act
        int expectedHealth = 50;
        hudManager.UpdatePlayerHealth(expectedHealth);

        // Assert
        Assert.AreEqual(expectedHealth, slider.value, "Slider value should be updated using the cached reference.");

        // Cleanup
        GameObject.DestroyImmediate(playerObj);
        GameObject.DestroyImmediate(uiInstance);
        GameObject.DestroyImmediate(sliderObj);
    }
}

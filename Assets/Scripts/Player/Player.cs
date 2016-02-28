using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour {

	[SyncVar]
	private bool _isDead = false;
	public bool isDead
	{
		get { return _isDead; }
		protected set { _isDead = value; }
	}

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

	[SerializeField]
	private Behaviour[] disableOnDeath;
	private bool[] wasEnabled;

    [SerializeField]
    GameObject playerGraphics;

    public string playerName;

    private HUDManager uimanager;
    void Start()
    {
        if(isLocalPlayer)
        uimanager = GetComponent<HUDManager>();
    }

    public void Setup ()
    {
        wasEnabled = new bool[disableOnDeath.Length];
		for (int i = 0; i < wasEnabled.Length; i++)
		{
			wasEnabled[i] = disableOnDeath[i].enabled;
		}

        SetDefaults();
    }

	void Update ()
	{
		if (!isLocalPlayer)
			return;

		if (Input.GetKeyDown(KeyCode.K))
		{
			RpcTakeDamage(99999, "The game");
		}
	}

	[ClientRpc]
    public void RpcTakeDamage (int _amount, string shooterName)
    {
		if (isDead)
			return;

        currentHealth -= _amount;

        Debug.Log(transform.name + " now has " + currentHealth + " health.");
        if(isLocalPlayer && uimanager != null)
        {
            uimanager.UpdatePlayerHealth(currentHealth);
        }

		if (currentHealth <= 0)
		{
			Die(shooterName);
		}
    }

	private void Die(string killerName)
	{
		isDead = true;

		for (int i = 0; i < disableOnDeath.Length; i++)
		{
			disableOnDeath[i].enabled = false;
		}

		Collider _col = GetComponent<Collider>();
		if (_col != null)
			_col.enabled = false;

        playerGraphics.SetActive(false);

        Debug.Log(killerName + " killed " + playerName);

		StartCoroutine(Respawn());
	}

	private IEnumerator Respawn ()
	{
		yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

		
		Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
		transform.position = _spawnPoint.position;
		transform.rotation = _spawnPoint.rotation;
        SetDefaults();

        Debug.Log(transform.name + " respawned.");
	}

    public void SetDefaults ()
    {
		isDead = false;

        currentHealth = maxHealth;

		for (int i = 0; i < disableOnDeath.Length; i++)
		{
			disableOnDeath[i].enabled = wasEnabled[i];
		}

		Collider _col = GetComponent<Collider>();
		if (_col != null)
			_col.enabled = true;

        playerGraphics.SetActive(true);
        if (isLocalPlayer && uimanager != null)
        {
            uimanager.UpdatePlayerHealth(currentHealth);
        }
    }

}

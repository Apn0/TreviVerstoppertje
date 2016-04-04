using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    public PlayerWeapon weapon;

    public GameObject bulletHolePrefab;

    private AudioSource gunSource;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private float t = 0f;


    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("PlayerShoot: No camera referenced!");
            this.enabled = false;
        }

        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
        {
            if (child.name=="CQAssaultRifle")
            {
                gunSource = child.GetComponent<AudioSource>();
            }
        }
    }

    void Update()
    {
        t += Time.deltaTime;
        if (Input.GetButton("Fire1") && weapon.guncooldown < t)
        {
            Shoot();
            t = 0;
        }
    }

    [Client]
    void Shoot()
    {
        CmdShootFX();
        RaycastHit _hit;

        if (gunSource != null)
        {
            gunSource.PlayOneShot(weapon.shootSound);
        }
        else
        {
            Debug.Log("Gun audio source not found");
        }

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
        {
            if (_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(_hit.collider.name, weapon.damage);
            }
            else
            {
                GameObject hole = (GameObject)GameObject.Instantiate(bulletHolePrefab, (_hit.point + _hit.normal * 0.01f), Quaternion.FromToRotation(-Vector3.forward, _hit.normal));
            }
        }

    }

    [Command]
    void CmdPlayerShot(string _playerID, int _damage)
    {
        Debug.Log(_playerID + " has been shot.");

        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage, GetComponent<Player>().playerName);
    }

    [Command]
    void CmdShootFX()
    {

    }

}

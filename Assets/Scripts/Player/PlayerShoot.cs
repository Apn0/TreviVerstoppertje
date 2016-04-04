using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    public PlayerWeapon weapon;
    public GameObject hitEffectPrefab;

    ParticleSystem muzzleFlash;

    public GameObject bulletHolePrefab;

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
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
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

    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    [ClientRpc]
    void RpcDoShootEffect()
    {
        muzzleFlash.Play();
    }

    //Is called on the server when we hit something
    //Takes in the hit point and the normal of the surface
    [Command]
    void CmdOnHit(Vector3 _pos, Vector3 _normal)
    {
        RpcDoHitEffect(_pos, _normal);
    }

    [ClientRpc]
    void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
    {
        GameObject _hitEffect = (GameObject)Instantiate(hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
        Destroy(_hitEffect, 2f);
        GameObject _hole = (GameObject)GameObject.Instantiate(bulletHolePrefab, _pos, Quaternion.FromToRotation(-Vector3.forward, _normal));
        Destroy(_hole, 10f);
    }

    [Client]
    void Shoot()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        CmdOnShoot();
        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
        {
            if (_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(_hit.collider.name, weapon.damage);
                Debug.Log("we hit a player!");
            }
            else
            {
                
                CmdOnHit((_hit.point + (_hit.normal * 0.01f)), _hit.normal);
                Debug.Log("we hit no player!");
            }
        }
        else
        {
            Debug.Log("we hit nothing!");
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

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    [Header("Sound Settings:")]
    [SerializeField]
    private AudioSource reloadGunAudio;
    [SerializeField]
    private AudioSource shootGunAudio;

    [Header("Camera Settings:")]
    [SerializeField]
    private Camera cam;

    [Header("Weapon Settings:")]
    [SerializeField]
    private GameObject[] weapons;

    [Header("Bullet Settings:")]
    [SerializeField]
    private LayerMask mask;

    [HideInInspector]
    public bool reloaded = false;

    private PlayerWeapon weapon;

    void Start()
    {
        ChangeWeapon(0);
        //weapon.Setup(); This Method has Been Write In ChangeWeapon Method
        if (cam == null)
        {
            Debug.LogError("Player Shoot: No camera refferenced!");
            enabled = false;
        }
    }

    void ChangeWeapon(int id)
    {
        weapons[id].SetActive(true);
        for (int i = 0; i < weapons.Length; i++)
        {
            if(i != id)
            {
                weapons[i].SetActive(false);
            }
        }
        weapon = weapons[id].GetComponent<PlayerWeapon>();
        weapon.Setup();
        reloaded = false;
    }

    void Update()
    {
        ReloadGun();
        if (PauseMenu.IsOn == true)
            return;
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width - 300, Screen.height / 2, 300, 100));

        GUILayout.BeginVertical();

        for(int i = 0; i < weapons.Length; i++)
        {
            PlayerWeapon model = weapons[i].GetComponent<PlayerWeapon>();
            if (model != weapon)
            {
                if (GUILayout.Button(model.name))
                {
                    GUI.skin.button.normal.textColor = Color.black;
                    ChangeWeapon(i);
                }
            }
        }

        GUILayout.EndVertical();

        GUILayout.EndArea();
    }

    void ReloadGun()
    {
        if(weapon.reload <= 0)
        {
            reloaded = true;
            reloadGunAudio.Stop();
        }
        else
        {
            reloaded = false;
            weapon.reload -= Time.deltaTime;
            if(reloadGunAudio.isPlaying == false)
            {
                reloadGunAudio.Play();
            }
        }
    }

    public IEnumerator TrueOffParticleGun()
    {
        yield return new WaitForSeconds(3f);

        weapon.TrueOffParticle();
    }

    [Client]
    void Shoot()
    {
        RaycastHit _hit;
        if(reloaded == true)
        {
            shootGunAudio.loop = false;
            shootGunAudio.Stop();
            shootGunAudio.Play();
            StartCoroutine(TrueOffParticleGun());
            if (weapon.particle.isPlaying == false)
                weapon.TrueOnParticle();
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
            {
                if (_hit.collider.tag == PLAYER_TAG)
                {
                    CmdPlayerShot(transform.gameObject, _hit.collider.name, weapon.GetNewDamge());
                }
            }
            weapon.ResetReload();
            reloaded = false;
            
        }
    }

    [Command]
    void CmdPlayerShot(GameObject obj, string _playerID, int damage)
    {
        Player player = GameManager.GetPlayer(_playerID.Replace("Player ", ""));
        player.RpcTakeDamage(obj, damage);
    }

}

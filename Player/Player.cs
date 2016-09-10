using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    [Header("GUI :")]
    [SerializeField]
    private GUISkin skin;
    [SerializeField]
    private Texture texture;
    [SerializeField]
    private TextMesh nameShow;

    [Header("GUI[DashBoard] :")]
    [SerializeField]
    private Texture HealthIcon;
    [SerializeField]
    private Texture PointIcon;
    [SerializeField]
    private Texture ReloadIcon;

    [Header("Effects :")]
    [SerializeField]
    private AudioSource soundOnHit;

    [Header("Data :")]
    [SerializeField]
    private int maxHealth = 100;
    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    private int defautMaxHealth;

    [SyncVar]
    private int currentHealth = 100;

    [SyncVar]
    private int points = 0;

    private int defualtPoints = 0;


    //Compon
    private PlayerShoot shoot;

    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    
    void Start()
    {
        shoot = GetComponent<PlayerShoot>();
    }
    

    public void Setup()
    {
        defautMaxHealth = maxHealth;
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }
        nameShow.text = transform.name;
        SetDefualts();
    }
    
    void OnGUI()
    {
        if (isLocalPlayer)
        {
            GUI.skin = skin;
            GUI.skin.label.normal.textColor = Color.white;
            Rect dataBoard = new Rect(Screen.width / 2 - 300, Screen.height - 100, 500, 100);
            Rect rectHealth = new Rect(Screen.width / 2 - 270, Screen.height - 100, 300, 100);
            Rect rectIconHealth = new Rect(Screen.width / 2 - 260, Screen.height - 85, 32, 32);
            Rect rectPoints = new Rect(Screen.width / 2 - 270, Screen.height - 65, 300, 100);
            Rect rectIconPoints = new Rect(Screen.width / 2 - 260, Screen.height - 50, 32, 32);
            Rect rectReload = new Rect(Screen.width / 2 - 70, Screen.height - 100, 300, 100);
            Rect rectIconReload = new Rect(Screen.width / 2 - 75, Screen.height - 85, 32, 32);
            GUI.DrawTexture(dataBoard, texture);
            GUI.DrawTexture(rectIconHealth, HealthIcon);
            GUI.Label(rectHealth, currentHealth.ToString());
            GUI.skin.label.fontSize = 25;
            GUI.DrawTexture(rectIconPoints, PointIcon);
            GUI.Label(rectPoints, points.ToString());
            GUI.skin.label.fontSize = 25;
            GUI.DrawTexture(rectIconReload, ReloadIcon);
            if (shoot.reloaded == true)
            {
                GUI.skin.label.normal.textColor = Color.green;
                GUI.Label(rectReload, "Reloaded");
            }else
            {
                GUI.skin.label.normal.textColor = Color.red;
                GUI.Label(rectReload, "Reloading");
            }
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3f);

        SetDefualts();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }

    public void SetDefualts()
    {
        isDead = false;
        maxHealth = defautMaxHealth;
        currentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = true;
    }

    [ClientRpc]
    public void RpcTakeDamage(GameObject obj, int amont)
    {
        if (isDead)
            return;

        currentHealth -= amont;
        Debug.Log(transform.name + " has been damage. The Health == " + currentHealth);

        if (currentHealth <= 0)
        {
            Player p = obj.GetComponent<Player>();
            if(p != null)
            {
                p.newPoint();
                Debug.Log("The " + obj.name + " Get The New Point is " + p.points);
            }else
            {
                Debug.LogError("The Object is Null");
            }
            Die();
        }
        else
        {
            soundOnHit.Stop();
            soundOnHit.Play();
        }
    }

    public void newPoint()
    {
        points += 1;
    }

    private void Die()
    {
        isDead = true;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;
        currentHealth = maxHealth;

        StartCoroutine(Respawn());
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (PauseMenu.IsOn == false)
                {
                    PauseMenu.IsOn = true;
                    Debug.Log("IsOn : " + PauseMenu.IsOn);
                }
                else
                {
                    PauseMenu.IsOn = false;
                }
            }
        }
    }


}

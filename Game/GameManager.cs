using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameManager in scene.");
        }
        else
        {
            instance = this;
        }
    }

    #region Match Manger
    


    #endregion

    #region Player tracking

    public const string PLAYER_ID_PREFIX = "Player ";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string _netID, Player _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_netID, _player);
        _player.transform.name = _playerID;
    }


    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    public static Player GetPlayer(string id)
    {
        return players[id];
    }
    
    [Header("Player List Settings:")]
    [SerializeField]
    private Texture2D playerList_Background;

    void OnGUI()
    {
        if(players.Keys.Count > 0)
        {
            GUILayout.BeginArea(new Rect(Screen.width / 2, 50, 200, 500));

            GUILayout.BeginVertical();
            GUILayout.Label("Players List:");
            foreach (string id in players.Keys)
            {
                GUI.skin.label.normal.background = playerList_Background;
                GUI.skin.label.normal.textColor = Color.red;
                GUILayout.Label(players[id].transform.name);
            }

            GUILayout.EndVertical();

            GUILayout.EndArea();
        }
        
    }

    #endregion

    #region Overrides 

    void Update()
    {
        if(players.Keys.Count == 0)
        {
            MouseShow.isOn = false;
        }
    }

    #endregion

}

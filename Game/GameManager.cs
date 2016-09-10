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
    private Texture playerList_Background;

    void OnGUI()
    {
        GUI.skin = skin;
        GUILayout.BeginArea(new Rect(Screen.width / 2, 50, 200, 500));

        GUILayout.BeginVertical();

        GUILayout.Box(playerList_Background);
        foreach (string id in players.Keys)
        {
            GUILayout.Label(id + "  -  " + players[id].transform.name);
        }

        GUILayout.EndVertical();

        GUILayout.EndArea();
    }

    #endregion
}

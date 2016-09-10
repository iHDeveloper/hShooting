using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class RoomScript : MonoBehaviour {

    [HideInInspector]
    public MatchDesc match;

    [HideInInspector]
    public GameObject RoomUI;

    [Header("Room Settings:")]
    [SerializeField]
    private Text name;
    [SerializeField]
    private Text size;

    private bool loading;

    private NetworkManager _networkManager;

    public void Setup()
    {
        _networkManager = NetworkManager.singleton;
        if (match != null)
        {
            name.text = match.name;
            size.text = "(" + match.currentSize + "/" + match.maxSize + ")";
        }
    }

    void OnGUI()
    {
        if (loading)
        {
            GUI.skin.label.fontStyle = FontStyle.Bold;
            GUI.skin.label.fontSize = 45;
            Rect rect = new Rect(Screen.width / 2, Screen.height - 75, 300, 100);
            GUI.Label(rect, "Joining");
        }
    }

    public void Join()
    {
        _networkManager.matchMaker.JoinMatch(match.networkId, "", OnJoinMatch);
        loading = true;
    }

    public void OnJoinMatch(JoinMatchResponse res)
    {
        if (res.success)
        {
            loading = false;
            if(RoomUI != null)
            {
                RoomUI.SetActive(false);
            }
            MatchInfo hostInfo = new MatchInfo(res);
            NetworkManager.singleton.StartClient(hostInfo);
        }
        else
        {
            Debug.LogError("Join match failed");
        }
    }

}

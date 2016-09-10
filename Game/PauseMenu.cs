using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class PauseMenu : MonoBehaviour {

    public static bool IsOn = false;

    [Header("Pause Settings")]
    [SerializeField]
    private GameObject PauseUI;
    [SerializeField]
    private GameObject RoomsUI;

    private NetworkManager _networkManager;

    void Start()
    {
        _networkManager = NetworkManager.singleton;
    }

    void Update()
    {
        if (IsOn)
        {
            if (PauseUI != null)
                PauseUI.SetActive(true);
        }else
        {
            if (PauseUI != null)
                PauseUI.SetActive(false);
        }
    }

    public void LeaveRoom()
    {
        MatchInfo info = _networkManager.matchInfo;
        _networkManager.matchMaker.DropConnection(info.networkId, info.nodeId, OnDrop);
        _networkManager.StopHost();
        PauseMenu.IsOn = false;
        PauseUI.SetActive(false);
        RoomsUI.SetActive(true);
    }

    void OnDrop(BasicResponse res)
    {
        Debug.Log("Dropped!");
    }

}

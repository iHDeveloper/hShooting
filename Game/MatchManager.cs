using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Networking.Match;
using System.Collections.Generic;
public class MatchManager : MonoBehaviour {

    [SerializeField]
    private uint maxSize = 6;

    private string name = "";

    private List<GameObject> matchList = new List<GameObject>();

    [Header("Room Settings:")]
    //[SerializeField]
    //private GameObject RoomUI;
    [SerializeField]
    private GameObject Content;
    [SerializeField]
    private GameObject Prefab;
    [SerializeField]
    private Text state;

    private NetworkManager _networkManger;

    void Start()
    {
        _networkManger = NetworkManager.singleton;
        if(_networkManger != null)
        {
            _networkManger.StartMatchMaker();
            RefreshInternetMatch();
        }
    }

    public void SetRoomName(InputField name)
    {
        this.name = name.text;
    }

    public void Host()
    {
        CreateMatchRequest match = new CreateMatchRequest();
        match.name = name;
        match.size = maxSize;
        match.advertise = false;
        match.password = "";

        _networkManger.matchMaker.CreateMatch(match, OnCreateMatch);
        ClearMatchList();
        state.text = "Creating...";
    }

    private void OnCreateMatch(CreateMatchResponse res)
    {
        if (res != null && res.success)
        {
            //_networkManger.OnMatchCreate(res);  
            GameObject obj = GameObject.Find("RoomsUI");
            if(obj != null)
            {
               obj.SetActive(false);
            }
            res.port = 7000;
            Debug.Log("Create match succeeded Port: " + res.port);
            MatchInfo hostInfo = new MatchInfo(res);
            NetworkServer.Listen(hostInfo, 9000);
            Debug.Log("Host Port: " + hostInfo.port);
            _networkManger.StartHost(hostInfo);
        }
        else
        {
            Debug.LogError("Create match failed");
        }

    }

    public void RefreshInternetMatch()
    {
        ClearMatchList();
        NetworkManager.singleton.matchMaker.ListMatches(0, 20, "", OnInternetMatchList);
        state.text = "Loading...";
    }
    
    public void ClearMatchList()
    {
        foreach(GameObject obj in matchList)
        {
            Destroy(obj);
        }
        matchList.Clear();
    }

    private void OnInternetMatchList(ListMatchResponse matchListResponse)
    {
        ClearMatchList();
        if (matchListResponse.success)
        {
            state.text = "";
            if (matchListResponse.matches.Count != 0)
            {
                foreach(MatchDesc match in matchListResponse.matches)
                {
                    GameObject obj = Instantiate(Prefab);
                    RoomScript room = obj.GetComponent<RoomScript>();
                    if (room != null)
                    {
                        room.match = match;
                        room.RoomUI = GameObject.Find("RoomsUI");
                        room.Setup();
                    }
                    obj.transform.parent = Content.transform;
                    matchList.Add(obj);
                }
            }
            else
            {
                state.text = "Sorry, No Rooms :(";
            }
        }
        else
        {
            state.text = "Not Connect :(";
        }
    }

}

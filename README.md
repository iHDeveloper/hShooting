# hShooting
This Game is Open Source (Ex: Runing In Web/IOS/Android/PS/Xbox)

## Game ( Script Folder )
#### **C#**
>  ***Game Manager.cs :***
- **Information :**
- This Class is Manage The Game for example (Ex: Register Player, UnRegister, Get Player, Get PlayerList)
- **Methods :**
```
void Awake()
public static void RegisterPlayer(string _netID, Player _player)
public static void UnRegisterPlayer(string _playerID)
public static Player GetPlayer(string id)
```
> ***Match Manager :***
- **Information :**
- This Class is Manage The Match
- **Methods :**
```
void Start()
public void SetRoomName(InputField name)
public void Host()
private void OnCreateMatch(CreateMatchResponse res)
public void RefreshInternetMatch()
public void ClearMatchList()
private void OnInternetMatchList(ListMatchResponse matchListResponse)
```
> ***Pause Menu :***
- **Information :**
- This Class is If The Player Click (ESC) Than Pause The Controller
- **Methods :**
```
void Start()
void Update()
public void LeaveRoom()
void OnDrop(BasicResponse res)
```



##### README Version: V1.1.1

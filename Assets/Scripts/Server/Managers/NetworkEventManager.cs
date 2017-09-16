using UnityEngine;

public class NetworkEventManager : Photon.MonoBehaviour 
{
	public static NetworkEventManager Instance;

	private const string roomName = "Room69";
    private RoomInfo[] roomsList;


	// Use this for initialization
	void Awake () 
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy (this);

		DontDestroyOnLoad (this);
        
	}


	void OnGUI()
	{
        //if(SceneManagerHelper.ActiveSceneBuildIndex == 0)
		//GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());

        /*
		// if we're not in a room
		if (PhotonNetwork.insideLobby && PhotonNetwork.room == null) 
		{
			// Create Room
			if (GUI.Button (new Rect (Screen.width / 2 - 50, Screen.height / 2, 100, 30), "Create Match"))
				PhotonNetwork.CreateRoom (roomName);

			// Join Room
			if (roomsList != null) 
			{
				for (int i = 0; i < roomsList.Length; i++) 
				{
					if (GUI.Button (new Rect (Screen.width / 2 - 50, Screen.height / 2 + 100, 100, 30), roomsList[i].Name))
						PhotonNetwork.JoinRoom (roomsList [i].Name);
				}
			}
		}
		// Display temp friend list
		if (PhotonNetwork.insideLobby) 
		{
			if (GUI.Button (new Rect (Screen.width / 2 - 50, Screen.height / 2 - 100, 100, 30), "Poll friends list")) 
			{
				string[] friends = new string[] { "dev", "olivia", "lulu" };
				PhotonNetwork.FindFriends (friends);
			}
		}*/
	}


    // Call automatically after we poll the friend list
    void OnUpdatedFriendList()
    {
        if (PhotonNetwork.Friends != null)
        {
            foreach (FriendInfo friend in PhotonNetwork.Friends)
            {
                Debug.Log(friend.Name + " is online: " + friend.IsOnline + " | In game: " + friend.IsInRoom);
            }
        }
        else
        {
            Debug.Log("You have no friends :c");
        }
    }


    // this is always automatically called when a new room is created, therefore we can use this
    // as the main point for the invite system
    void OnReceivedRoomListUpdate()
	{
		roomsList = PhotonNetwork.GetRoomList();
        
		Debug.Log ("Received room list update");
	}


    void OnJoinedLobby()
    {
        Debug.Log(PhotonNetwork.playerName + " joined the lobby");
    }


    void OnConnectedToMaster()
	{
		Debug.Log ("ConnectedToMaster");
        PhotonNetwork.JoinLobby();
	}


	void OnJoinedRoom()
	{
		Debug.Log("Connected to Room");
	}
}

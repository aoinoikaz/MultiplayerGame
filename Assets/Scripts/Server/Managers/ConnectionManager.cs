using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ConnectionManager : MonoBehaviour 
{
	public static ConnectionManager Instance;

	// Use this for initialization
	void Awake () 
	{
		if(Instance == null)
			Instance = this;
		else if(Instance != this)
			Destroy(this);

		DontDestroyOnLoad (this);
	}


	// TODO fix this cuz its terrible practice
	public IEnumerator Connect(string username)
	{
		PhotonNetwork.playerName = username;

		bool connected = PhotonNetwork.ConnectUsingSettings (Constants.GameVersion);

		if (connected)
        {
            AuthenticationManager.Instance.AuthenticatedUsername = username;
            yield break;
        }
    }

}

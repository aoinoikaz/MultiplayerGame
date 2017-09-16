using System.Collections;

using UnityEngine;

public class AuthenticationManager : MonoBehaviour 
{
	public static AuthenticationManager Instance;

	// This event passes the authentication response back to its subscribed callback
	public delegate IEnumerator OnAuthenticationResponse(LoginResponse loginResponse, string returnMessage);
	public event OnAuthenticationResponse OnAuthenticationFailed;
	public event OnAuthenticationResponse OnAuthenticationSuccess;

    public string AuthenticatedUsername { get; set; }

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy (this);

		DontDestroyOnLoad (this);
	}


	// This function sends the data to the server, waits for a response
	// and delegates the response back to the subscribed method
	public IEnumerator Authenticate(string username, string password)
	{
		string returnMessage = null;
		LoginResponse? responseType = null;

		// The form that will be used to send data
		WWWForm form = new WWWForm();
		form.AddField ("dsnloginusername", username);
		form.AddField ("dsnloginpassword", password);

		WWW request  = new WWW (Constants.LoginServerURL, form);

		yield return request;

		// Server returned an error
		if (!string.IsNullOrEmpty (request.error))
		{
			returnMessage = request.error;
			responseType = LoginResponse.Error;
		}

		// Returned an authentication response or server sided error
		if (!string.IsNullOrEmpty (request.text)) 
		{
			returnMessage = request.text;

			responseType = returnMessage.Equals (Constants.AuthenticationSuccess) ? LoginResponse.Success : 
				returnMessage.Equals (Constants.AuthenticationFailed) ? LoginResponse.Failed : 
				LoginResponse.Error;
		}

		// been authenticated
		if (responseType == LoginResponse.Success)
		{
			if (OnAuthenticationSuccess != null) 
			{
				StartCoroutine (OnAuthenticationSuccess (responseType.Value, returnMessage));
			}
		}
		else
		{
			if (OnAuthenticationSuccess != null) 
			{
				StartCoroutine(OnAuthenticationFailed(responseType.Value, returnMessage));
			}
		}
	}
}

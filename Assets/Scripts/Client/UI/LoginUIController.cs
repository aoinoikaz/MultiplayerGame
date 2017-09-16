using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LoginUIController : MonoBehaviour 
{
	private InputField UsernameInputField;
	private InputField PasswordInputField;
	private Text LoginLabel;

	private SceneController sceneController;
	private CanvasGroup canvasGroup;

	private string username, password;

	void Awake()
	{
		// subscribe to our authentication handler events
		AuthenticationManager.Instance.OnAuthenticationSuccess += OnAuthenticationSuccess;
		AuthenticationManager.Instance.OnAuthenticationFailed += OnAuthenticationFailed;

		sceneController = FindObjectOfType<SceneController> ();

		canvasGroup = GameObject.Find ("Canvas").GetComponentInChildren<CanvasGroup> ();
		UsernameInputField = GameObject.Find ("UsernameInputField").GetComponent<InputField> ();
		PasswordInputField = GameObject.Find ("PasswordInputField").GetComponent<InputField> ();
		LoginLabel = GameObject.Find ("LoginLabel").GetComponent<Text> ();

		canvasGroup.blocksRaycasts = false;
	}


	public void Init()
	{
		StartCoroutine (StartAuthenticationProcess ());
	}


	// UI login button triggers this
	public IEnumerator StartAuthenticationProcess()
	{
		canvasGroup.blocksRaycasts = true;

		LoginLabel.text = "Authenticating...";
		username = UsernameInputField.text;
		password = PasswordInputField.text;

		// The user has inputted data
		if (string.IsNullOrEmpty (username) || string.IsNullOrEmpty (password)) 
		{
			UsernameInputField.text = string.Empty;
			PasswordInputField.text = string.Empty;
			LoginLabel.text = "Fields cannot be blank...";
			yield return new WaitForSeconds (2f);
			LoginLabel.text = string.Empty;
			canvasGroup.blocksRaycasts = false;

			yield break;
		}
		StartCoroutine (AuthenticationManager.Instance.Authenticate (username, password));
	}


	private IEnumerator OnAuthenticationSuccess (LoginResponse loginResponse, string returnMessage)
	{
		LoginLabel.text = "Authenticated!";
		yield return new WaitForSeconds (1f);
		LoginLabel.text = "Connecting to server...";
		yield return StartCoroutine (ConnectionManager.Instance.Connect(username));
		yield return new WaitForSeconds (1f);
		LoginLabel.text = string.Empty;
		sceneController.FadeAndLoadScene ("Lobby");
		yield break;
	}


	private IEnumerator OnAuthenticationFailed (LoginResponse loginResponse, string returnMessage)
	{
		if (loginResponse == LoginResponse.Failed)
			LoginLabel.text = Constants.InvalidCredentials;
		else if (loginResponse == LoginResponse.Error)
			LoginLabel.text = Constants.ServerError;
		
		yield return new WaitForSeconds (2f);
		LoginLabel.text = string.Empty;
		canvasGroup.blocksRaycasts = false;
	}


	void OnDisable()
	{
		if (AuthenticationManager.Instance != null) 
		{
			// Always unsubscribe or itll cause memory leaks :/
			AuthenticationManager.Instance.OnAuthenticationFailed -= OnAuthenticationFailed;
			AuthenticationManager.Instance.OnAuthenticationSuccess -= OnAuthenticationSuccess;
		}
	}
}

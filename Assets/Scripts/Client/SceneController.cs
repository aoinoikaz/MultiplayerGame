using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour 
{
	// Used for triggering events before scene and after
	public event Action BeforeSceneUnload;
	public event Action AfterSceneLoad;

	private CanvasGroup FaderCanvasGroup;

	public float FadeDuration;

	private bool isFading;

	// Use this for initialization
	private IEnumerator Start () 
	{
		FaderCanvasGroup = GameObject.Find ("FadeImage").GetComponent<CanvasGroup> ();
		FaderCanvasGroup.alpha = 1f;

		// fade into a scene
		yield return StartCoroutine (Fade (0f));
	}


	public void FadeAndLoadScene(string sceneName)
	{
		if (!isFading) 
		{
			StartCoroutine (FadeAndSwitchScenes (sceneName));
		}
	}


	// this is used to unload the scene we're currently in and load the next scene
	private IEnumerator FadeAndSwitchScenes(string sceneName)
	{
		// Fade screen to black
		yield return StartCoroutine (Fade (1f));

		if (BeforeSceneUnload != null) 
		{
			BeforeSceneUnload ();
		}

		yield return SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Single);

		// Unity keeps track of all scenes loaded by index, 
		// therefore take the newly loaded scene
		Scene newlyLoadedScene = SceneManager.GetSceneAt (SceneManager.sceneCount-1);
		Scene sceneToUnload = SceneManager.GetActiveScene ();

		// Set the desired scene active
		SceneManager.SetActiveScene (newlyLoadedScene);

		// Unload the previous scene we came from 
		SceneManager.UnloadSceneAsync (sceneToUnload.buildIndex);

		if (AfterSceneLoad != null) 
		{
			AfterSceneLoad ();
		}

		yield return StartCoroutine (Fade (0f));
	}


	private IEnumerator Fade(float finalAlpha)
	{
		isFading = true;
		FaderCanvasGroup.blocksRaycasts = true;

		// Get the difference between the current alpha and the destined alpha 
		float fadeSpeed = Mathf.Abs (FaderCanvasGroup.alpha - finalAlpha) / FadeDuration;

		// Continue iterating while the current alpha is not approximately equal
		// to the final alpha
		while(!Mathf.Approximately(FaderCanvasGroup.alpha, finalAlpha))
		{
			// increment the alpha over fade speed time
			FaderCanvasGroup.alpha = Mathf.MoveTowards(FaderCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
			
			// Exit the iteration and come back for the next frame
			yield return null;
		}

		isFading = false;
		FaderCanvasGroup.blocksRaycasts = false;
	}

}

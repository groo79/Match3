using UnityEngine;
using System.Collections;

public class Buttons : MonoBehaviour {

	public AudioSource sound;
	bool startGame = false;
	AsyncOperation async;

	// Use this for initialization

	void Start()
	{
		startGame = false;
		StartCoroutine(LevelLoad());
	}

	IEnumerator LevelLoad()
	{
		async = Application.LoadLevelAsync(1);
		async.allowSceneActivation = false;
		yield return async;
	}

	public void StartGame()
	{
		sound.Play();
		startGame = true;
	}

	void Update()
	{
		if (startGame && !sound.isPlaying)
		{
			async.allowSceneActivation = true;
		}
	}
}

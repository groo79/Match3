using UnityEngine;
using System.Collections;

public class Buttons : MonoBehaviour {

	public AudioSource sound;
	bool startGame = false;

	// Use this for initialization

	void Start()
	{
		startGame = false;
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
			Application.LoadLevel(1);
		}
	}
}

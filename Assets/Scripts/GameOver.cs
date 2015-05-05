using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	/*
	 * Gary Horner
	*/

	// Use this for initialization
	public void Restart()
	{
		int curLevel;
		curLevel = Application.loadedLevel;
		Time.timeScale = 1;
		Application.LoadLevel(curLevel);
	}

	public void ReturnToMainMenu()
	{
		Time.timeScale = 1;
		Application.LoadLevel(0);
	}
}

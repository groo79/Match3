using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Options : MonoBehaviour {

	public GameObject optionsPanel;
	public Button resetButton;
	bool active;
	float timer;


	private Animator optionsAnim;

	public void SetActive()
	{
		active = true;
		optionsPanel.SetActive(true);
		optionsAnim.Play("open");

	}

	public void resetHighScore()
	{
		PlayerPrefsPro.SetInt("HighScore",0);
	}

	public void exitOptions()
	{
		optionsAnim.Play("close");
		active = false;
	}


	// Use this for initialization
	void Start () {
		optionsAnim = optionsPanel.GetComponent<Animator>();
		optionsPanel.SetActive(false);
		active = false;
		if (PlayerPrefsPro.GetInt("HighScore") == 0)
		{
			SetActive();
			resetButton.interactable = false;
		}

	
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if (active == false)
		{
			timer = 0f;
			if (timer >= 1f)
				optionsPanel.SetActive(false);
		}
	
	}

}

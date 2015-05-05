using UnityEngine;
using System.Collections;

public class BombIcon : MonoBehaviour {

	public GameManager manager;
	public SpriteRenderer sprite;

	public ParticleSystem flare;

	public static bool active;

	public int activationNumber = 4;

	public void setActive()
	{
		sprite.color = Color.white;
		active = true;
	}

	public void deactivate()
	{
		flare.Stop();
		sprite.color = Color.black;
		active = false;
	}

	public bool isActive()
	{
		return active;
	}

	// Use this for initialization
	void Start () {

		manager = FindObjectOfType<GameManager>();
	
	}
	
	// Update is called once per frame
	void Update () {


	
	}

	void OnMouseOver()
	{
		if (GameManager.Playing && active)
		{

			if (Input.GetMouseButtonDown(0))
			{
				flare.Play();
				manager.bombTrigger = true;
				//Debug.Log("kaboom");
			}
		}
	}


}

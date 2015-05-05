using UnityEngine;
using System.Collections;

public class Blocks : MonoBehaviour {

	/*
	 * Gary Horner
	 * credit to Dragutin via http://unityplus.blogspot.com/2014/05/match3-game-tutorial.html
	 * for help getting the match 3 mechanic working.
	*/

	public int ID;
	public int x;
	public int y;


	public float delay = .8f;
	public ParticleSystem poof ;

	private Vector2 scale;
	private float startTime;

	public static Transform select;
	public static Transform moveTo;

	private float timer;
	private SpriteRenderer sprite;

	private int Score = 10;

	public AudioSource sound;
	private AudioClip selectSound;

	private GameManager manager;
	private Treasure chest;

	public int GetScore()
	{
		return Score;
	}

	public void Poof()
	{
		poof.Play();
	}

//
	void Awake () {

		poof = GetComponentInChildren<ParticleSystem>();
		sprite = GetComponent<SpriteRenderer>();
		sound = gameObject.AddComponent<AudioSource>();
		manager = GameObject.FindWithTag(Tags.gameManager).GetComponent<GameManager>();
		selectSound = manager.SelectSound;
		sound.volume = .25f;
		sprite.enabled = false;
		Poof();
		scale = transform.localScale;

		if (gameObject.tag == Tags.treasure)
		{
			chest = GetComponent<Treasure>();
		}


	
	}
	
	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;

		if (timer >= delay)
		{
			sprite.enabled = true;
		}


			
	}

	void OnMouseOver()
	{
		if (GameManager.Playing)
		{
			//Debug.Log("mouse over block "+x+", "+y);
			transform.localScale = new Vector2(scale.x+.2f, scale.y+.2f);
			if (Input.GetMouseButtonDown(0)&& manager.bombTrigger)
			{
				moveTo = transform;
				sound.PlayOneShot(selectSound, 0.1f);
			}
			if (!select && Input.GetMouseButtonDown(0)&& gameObject.tag == Tags.treasure)
			{
				if (!chest.IsOpen())
				{
					Debug.Log("Treasure Block");
					select = transform;
					moveTo = transform;
					manager.treasureSelected();
				}
			}
			else if (Input.GetMouseButtonDown(0)&& !manager.bombTrigger)
			{
				if (!select)
				{
					select = transform;
					sound.PlayOneShot(selectSound, 0.1f);
				}

				else if(select != transform && !moveTo)
				{
					moveTo = transform;
					sound.PlayOneShot(selectSound, 0.1f);
				}
			}



		}
	}
	void OnMouseExit()
	{
		transform.localScale = scale;
	}
}

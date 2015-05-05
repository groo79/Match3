using UnityEngine;
using System.Collections;

public class Treasure : MonoBehaviour {

	public AudioClip goodSound;
	public float goodVol = 1;
	public AudioClip mehSound;
	public float mehVol = 1;
	public AudioClip badSound;
	public float badVol = 1;

	AudioSource sound;

	public ParticleSystem goodJob;
	public ParticleSystem meh;
	public ParticleSystem thatSucks;
	public Sprite openChest;

	SpriteRenderer sprite;

	public int bonus;
	public int damage;
	bool open;

	GameManager manager;

	public int action;

	public bool IsOpen()
	{
		return open;
	}

	public void OpenChest()
	{
		open = true;
		sprite.sprite = openChest;
	}

	public void Randomize()
	{
		action = Random.Range(1,4);
	}

	public void PlaySounds()
	{
		if (!open)
		{
			if (action == 1)
				sound.PlayOneShot(goodSound,goodVol);
			if (action == 2)
				sound.PlayOneShot(mehSound, mehVol);
			if (action == 3)
				sound.PlayOneShot(badSound, badVol);
		}
	}

	// Use this for initialization
	void Awake () {
		manager = FindObjectOfType<GameManager>();
		sound = GetComponent<AudioSource>();
		sprite = GetComponent<SpriteRenderer>();
		Randomize();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

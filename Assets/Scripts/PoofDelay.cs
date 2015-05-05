using UnityEngine;
using System.Collections;

public class PoofDelay : MonoBehaviour {

	public float delay;

	float timer;
	SpriteRenderer sprite;

	// Use this for initialization
	void Awake () {
		timer = 0;
		sprite = GetComponent<SpriteRenderer>();
		sprite.enabled = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer >= delay)
		{
			sprite.enabled = true;
		}
	
	}
}

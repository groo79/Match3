using UnityEngine;
using System.Collections;

public class ParticleDestroy : MonoBehaviour {

	public float delayAfterDuration;

	private ParticleSystem part;

	// Use this for initialization
	void Start () {
	part = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		Destroy(gameObject, part.duration+delayAfterDuration);
	
	}
}

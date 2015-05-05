using UnityEngine;
using System.Collections;

public class Backgrounds : MonoBehaviour {

	/*
	 * Gary Horner
	*/

	public GameObject[] castlegroup;
	public GameObject[] cloudGroup;
	public GameObject[] houseGroup;
	public GameObject[] treeGroup;

	int castles = 0;
	int clouds = 0;
	int houses = 0;
	int trees = 0;

	void hideBackground(GameObject[] group)
	{
		for (int i = 0; i < group.Length; i++)
		{
			group[i].SetActive(false);
		}
	}

	public void activateBackground(Transform obj)
	{
		if (obj.tag == Tags.castle)
		{
			if (castles < castlegroup.Length)
			{
				castlegroup[castles].SetActive(true);
				castles++;
			}

		}
		if (obj.tag == Tags.cloud)
		{
			if (clouds < cloudGroup.Length)
			{
				cloudGroup[clouds].SetActive(true);
				clouds++;
			}
			
		}
		if (obj.tag == Tags.house)
		{
			if (houses < houseGroup.Length)
			{
				houseGroup[houses].SetActive(true);
				houses++;
			}
			
		}
		if (obj.tag == Tags.tree)
		{
			if (trees < treeGroup.Length)
			{
				treeGroup[trees].SetActive(true);
				trees++;
			}
			
		}
	}
	

	// Use this for initialization
	void Start () {
		hideBackground(castlegroup);
		hideBackground(cloudGroup);
		hideBackground(houseGroup);
		hideBackground(treeGroup);

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

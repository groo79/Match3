using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	/*
	 * Gary Horner
	 * credit to Dragutin via http://unityplus.blogspot.com/2014/05/match3-game-tutorial.html
	 * for help getting the match 3 mechanic working.
	*/

	public static bool Playing;
	bool refreshing = false;

	public Text scoreText;
	public Text timerText;
	public Text statusText;
	public Backgrounds backgroundParent;
	public int timeLimit = 100;
	public Animator gameOverSlide;
	public Text FinalScoreText;
	public Text bonusScoreText;
	public Text highScoreText;
	public Text highScoreScreenText;
	public Button refreshButton;
	public float waitTime = 1f;

	public int[,] board;

	public int refreshCost;

	private int score;
	private int bonusScore;
	private int countDownTimer;
	private float timer;
	private float waitTimer;
	private bool bombUsed;

	public Transform[] block;
	public BombIcon bomb;
	public Animator bombText;
	public AudioClip bombexplode;
	public bool bombTrigger;
	public int noBombBonus;

	public bool treasure = false;

	private int bonus = 0;

	public int typeCount = 2;
	private int castleCount = 0;
	private int houseCount = 0;
	private int cloudCount = 0;
	private int treeCount = 0;

	//audio 
	private AudioSource sound;
	public AudioClip SelectSound;
	public AudioClip matchSound;
	public float matchVol = .2f;
	public AudioClip noMatchSound;
	public float noMatchVol = .2f;
	public AudioClip gameOverSound;
	public float gameOverVol = .2f;
	public AudioClip highScoreSound;
	public float highScoreVol = .2f;



	void NewBoard()
	{
		for (int x = 0; x < board.GetLength(0); x++)
		{
			for (int y = 0; y < board.GetLength(1); y++)
			{
				int randNum = Random.Range(0, block.Length);
				Transform obj = (Transform)Instantiate(block[randNum].transform, new Vector2(x, y), Quaternion.identity) as Transform;
				obj.parent = transform;
				obj.name = "Block[X:"+x+"Y:"+y+"]ID";
				board[x,y] = randNum;

				Blocks b = obj.gameObject.AddComponent<Blocks>();
				b.x = x;
				b.y = y;
				b.ID = randNum;
			}
		}
		sound.PlayOneShot(matchSound, matchVol);
	}

	public void treasureSelected()
	{
		treasure = true;
		Debug.Log("treasure selected");
	}

	public void refreshBoard()
	{
		if (!refreshing)
		{
			bool bombOver = BombIcon.active;
			Debug.Log ("bomb status "+bombOver);
			refreshing = true;
			Blocks[] allBlocks = FindObjectsOfType(typeof(Blocks)) as Blocks[];
			score -= refreshCost;
			scoreText.text = "Score "+score;
			sound.PlayOneShot(matchSound);
			foreach(Blocks b in allBlocks)
			{
					Destroy(b.gameObject);
					board[b.x, b.y] = 500;
			}
			Respawn();
			waitTimer = 0;
			if (bombOver == false)
				bomb.deactivate();
		}
		
	}

	void checkTags(Transform selected)
	{
		if (selected.tag == Tags.house)
		{
			houseCount++;
			if (houseCount >= typeCount)
			{
				backgroundParent.activateBackground(selected);
				houseCount = 0;
			}
		}
		if (selected.tag == Tags.cloud)
		{
			cloudCount++;
			if (cloudCount >= typeCount)
			{
				backgroundParent.activateBackground(selected);
				cloudCount = 0;
			}
		}
		if (selected.tag == Tags.castle)
		{
			castleCount++;
			if (castleCount >= typeCount)
			{
				backgroundParent.activateBackground(selected);
				castleCount = 0;
			}
		}
		if (selected.tag == Tags.tree)
		{
			treeCount++;
			if (treeCount >= typeCount)
			{
				backgroundParent.activateBackground(selected);
				treeCount = 0;
			}
		}


	}
	
	bool CheckBoardForMatches()
	{
		Blocks[] allBlocks = FindObjectsOfType(typeof(Blocks)) as Blocks[];
		bool matches = false;

		for (int x = 0; x < board.GetLength(0); x++)
		{
			for (int y = 0; y < board.GetLength(1); y++)
			{
				foreach(Blocks b in allBlocks) 
				{
					if (CheckForMatch(b.gameObject.GetComponent<Blocks>()) == true)
						matches = true;
				}
			}
		}
		if (matches)
		{
			return true;
		}
		else 
		{
			return false;
		}
	}

	void ExplodeBomb()
	{
		Blocks[] allBlocks = FindObjectsOfType(typeof(Blocks)) as Blocks[];
		Blocks mov = Blocks.moveTo.gameObject.GetComponent<Blocks>();
		foreach (Blocks b in allBlocks)
		{
			if (b.x == mov.x && b.y == mov.y)
			{
				Destroy(b.gameObject);
				board[b.x, b.y] = 500;
			}
			if (b.x == mov.x-1 && b.y == mov.y)
			{
				Destroy(b.gameObject);
				board[b.x, b.y] = 500;
			}
			if (b.x == mov.x+1 && b.y == mov.y)
			{
				Destroy(b.gameObject);
				board[b.x, b.y] = 500;
			}
			if (b.x == mov.x-1 && b.y == mov.y-1)
			{
				Destroy(b.gameObject);
				board[b.x, b.y] = 500;
			}
			if (b.x == mov.x && b.y == mov.y-1)
			{
				Destroy(b.gameObject);
				board[b.x, b.y] = 500;
			}
			if (b.x == mov.x+1 && b.y == mov.y-1)
			{
				Destroy(b.gameObject);
				board[b.x, b.y] = 500;
			}
			if (b.x == mov.x-1 && b.y == mov.y+1)
			{
				Destroy(b.gameObject);
				board[b.x, b.y] = 500;
			}
			if (b.x == mov.x && b.y == mov.y+1)
			{
				Destroy(b.gameObject);
				board[b.x, b.y] = 500;
			}
			if (b.x == mov.x+1 && b.y == mov.y+1)
			{
				Destroy(b.gameObject);
				board[b.x, b.y] = 500;
			}
		}
		Respawn();

	}

	void GoodTreasure()
	{
		Treasure chest = Blocks.select.GetComponent<Treasure>();
		chest.goodJob.Play();
		score += chest.bonus;
		scoreText.text = "Score "+score;

	}

	void MehTreasure()
	{
		Treasure chest = Blocks.select.GetComponent<Treasure>();
		chest.meh.Play();

	}

	void BadTreasure()
	{
		Treasure chest = Blocks.select.GetComponent<Treasure>();
		chest.thatSucks.Play();
		if (score >= chest.damage)
		{
			score -= chest.damage;
		}
		else if (score < chest.damage)
		{
			int overage = chest.damage - score;
			score = 0;
			countDownTimer -= overage;

		}
		scoreText.text = "Score "+score;
	}

	void Respawn()
	{
		int arrayPos = 0;
		List<Transform> replaced = new List<Transform>();
		for (int x = 0; x < board.GetLength(0); x++)
		{
			for (int y = 0; y < board.GetLength(1); y++)
			{
				if (board[x,y] == 500)
				{
					int randNum = Random.Range(0, block.Length);
					Transform obj = (Transform)Instantiate(block[randNum].transform, new Vector2(x, y), Quaternion.identity) as Transform;
					obj.parent = transform;
					Blocks b = obj.gameObject.AddComponent<Blocks>();
					b.x = x;
					b.y = y;
					b.ID = randNum;
					board[x,y] = randNum;
					replaced.Add(obj);
					arrayPos++;
				}
			}
		}
		for (int i = 0; i<replaced.Count;i++)
		{
			Blocks check = replaced[i].GetComponent<Blocks>();
			if (CheckForMatch(check))
			{
				score += check.GetScore();
				//Debug.Log("extra matches");
			}
		}
	}

	bool CheckIfNear()
	{
		Blocks sel = Blocks.select.gameObject.GetComponent<Blocks>();
		Blocks mov = Blocks.moveTo.gameObject.GetComponent<Blocks>();

		if (sel.x-1 == mov.x && sel.y == mov.y)
		{
			return true;
		}
		if (sel.x+1 == mov.x && sel.y == mov.y)
		{
			return true;
		}
		if (sel.x == mov.x && sel.y-1 == mov.y)
		{
			return true;
		}
		if (sel.x == mov.x && sel.y+1 == mov.y)
		{
			return true;
		}
		//Debug.Log("not near each other");
		return false;
	}

	void SwitchBlocks()
	{
		Blocks sel = Blocks.select.gameObject.GetComponent<Blocks>();
		Blocks mov = Blocks.moveTo.gameObject.GetComponent<Blocks>();

		Vector2 tempP = sel.transform.position;
		int tempX = sel.x;
		int tempY = sel.y;

		sel.transform.position = mov.transform.position;
		mov.transform.position = tempP;

		sel.x = mov.x;
		sel.y = mov.y;
		mov.x = tempX;
		mov.y = tempY;
		board[sel.x, sel.y] = sel.ID;
		board[mov.x, mov.y] = mov.ID;
	}

	bool CheckForMatch(Blocks blockToCheck)
	{
		Blocks[] allBlocks = FindObjectsOfType(typeof(Blocks)) as Blocks[];
		Blocks selected = blockToCheck;


		int countUp = 0;
		int countDown = 0;
		int countLeft = 0;
		int countRight = 0;

		bool h = false;
		bool v = false;

		//Check Left
		for (int i = selected.x-1; i >= 0; i--)
		{
			if (board[i,selected.y] == selected.ID)
			{
				countLeft++;
				//Debug.Log("+1 left");
			}
			if (board[i,selected.y] != selected.ID)
			{
				break;
			}
		}

		//Check right
		for (int i = selected.x; i < board.GetLength(0); i++)
		{
			if (board[i,selected.y] == selected.ID)
			{
				countRight++;
				//Debug.Log("+1 right");
			}
			if (board[i,selected.y] != selected.ID)
			{
				break;
			}
		}

		//Check Up
		for (int i = selected.y; i < board.GetLength(1); i++)
		{
			if (board[selected.x,i] == selected.ID)
			{
				countUp++;
				//Debug.Log("+1 up");
			}
			if (board[selected.x,i] != selected.ID)
			{
				break;
			}
		}

		//Check Down
		for (int i = selected.y-1; i >=0; i--)
		{
			if (board[selected.x,i] == selected.ID)
			{
				countDown++;
				//Debug.Log("+1 down");
			}
			if (board[selected.x,i] != selected.ID)
			{
				break;
			}
		}

		if ((countLeft+countRight)>=3 || (countUp+countDown)>=3)
		{
			//Debug.Log("Match 3");
			if (countLeft+countRight >=3)
			{
				h = true;
				//Debug.Log("Horizontal Match");
				for (int del = 0; del <= countLeft; del++)
				{
					foreach(Blocks b in allBlocks)
					{
						if(b.x == selected.x-del && b.y == selected.y)
						{
							//Debug.Log("Block "+b.x+", "+b.y+" destroyed");
							Destroy(b.gameObject);
							board[b.x, b.y] = 500;
						}
					}
				}
				for (int del = 0; del < countRight; del++)
				{
					foreach(Blocks b in allBlocks)
					{
						if(b.x == selected.x+del && b.y == selected.y)
						{
							//Debug.Log("Block "+b.x+", "+b.y+" destroyed");
							Destroy(b.gameObject);
							board[b.x, b.y] = 500;
						}
					}
				}
			}
			if (countUp+countDown>=3)
			{
				v = true;
				//Debug.Log("Vertical Match");
				for (int del = 0; del <= countDown; del++)
				{
					foreach(Blocks b in allBlocks)
					{
						if(b.x == selected.x && b.y == selected.y-del)
						{
							//Debug.Log("Block "+b.x+", "+b.y+" destroyed");
							Destroy(b.gameObject);
							board[b.x, b.y] = 500;
						}
					}
				}
				for (int del = 0; del < countUp; del++)
				{
					foreach(Blocks b in allBlocks)
					{
						if(b.x == selected.x && b.y == selected.y+del)
						{
							//Debug.Log("Block "+b.x+", "+b.y+" destroyed");
							Destroy(b.gameObject);
							board[b.x, b.y] = 500;
						}
					}
				}
			}
			if(h || v)
			{
				if(Playing && !refreshing)
				{
					if ((countLeft+countRight)>=bomb.activationNumber || (countUp+countDown)>=bomb.activationNumber
					    || (countDown+countUp+countLeft+countRight)>bomb.activationNumber)
					{
						bomb.setActive();
						//bombText.gameObject.SetActive(true);
						bombText.Play("BombTextOpen");

					}
				}
				if (h && v)
				{
					bonus = (countLeft+countRight+countDown+countUp)-3;
				}
				else if (v)
				{
					bonus = (countUp+countDown)-3;
				}
				else if (h)
				{
					bonus = (countLeft+countRight)-3;
				}
			}
			
			Respawn ();
			return true;
		}
		return false;
	}

	// Use this for initialization
	void Start () {
		sound = GetComponent<AudioSource>();
		board = new int[10,10];
		NewBoard();
		CheckBoardForMatches();
		bomb.deactivate();
		score = 0;
		highScoreScreenText.text = "High Score: "+PlayerPrefsPro.GetInt("HighScore");
		scoreText.text = "Score "+score;
		statusText.text = "Select an object";
		countDownTimer = timeLimit;
		treasure = false;
		Playing = true;

	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		waitTimer += Time.deltaTime;

		if(treasure == true)
		{

			Treasure chest = Blocks.select.GetComponent<Treasure>();
			if (chest.action == 1)
				GoodTreasure();
			if (chest.action == 2)
				MehTreasure();
			if (chest.action == 3)
				BadTreasure();
			chest.PlaySounds();
			chest.OpenChest();

			Blocks.select = null;
			Blocks.moveTo = null;
				

			treasure = false;
		}

		if (score >= refreshCost && countDownTimer>0)
		{
			refreshButton.interactable = true;
		}

		if (score < refreshCost)
		{
			refreshButton.interactable = false;
		}

		if (Blocks.select && Playing)
		{
			statusText.text = "Swap with which object?";
		}
		if (Blocks.select && Blocks.moveTo && Playing)
		{

			if (CheckIfNear() == true)
			{
				//Debug.Log("blocks are near");

				SwitchBlocks();

				if(CheckForMatch(Blocks.select.gameObject.GetComponent<Blocks>())==true)
				{
					checkTags(Blocks.select);
					Blocks selected = Blocks.select.gameObject.GetComponent<Blocks>();
					score += selected.GetScore()+(bonus*(selected.GetScore()/2));
					scoreText.text = "Score "+score;
					//Debug.Log("Match");
					if(CheckForMatch(Blocks.moveTo.gameObject.GetComponent<Blocks>())==true)
					{
						checkTags(Blocks.moveTo);
						score += selected.GetScore()+(bonus*(selected.GetScore()/2));
						scoreText.text = "Score "+score;
						//Debug.Log("Match");
					}
					sound.PlayOneShot(matchSound, matchVol);
					Blocks.select = null;
					Blocks.moveTo = null;
					bonus = 0;
//					
				}
				else if(CheckForMatch(Blocks.moveTo.gameObject.GetComponent<Blocks>())==true)
				{
					checkTags(Blocks.moveTo);
					Blocks selected = Blocks.moveTo.gameObject.GetComponent<Blocks>();
					score += selected.GetScore()+(bonus*(selected.GetScore()/2));
					scoreText.text = "Score "+score;
					sound.PlayOneShot(matchSound, matchVol);
					Blocks.select = null;
					Blocks.moveTo = null;
					bonus = 0;
					//					
				}


				else 
				{
					//Debug.Log("No Match");
					SwitchBlocks();
					Blocks.select = null;
					Blocks.moveTo = null;
				}

			}
			else 
			{

				Blocks.select = null;
				Blocks.moveTo = null;
			}

			statusText.text = "Select an object";
		}
		if (Input.GetMouseButtonDown(1))
		{
			Blocks.select = null;
			Blocks.moveTo = null;
			statusText.text = "Select an object";
		}
		if (timer >= 1f)
		{
			countDownTimer -= 1;
			timerText.text = "Time: "+ countDownTimer;
			timer = 0;
			bonusScore = 0;
			if (!bombUsed)
			{
				bonusScore += noBombBonus;
				bonusScoreText.text = "No Bombs Used: " + noBombBonus;
			}
			if (bonusScore == 0)
			{
				bonusScoreText.text = "No Bonus Awarded";
			}

			if (countDownTimer <= 0)
			{
				score += bonusScore;
				refreshButton.interactable = false;
				gameOverSlide.gameObject.SetActive(true);
				gameOverSlide.SetTrigger("GameOver");
				sound.PlayOneShot(gameOverSound, gameOverVol);
				if (score >= PlayerPrefsPro.GetInt("HighScore"))
				{
					PlayerPrefsPro.SetInt("HighScore",score);
					highScoreText.text = "New High Score";
					sound.PlayOneShot(highScoreSound, highScoreVol);
				}
				if (score < PlayerPrefsPro.GetInt("HighScore"))
				{
					highScoreText.text = "High Score: " + PlayerPrefsPro.GetInt("HighScore");
				}
				FinalScoreText.text = "Final Score: "+score;
				Playing = false;
				Time.timeScale = 0;
			}
		}
		if (bombTrigger)
		{
			statusText.text = "Place the Bomb";
			if (Blocks.moveTo)
			{
				ExplodeBomb();
				sound.PlayOneShot(bombexplode);
				score += (Blocks.moveTo.GetComponent<Blocks>().GetScore()) * 2;
				scoreText.text = "Score "+score;
				Blocks.moveTo = null;
				bombTrigger = false;
				bomb.deactivate();
				bombText.Play("BombTextClose");
				bombUsed = true;
			}
		}

		if (refreshing)
		{

			Debug.Log("waiting for "+waitTimer);
			if (waitTimer >= 1)
			{
				Debug.Log("Refresh Ready");
				refreshing = false;
			}
		}
	}
}

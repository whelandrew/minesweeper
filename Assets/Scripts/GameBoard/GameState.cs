using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour 
{
	public enum GameStates{active ,gameOVer, frozen};
	Grid grid;

	public int totalMines;
	public int totalNodes;
	public int totalFlags;
	public int nodesRevealed;

	public TextMesh endText;

	void Start()
	{
		grid = GetComponent<Grid> ();
		endText.renderer.enabled = false;
	}

	void Update()
	{
		if (nodesRevealed == (totalNodes-totalMines))
			FinishGame ();
	}

	void FinishGame()
	{
		endText.renderer.enabled = true;
		endText.text = "YOU WIN!";
		endText.color = Color.cyan;

		grid.state = GameStates.frozen;
	}

	void OnGUI()
	{
		if(grid.state == GameStates.frozen)
		{
			if(GUI.Button(new Rect(0,Screen.height-50,50,50),"QUIT"))
				Application.Quit();
		}

		if(GUI.Button(new Rect(50,Screen.height-50,100,50),"NEW GAME"))
		{
			Reset ();
		}
	}

	void Reset()
	{
		grid.Reset();

		endText.renderer.enabled = false;
		totalFlags=0;
		nodesRevealed=0;
	}
}

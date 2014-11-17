using UnityEngine;
using System.Collections;

public class NodeActions : MonoBehaviour 
{
	public Material mIdle;
	public Material mHover;
	public Material mUncovered;

	public GameObject flag;
	public GameState game;
	public Grid grid;
	NodeData node;

	public TextMesh endText;

	void Start()
	{
		flag.renderer.enabled = false;
		node = GetComponent<NodeData> ();
	}

	void OnMouseOver()
	{
		if(grid.state==GameState.GameStates.active)
		{
			if(node.state!=NodeData.nodeState.uncovered)
				renderer.material = mHover;

			if(node.state==NodeData.nodeState.idle || node.state==NodeData.nodeState.flagged)
			{
				if(Input.GetMouseButtonDown(1))
				{
					SetFlag();
				}
			}

			if(node.state==NodeData.nodeState.idle)
			{
				if(Input.GetMouseButtonDown(0))
				{
					ShowNode();
				}
			}
		}
	}

	void OnMouseExit()
	{
		if(node.state==NodeData.nodeState.idle || node.state==NodeData.nodeState.flagged)
			if(grid.state==GameState.GameStates.active)
				renderer.material = mIdle;
	}

	void SetFlag()
	{
		if(node.state==NodeData.nodeState.idle)
		{
			node.state=NodeData.nodeState.flagged;
			flag.renderer.enabled=true;
		}
		else if(node.state==NodeData.nodeState.flagged)
		{
			node.state=NodeData.nodeState.idle;
			flag.renderer.enabled=false;
		}
	}

	public void ShowNode()
	{
		if(node.isMine)
		{
			Explode ();
		}
		else
		{
			game.nodesRevealed++;
			node.state=NodeData.nodeState.uncovered;
			if(node.totalMines>0)
				node.text.renderer.enabled=true;

			node.renderer.material=mUncovered;

			flag.renderer.enabled=false;

			if(node.totalMines==0)
				UncoverAdjacentNodes();
		}
	}

	public void Explode()
	{
		renderer.material.color = Color.red;
		node.state = NodeData.nodeState.detonated;
		grid.state = GameState.GameStates.gameOVer;

		endText.renderer.enabled = true;
		endText.text = "GAME OVER";
		endText.color = Color.red;
	}

	void UncoverAdjacentNodes()
	{
		foreach(NodeData n in node.neighbors)
		{
			if(n.state==NodeData.nodeState.idle)
			{
				if(!n.isMine)
				{
					n.actions.ShowNode();
				}
			}
		}
	}

	public void Reset()
	{
		renderer.material = mIdle;
		flag.renderer.enabled=false;
	}
}

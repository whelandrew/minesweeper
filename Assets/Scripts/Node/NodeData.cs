/* Andrew Whelan 11/7/2014 11:30
 * Minesweeper Game
 * Individual data for each Node
 */ 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeData : MonoBehaviour 
{
	public enum nodeState{idle,uncovered,flagged,detonated};

	public int ID;

	public Transform xForm;
	public TextMesh text;

	public bool isActive=false;
	public bool isMine=false;

	public int totalMines;
	public List<NodeData> neighbors = new List<NodeData> ();

	public nodeState state;
	public NodeActions actions;
	public GameObject linesCache;
	LineRenderer[] lines;

	void Awake()
	{
		xForm=GetComponent<Transform> ();
		text = GetComponentInChildren<TextMesh> ();
		actions = GetComponent<NodeActions> ();
		lines = linesCache.GetComponentsInChildren<LineRenderer> ();

		Reset ();
	}

	public void CountMines(List<NodeData> _neighbors)
	{
		neighbors = _neighbors;
		for(int i=0;i<neighbors.Count;i++)
		{
			lines[i].SetPosition(0,xForm.position);
			lines[i].SetPosition(1,neighbors[i].xForm.position);
			lines[i].renderer.enabled=true;

			if(neighbors[i].isMine)
			{
				totalMines++;
			}
		}
		if(totalMines>0 && !isMine)
		{
			text.text = totalMines.ToString ();
		}
	}

	public void Reset()
	{
		xForm.transform.position = new Vector3 (0, 0, 100);

		ID = 0;

		text.renderer.enabled = false;
		isActive = false;
		isMine = false;

		state = nodeState.idle;

		totalMines = 0;

		foreach(LineRenderer l in lines)
		{
			l.renderer.enabled=false;
			l.SetColors(Color.black,Color.gray);
		}

		actions.Reset ();
	}
}

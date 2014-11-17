/* Andrew Whelan 11/7/2014 11:30
 * Minesweeper Game
 * Handles the distribution of mines (nodes) on the games grid
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Grid : MonoBehaviour 
{
	public GameState.GameStates state;
	GameState game;

	// By defining a collection of prefabs already loaded into the scene
	// I can quickly build a scene without Instatiating and reduce run-time
	// memory alloc
	public GameObject nodeCahe; 
	
	public int numberOfNodes;
	public int nodesPerRow;
	public float nodeDistance;
	public int mineCount;
	
	Transform xForm;

	List<NodeData> assignedNodes=new List<NodeData>();

	void Awake()
	{
		xForm = GetComponent<Transform> ();
		state = GameState.GameStates.active;
	}
	
	void Start()
	{
		game = GetComponent<GameState> ();
		game.totalMines = mineCount;
		game.totalNodes = numberOfNodes;

		Reset ();
	}

	void Update()
	{
		if(state==GameState.GameStates.gameOVer)
		{
			for(int i=0;i<assignedNodes.Count;i++)
			{
				if(assignedNodes[i].isMine)
					assignedNodes[i].renderer.material.color=Color.red;
			}

			state=GameState.GameStates.frozen;
		}
	}

	void AssignMines()
	{
		for(int i=0;i<mineCount;i++)
		{
			int ran=UnityEngine.Random.Range(0,assignedNodes.Count);
			if(!assignedNodes[ran].isMine)
			{
				assignedNodes[ran].isMine=true;
			}
			else
				i--;
		}
	}

	void AssignNeighbors()
	{
		foreach(NodeData curr in assignedNodes)
		{
			List<NodeData> n=new List<NodeData>();

			//Upper
			var id=curr.ID + nodesPerRow;
			if(inBounds(id))          
			{
				n.Add(assignedNodes[id]);
			}			

			//Lower
			id=curr.ID - nodesPerRow;
			if(inBounds(id)) 
			{
				n.Add(assignedNodes[id]);
			}

			//Left
			id=curr.ID - 1;
			if(inBounds(id) && curr.ID % nodesPerRow != 0)  
			{
				n.Add(assignedNodes[id]);
			}

			//Right
			id=curr.ID + 1;
			if(inBounds(id) && (id % nodesPerRow) != 0)  
			{
				n.Add(assignedNodes[id]);
			}

			//UpperRight
			id=curr.ID + nodesPerRow + 1;
			if(inBounds(id) && ((curr.ID+1) % nodesPerRow) != 0) 
			{
				n.Add(assignedNodes[id]);
			}

			//UpperLeft
			id=curr.ID + nodesPerRow - 1;
			if(inBounds(id) && (curr.ID % nodesPerRow) != 0) 
			{
				n.Add(assignedNodes[id]);
			}

			//LowerRight
			id=curr.ID + nodesPerRow + 1;
			if(inBounds(id) && (curr.ID+1) % nodesPerRow != 0) 
			{
				n.Add(assignedNodes[id]);
			}

			//LowerLeft
			id=curr.ID - nodesPerRow - 1;
			if(inBounds(id) && curr.ID % nodesPerRow != 0) 
			{
				n.Add(assignedNodes[id]);
			}

			curr.CountMines(n);
		}
	}

	bool inBounds(int targetID)
	{
		if(targetID<0 || targetID>=assignedNodes.Count)
			return false;
		return true;
	}

	Vector3 GetNewPosition()
	{
		var newP=Vector3.zero;
		var ok=false;
		while(!ok)
		{
			int x=UnityEngine.Random.Range(((int)-cubeSize.x/2)+1,((int)cubeSize.x/2)-1);
			int z=UnityEngine.Random.Range(0,(int)cubeSize.z);

			newP=new Vector3(x+nodeDistance,0,z+nodeDistance);

			ok=true;

			foreach(NodeData n in assignedNodes)
			{
				if(n!=null && n.xForm.position==newP)
					ok=false;
			}
		}

		return newP;
	}

	void PlaceNodes()
	{
		var xOff = 0f;
		var zOff = 0f;
		var rowID = 0;
		
		for(int i=0;i<numberOfNodes;i++)
		{
			xOff+=nodeDistance;
			
			if(i%nodesPerRow==0)
			{
				zOff+=nodeDistance;
				xOff=0;
				rowID++;
			}
			
			foreach(NodeData node in nodeCahe.GetComponentsInChildren<NodeData>())
			{
				if(!node.isActive)
				{
					var newP=new Vector3((-cubeSize.x/2+.5f)+xOff,0,xForm.position.z-.5f+zOff);
					node.xForm.position=newP;
					node.isActive=true;
					node.ID=i;
					node.name="Node_"+i;

					assignedNodes.Add(node);
					break;
				}
			}
		}

		AssignMines ();
		AssignNeighbors ();
	}

	public void Reset()
	{
		state = GameState.GameStates.active;

		foreach(NodeData n in assignedNodes)
			n.Reset();
	
		assignedNodes = new List<NodeData> ();

		PlaceNodes ();
	}

	public Vector3 cubePos;
	public Vector3 cubeSize;
	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(cubePos,cubeSize);
	}
	#endif
}

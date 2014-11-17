using UnityEngine;
using System.Collections;

public class Gameboard : MonoBehaviour 
{
	public GameObject nodeCache;
	NodeData[] assignedNodes=new NodeData[0];

	public int totalNodes;

	void Start () 
	{
		assignedNodes=new NodeData[totalNodes];

		PlaceNodes ();
	}

	void PlaceNodes()
	{
		for(int i=0;i<totalNodes;i++)
		{

			foreach(NodeData n in nodeCache.GetComponentsInChildren<NodeData>())
			{
				if(!n.isActive)
				{
					var x=UnityEngine.Random.Range(0,10);
					var z=UnityEngine.Random.Range(0,10);
					var newP=new Vector3(x,0,z);
					n.xForm.position=newP;
					n.isActive=true;

					assignedNodes[i]=n;
				}
			}
		}
	}
}

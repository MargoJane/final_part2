using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeScript : MonoBehaviour {
	public int X;
	public int Y;
	public bool cubeActive = false;
	gameController myGameController;

	// Use this for initialization
	void Start () {
		myGameController = GameObject.Find ("GameObject").GetComponent<gameController> ();
	}

	void OnMouseDown(){
		gameController.processClick (gameObject, X, Y, cubeActive);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

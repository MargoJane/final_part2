﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour {
	//turn time
	float endTime;
	float turnTime; 
	float turnChange;

	//making the grid
	int xLength;
	int yLength;
	float gridXPosition;
	float gridYPosition;
	public static GameObject[,] gridCube;
	public GameObject cubePrefab;
	Vector3 gridCubePosition;

	//making the next cube
	float nextCubePositionX;
	float nextCubePositionY;
	Vector3 nextCubePostion;
	public GameObject nextCubePrefab;
	GameObject nextCube;
	Color[] nextCubeColors = { Color.blue, Color.green, Color.red, Color.yellow, Color.magenta };
	bool isNextCubeThere;

	int blackOutX;
	int blackOutY;

	//placing the next cube
	int keyPressed;

	public static int cubeX;
	public static int cubeY;

	public static GameObject activeCube = null;

	public static float distX;
	public static float distY;

	// Use this for initialization
	void Start () {
		endTime = 60;
		turnTime = 2;
		turnChange = 2;

		xLength = 8;
		yLength = 5;
		gridXPosition = -17.5f;
		gridYPosition = 4f;

		nextCubePositionX = -17.5f;
		nextCubePositionY = 7f;
		nextCubePostion = new Vector3 (nextCubePositionX, nextCubePositionY, 0);

		gridCube = new GameObject[xLength, yLength];

		activeCube = null;


		for (int y = 0; y < yLength; y++) {
			for (int x = 0; x < xLength; x++) {
				gridCubePosition = new Vector3 (gridXPosition, gridYPosition, 0);
				gridCube [x, y] = Instantiate (cubePrefab, gridCubePosition, Quaternion.identity);
				gridCube [x, y].GetComponent<Renderer> ().material.color = Color.white;
				gridCube [x, y].GetComponent<cubeScript> ().X = x;
				gridCube [x, y].GetComponent<cubeScript> ().Y = y;
				gridXPosition += 5;
			}

			gridXPosition = -17.5f;
			gridYPosition -= 2;
		}
	}


	void blackOutLose(){
		Vector2 blackOutCoordinates =  freeCubeWholeBoard();
		if (blackOutCoordinates == new Vector2(-1,-1)) {
			blackOutLose ();
		}else{
			gridCube [(int)blackOutCoordinates.x,(int)blackOutCoordinates.y].GetComponent<Renderer> ().material.color = Color.black;
		}
	}

	void generateNextCube(){
		if (nextCube == null) {
			nextCube = Instantiate (nextCubePrefab, nextCubePostion, Quaternion.identity);
			isNextCubeThere = true;
		} else {
			isNextCubeThere = false;
		}
		nextCube.GetComponent<Renderer> ().material.color = nextCubeColors[Random.Range(0,nextCubeColors.Length)];
	}

	int findFreeCube(int row){
		List<int> freeCubes;
		freeCubes = new List<int> ();
		for (int length = 0; length < xLength; length++) {
			if (gridCube [length, row].GetComponent<Renderer> ().material.color == Color.white) {
				freeCubes.Add (length);
			}
		}
		if (freeCubes.Count == 0) {
			return -1;
		}
		int cubeIndex = Random.Range (0, freeCubes.Count);
		int column = freeCubes [cubeIndex];
		return column;
	}

	Vector2 freeCubeWholeBoard(){
		List<Vector2> freeCubes;
		freeCubes = new List<Vector2> ();
		for (int length = 0; length < xLength; length++) {
			for(int height = 0; height < yLength; height++){
				if (gridCube [length, height].GetComponent<Renderer> ().material.color == Color.white) {
					freeCubes.Add (new Vector2(length,height));
				}
			}
		}
		if (freeCubes.Count == 0) {
			return new Vector2(-1,-1);
		}
		int cubeIndex = Random.Range (0, freeCubes.Count);
		Vector2 coordinates = freeCubes [cubeIndex];
		return coordinates;
	}

	void placeNextCube(int row){
		int column = findFreeCube (row);
		if (column == -1) {
			endGame ();
		}else{
			gridCube [column, row].GetComponent<Renderer> ().material.color = nextCube.GetComponent<Renderer> ().material.color;
			Destroy (nextCube);
			nextCube = null;
		}
	}

	void keyboard(){
		keyPressed = 0;
		if (isNextCubeThere) {
			if(Input.GetKeyDown (KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1)){
				keyPressed = 1;
			}else if(Input.GetKeyDown (KeyCode.Keypad2)|| Input.GetKeyDown(KeyCode.Alpha2)){
				keyPressed = 2;
			}else if (Input.GetKeyDown (KeyCode.Keypad3)|| Input.GetKeyDown(KeyCode.Alpha3)){
				keyPressed = 3;
			}else if (Input.GetKeyDown (KeyCode.Keypad4)|| Input.GetKeyDown(KeyCode.Alpha4)){
				keyPressed = 4;
			}else if (Input.GetKeyDown (KeyCode.Keypad5)|| Input.GetKeyDown(KeyCode.Alpha5)){
				keyPressed = 5;
			}
		}
		if (nextCube != null && keyPressed != 0) {
			placeNextCube (keyPressed-1);
		}
	}

	public static void processClick(GameObject clickedCube, int x, int y, bool cubeActive){
		if(clickedCube.GetComponent<Renderer> ().material.color != Color.white && clickedCube.GetComponent<Renderer> ().material.color != Color.black){
			if (cubeActive) {
				clickedCube.GetComponent<cubeScript>().cubeActive = false;
				clickedCube.transform.localScale /= 2f;
				activeCube = null;
			} else {
				if (activeCube != null) {
					activeCube.GetComponent<cubeScript>().cubeActive = false;
					activeCube.transform.localScale /= 2f;
				}
				clickedCube.GetComponent<cubeScript> ().cubeActive = true;
				clickedCube.transform.localScale *= 2f;
				activeCube = clickedCube;
			}
		}
		else if (clickedCube.GetComponent<Renderer> ().material.color == Color.white) {
			distX = clickedCube.GetComponent<cubeScript> ().X - activeCube.GetComponent<cubeScript> ().X;
			distY = clickedCube.GetComponent<cubeScript> ().Y - activeCube.GetComponent<cubeScript> ().Y;
			if (Mathf.Abs (distX) <= 1 && Mathf.Abs(distY) <= 1) {
				clickedCube.GetComponent<Renderer> ().material.color = activeCube.GetComponent<Renderer> ().material.color;
				clickedCube.transform.localScale *= 2f;
				clickedCube.GetComponent<cubeScript> ().cubeActive = true;
				activeCube.GetComponent<Renderer> ().material.color = Color.white;
				activeCube.transform.localScale /= 2f;
				activeCube = clickedCube;
			}
		}
	}

	void findSameColorPlus(){
		
	}

	void findDifferentColorPlus(){
		
	}
		
	void blackOutPoints(){
		
	}

	void endGame(){
		
	}

	// Update is called once per frame
	void Update () {
		findSameColorPlus ();
		findDifferentColorPlus ();
		keyboard();
		if (Time.time > turnChange) {
			generateNextCube ();
			turnChange += turnTime;
			if (isNextCubeThere == false) {
				blackOutLose ();
				isNextCubeThere = true;
			}
		}
	}
}

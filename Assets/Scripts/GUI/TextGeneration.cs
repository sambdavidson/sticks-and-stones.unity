using UnityEngine;
using System.Collections;

public class TextGeneration : MonoBehaviour {

	public GameObject letterObject;
	public string stringToDraw;
	
	private float currentXIndent;
	private float currentYIndent;
	private char[] strCharArray;
	private bool hasCreatedObjects;

	// Use this for initialization
	void Start () {
		stringToDraw = stringToDraw.ToLower();
		stringToDraw = stringToDraw.Replace("^", System.Environment.NewLine);
		strCharArray = stringToDraw.ToCharArray();
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.O)){
			createLetterObjects();
		}

	
	}
	private void createLetterObjects() {
		foreach(char curChar in strCharArray) {
			GameObject currentLetterObj = (GameObject) Instantiate(letterObject);
			currentLetterObj.transform.parent = this.transform;
			currentLetterObj.transform.localPosition = new Vector3(currentXIndent,currentYIndent,0.0f);
			currentXIndent += 0.35f;
			int charToInt = 0;
			print(curChar);
			switch(curChar) {
				case ' ':
					charToInt = 0;
					break;
				case 'a':
					charToInt = 1;
					break;
				case 'b':
					charToInt = 2;
					break;
				case 'c':
					charToInt = 3;
					break;
				case 'd':
					charToInt = 4;
					break;
				case 'e':
					charToInt = 5;
					break;
				case 'f':
					charToInt = 6;
					break;
				case 'g':
					charToInt = 7;
					break;
				case 'h':
					charToInt = 8;
					break;
				case 'i':
					charToInt = 9;
					break;
				case 'j':
					charToInt = 10;
					break;
				case 'k':
					charToInt = 11;
					break;
				case 'l':
					charToInt = 12;
					break;
				case 'm':
					charToInt = 13;
					break;
				case 'n':
					charToInt = 14;
					break;
				case 'o':
					charToInt = 15;
					break;
				case 'p':
					charToInt = 16;
					break;
				case 'q':
					charToInt = 17;
					break;
				case 'r':
					charToInt = 18;
					break;
				case 's':
					charToInt = 19;
					break;
				case 't':
					charToInt = 20;
					break;
				case 'u':
					charToInt = 21;
					break;
				case 'v':
					charToInt = 22;
					break;
				case 'w':
					charToInt = 23;
					break;
				case 'x':
					charToInt = 24;
					break;
				case 'y':
					charToInt = 25;
					break;
				case 'z':
					charToInt = 26;
					break;
				case '.':
					charToInt = 27;
					break;
				case '?':
					charToInt = 28;
					break;
				case '!':
					charToInt = 29;
					break;
				case '\'':
					charToInt = 29;
					break;
				case '\n':
					
					currentYIndent -= 0.5f;
					currentXIndent = 0.0f;
					break;
			
			}
			currentLetterObj.GetComponent<Animator>().SetInteger("Letter", charToInt);
		}
	}
}

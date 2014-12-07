using System;
using UnityEngine;
using System.Collections;

public class ControllerScript : MonoBehaviour {
	
	// 0 - Paused; 1 - Running; 2 - Dialogue;
	private int gameState;
	
	public AudioClip GetHim;
	public AudioClip Winner;
	public bool isDebug = false;
	
	private GameObject player;
	private GameObject[] NPCs;
	
	private GameObject InteractableObj;
	private bool canInteract;

    private bool isInCutscene;

	
	//GUI Stuff
	private GameObject largeWords;
	private float largeWordsTimer;

	// Use this for initialization
	void Start () {
	
		gameState = 1;
		
		player = GameObject.FindGameObjectWithTag("Player");
		NPCs = GameObject.FindGameObjectsWithTag("NPC");
		largeWords = GameObject.Find("LargeWords");
		
		UnPauseGameplay();
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown(KeyCode.Escape)){
			if(gameState == 1)
				PauseGameplay();
			else if(gameState == 0)
				UnPauseGameplay();
		}
		
		//Find if player is close enough to talk.
		InteractableObj = null;
		canInteract = false;
		foreach(GameObject npc in NPCs) {
			if(Vector3.Distance(npc.transform.position,player.transform.position) < 1.0f) {
				if(npc.GetComponent<ChildBehavior>().aiType == ChildBehavior.AiType.Waving) {
					InteractableObj = npc;
					canInteract = true;
				}
			}
		}
		//Check if we want to and can interact
		if(canInteract) {
			if(Input.GetKeyDown(KeyCode.Space)){
				if(InteractableObj.tag == "NPC") {
					InteractableObj.GetComponent<ChildBehavior>().ChangeState(ChildBehavior.AiType.Running);
					InteractableObj.GetComponent<ChildBehavior>().speechBubble.GetComponent<Animator>().SetBool("isOpen",false);
					InteractableObj = null;
					canInteract = false;
					TriggerCutscene(Cutscene.FirstKid);
				}
			}
		}
		
		//Cutscene stuff
		if(isInCutscene) {
			cutsceneTimer += Time.deltaTime;
			if(cutsceneTimer >= cutsceneLength) {
				isInCutscene = false;
				UnPauseGameplay();
				if(cutsceneID == Cutscene.FirstKid) {
					largeWords.GetComponent<SpriteRenderer>().enabled = true;
					largeWordsTimer = 2.0f;
					AudioSource.PlayClipAtPoint(GetHim,player.transform.position);
				}
			}
		} else if(largeWordsTimer > 0.0f) {
			largeWordsTimer -= Time.deltaTime;
			if(largeWordsTimer <= 0.0f) {
				largeWordsTimer = 0.0f;
				largeWords.GetComponent<SpriteRenderer>().enabled = false;
			}
		}
		
		
	
	}
	public void TriggerCutscene(Cutscene sceneToTrigger) {

		//First time with the first kid
		if(sceneToTrigger == Cutscene.FirstKid) {
			isInCutscene = true;
			cutsceneTimer = 0.0f;
			cutsceneLength = 3.0f;
			cutsceneID = sceneToTrigger;
			PauseGameplay();
		}
	}
	/// <summary>
	/// Pauses all NPCs and relevant objects and updates gamestate.
	/// </summary>
	public void PauseGameplay() {

		player.GetComponent<PlayerControl>().isActive = false;
		foreach(GameObject npc in NPCs) {
			npc.GetComponent<ChildBehavior>().isActive = false;
		}
		gameState = 0;
		if(isInCutscene) // what is the nature of the pause
			gameState = 2;
		
	}
    /// <summary>
    /// Unpauses all NPCs and relevant objects and updates the gamestate.
    /// </summary>
	public void UnPauseGameplay() {

		player.GetComponent<PlayerControl>().isActive = true;
		foreach(GameObject npc in NPCs) {
			npc.GetComponent<ChildBehavior>().isActive = true;
		}
		gameState = 1;
	}
	//Refresh the NPCs, use after one is destroyed
	public void refreshNPCs() {
		NPCs = GameObject.FindGameObjectsWithTag("NPC");
	}
	//A temporary YOU WIN function
	public void win() {
		print("win");
	}
	void OnGUI() {
		if(gameState == 0)
			GUI.TextArea(new Rect(Screen.width/2 - 50,Screen.height/2 - 25,100,50),"PAUSED");
		
		//cutscene debug
	    if (isInCutscene)
	    {
	        GUI.TextArea(new Rect(Screen.width/2 - 50, Screen.height/2 - 25, 100, 70),
	            (cutsceneLength - cutsceneTimer).ToString());

            //configure gui for cutscene black bars
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.black);
            texture.Apply();
            GUI.skin.box.normal.background = texture;
	        if (cutsceneBlackBarPercent < 1.0f)
	        {
                //Expand out the black bars
	            cutsceneBlackBarPercent = Mathf.Lerp(cutsceneBlackBarPercent, 1.0f, 0.03f);

	        }
	    }
	    else
	    {
            //Shrink  the black bars
            cutsceneBlackBarPercent = Mathf.Lerp(cutsceneBlackBarPercent, 0.0f, 0.1f);
	    }
        //Draw black bars
        GUI.Box(new Rect(0, 0, Screen.width, cutsceneBlackBarPercent * 75.0f), GUIContent.none);
        GUI.Box(new Rect(0, Screen.height - (75.0f * cutsceneBlackBarPercent), Screen.width, 100), GUIContent.none);
	}
	
	/// <summary>
	/// Editor image drawing
	/// </summary>
	void OnDrawGizmos() {
		Gizmos.DrawIcon(transform.position, "controller.png", true); //Make the controller visible in the editor.
	}
	
}
//Cutscene Enumerations
public enum Cutscene
{
    StartOfDay,
    FirstKid,
    RevisitFirstKid
}

using System;
using System.Linq;
using UnityEngine;
using System.Collections;

public class ControllerScript : MonoBehaviour {
	
	// 0 - Paused; 1 - Running; 2 - Dialogue;
	private int gameState;
	

	public AudioClip Winner;
	public bool isDebug = false;
	
	private GameObject player;
    /// <summary>
    /// List containing the NPCs that currently exist.
    /// </summary>
	private System.Collections.Generic.List<GameObject> NPCs;
	
	private GameObject InteractableObj;
	private bool canInteract;

    /// <summary>
    /// Cutscene
    /// </summary>
    private bool isInCutscene;
    private CutsceneHandler cutsceneHandler;


	// Use this for initialization
	void Start () {
	
		gameState = 1;
		
		player = GameObject.FindGameObjectWithTag("Player");
		NPCs = GameObject.FindGameObjectsWithTag("NPC").ToList();
	    cutsceneHandler = this.GetComponent<CutsceneHandler>();
	    cutsceneHandler.CutsceneFinishedCallback = CutsceneCallback;
		
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
	    if (!isInCutscene) //Dont check for all this unless we are in a cutscene.
	    {
            //Find if player is close enough to talk.
            InteractableObj = null;
            canInteract = false;
            foreach (GameObject npc in NPCs)
            {
                if (Vector3.Distance(npc.transform.position, player.transform.position) < 1.0f)
                {
                    if (npc.GetComponent<ChildBehavior>().aiType == ChildBehavior.AiType.Waving)
                    {
                        InteractableObj = npc;
                        canInteract = true;
                    }
                }
            }
            //Check if we want to and can interact
            if (canInteract)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (InteractableObj.tag == "NPC")
                    {
                        InteractableObj.GetComponent<ChildBehavior>().ChangeState(ChildBehavior.AiType.Running);
                        InteractableObj.GetComponent<ChildBehavior>().speechBubble.GetComponent<Animator>().SetBool("isOpen", false);
                        InteractableObj = null;
                        canInteract = false;
                        cutsceneHandler.TriggerCutscene(Cutscene.FirstKid);
                    }
                }
            }
	    }
	}
    /// <summary>
    /// Callback for the cutscene handler to call when the current cutscene has finished
    /// </summary>
    /// <param name="scene"></param>
    public void CutsceneCallback(Cutscene scene)
    {
        isInCutscene = false;
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
	/// <summary>
    /// Removes the NPC from the internal list of NPCs in existance.
    /// Use whenever a NPC is destroyed
	/// </summary>
	public void removeNPC(GameObject npcToDestroy)
	{
	    NPCs.Remove(npcToDestroy);
	}
    /// <summary>
    /// OnGUI method
    /// </summary>
	void OnGUI() {
		if(gameState == 0)
			GUI.TextArea(new Rect(Screen.width/2 - 50,Screen.height/2 - 25,100,50),"PAUSED");
	}
	
	/// <summary>
	/// Editor image drawing
	/// </summary>
	void OnDrawGizmos() {
		Gizmos.DrawIcon(transform.position, "controller.png", true); //Make the controller visible in the editor.
	}
	
}
/// <summary>
/// Enumerations used to identify the many cutscenes that are used.
/// </summary>
public enum Cutscene
{
    StartOfDay,
    FirstKid,
    RevisitFirstKid
}

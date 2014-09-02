using UnityEngine;
using System.Collections;

public class DeactivateSpriteRenderer : MonoBehaviour {

	private ControllerScript controller;
	// Use this for initialization
	void Start () {
	
		controller = GameObject.Find("GlobalController").GetComponent<ControllerScript>();

	
	}
	
	// Update is called once per frame
	void Update () {
		if(controller.isDebug) {
			this.GetComponent<SpriteRenderer>().enabled = true;
		} else {
			this.GetComponent<SpriteRenderer>().enabled = false;
		}
	
	}
}

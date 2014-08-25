using UnityEngine;
using System.Collections;

public class DesitnationPointScript : MonoBehaviour {

	// Use this for initialization
	
	// Update is called once per frame
	//Draw the stupid editor image
	
	void OnDrawGizmos() {
		if(this.name == "DestinationPointsContainer") {
			Gizmos.DrawIcon(transform.position, "xBW.png", true);
			return;
		}
		if(this.tag == "DPStart") {
			Gizmos.DrawIcon(transform.position, "xGreen.png", true);
			return;
		}
		Gizmos.DrawIcon(transform.position, "xRed.png", true);
	}
	

}

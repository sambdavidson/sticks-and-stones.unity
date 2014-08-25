using UnityEngine;
using System.Collections;

public class IntroBehavior : MonoBehaviour {

	public GameObject freshFourLess;
	public GameObject school;
	public GameObject pressSpace;

	private float introTimer;
	private int introPart;
	private bool readyToPlay;

	// Use this for initialization
	void Start () {

		introPart = 0;
		introTimer = 0.0f;
		freshFourLess.GetComponent<SpriteRenderer> ().color = new Color(1f,1f,1f,0.0f);
		school.GetComponent<SpriteRenderer> ().color = new Color(1f,1f,1f,0.0f);
		
		pressSpace.GetComponent<SpriteRenderer>().enabled = false;
		//Move the School out of sight
		school.transform.position = new Vector3 (10f, 10f,0f);
		//Center Fresh4Less
		freshFourLess.transform.position = Vector3.zero;
		
		readyToPlay = false;
	}
	
	// Update is called once per frame
	void Update () {

		introTimer += Time.deltaTime;

		//Test if the intro is still going
		if (introPart < 2) {

			//Dont Start Until 2 Seconds in
			if (introTimer > 2.0f) 

					//Fade in Fresh4Less
					if (introTimer < 3.0f)
						freshFourLess.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, introTimer - 2.0f);

					if (introTimer > 5.0f) {

						if (introTimer < 6.0f)
							freshFourLess.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 6.0f - introTimer);
							
							//Start with the school
							if (introTimer > 8.0f) {

									//Reposition the Intro Cards
									if (introPart == 0) {

										//get Fresh4Less outa here!
										freshFourLess.transform.position = new Vector3 (10f, 10f, 0f);
											//Its School's Turn
										school.transform.position = Vector3.zero;

										introPart = 1;
										
									}
									if(introTimer < 9.0f)
										school.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, introTimer - 8.0f);
										
									if(introTimer > 9.0f && introPart < 2) {
										TriggerIntroObjects();
										introPart = 2;
									}
									
										
							}

					}

			
		}
		if(introPart == 2 && introTimer > 14.5f) {
		
			pressSpace.GetComponent<SpriteRenderer>().enabled = true;
			readyToPlay = true;
			introPart = 3;
		
		}
		//Continue
		
		if(Input.GetKeyDown (KeyCode.Space)) {
			if(readyToPlay) {
				Application.LoadLevel ("Playground");
			} else {
				freshFourLess.transform.position = new Vector3 (10f, 10f, 0f);
				school.transform.position = Vector3.zero;
				school.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);
				pressSpace.GetComponent<SpriteRenderer>().enabled = true;
				readyToPlay = true;
				
				skipIntroObjects();
			}
		}
		
		
	}
	void OnGUI() {
		if(introPart == 4) {
			GUI.TextArea( new Rect( 300,200,100,200),"Game Started");
		}
	}
	private void TriggerIntroObjects() {
	
		foreach(GameObject triggerable in GameObject.FindGameObjectsWithTag("IntroTriggerable")) {
		
			triggerable.GetComponent<IntroTrigger>().Trigger();
		}
		
	}
	
	private void skipIntroObjects() {
			
		foreach(GameObject triggerable in GameObject.FindGameObjectsWithTag("IntroTriggerable")) {
				
			triggerable.GetComponent<IntroTrigger>().Skip();
		}
			
	}
	
}

using UnityEngine;
using System.Collections;

public class IntroStickScript : MonoBehaviour {

	private Vector3 startingPosition;
	
	private Vector3 outsidePosition;
	private Quaternion outsideRotation;
	private float travelLength;
	private float speed;
	private float startTime;
	
	private int introState;

	// Use this for initialization
	void Start () {
		
		introState = 0;
		
		//Get our starting position then move us out of screen in a random direction.
		startingPosition = this.transform.position;

		float randomVal = Random.value * 2 * Mathf.PI;
		
		this.transform.Translate(new Vector3(Mathf.Cos(randomVal), Mathf.Sin(randomVal), 0f) * 20);
		
		outsidePosition = this.transform.position;
		
		speed = 10.0f + (Random.value * 6f); 
		
		travelLength = Vector3.Distance(outsidePosition,startingPosition);
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(introState == 1) {
			
			
			float distanceTraveled = (Time.time - startTime) * speed;
			float percentageTraveled = distanceTraveled / travelLength;
			float lerpPercentage = (1 / (1 + Mathf.Exp(-1 * percentageTraveled * 10)));
			
			this.transform.position = Vector3.Lerp(outsidePosition,startingPosition,lerpPercentage);

			if(percentageTraveled > 1f) {
				introState = 2;
			}
		}
		
	
	}
	
	public void Trigger() {
		if(introState == 0) { 
	 		introState = 1;
	 		startTime = Time.time;
	 	}
	}
	public void Skip() {
		
		transform.position = startingPosition;
		introState = 3;
	
	}
}

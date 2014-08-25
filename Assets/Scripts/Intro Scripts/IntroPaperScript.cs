using UnityEngine;
using System.Collections;

public class IntroPaperScript : MonoBehaviour {

	public GameObject words;
	
	private Vector3 startingPosition;
	private Vector3 outsidePosition;
	private float travelLength;
	private float speed = 10f;
	private float startTime;
	
	private float introState;

	// Use this for initialization
	void Start () {
	
		words.transform.parent = this.transform;
		
		startingPosition = this.transform.position;
		this.transform.Translate(new Vector3(10f,0f,0f));
		outsidePosition = this.transform.position;
		
		travelLength = Vector3.Distance(outsidePosition,startingPosition);
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(introState == 2) {
			
			
			float distanceTraveled = (Time.time - startTime) * speed;
			float percentageTraveled = distanceTraveled / travelLength;
			float lerpPercentage = (1 / (1 + Mathf.Exp(-1 * percentageTraveled * 10)));
			
			this.transform.position = Vector3.Lerp(outsidePosition,startingPosition,lerpPercentage);
			
			if(percentageTraveled > 1f) {
				introState = 3;
			}
		}
		if(introState == 1 && Time.time - startTime > .7f) {
			introState = 2;
			startTime = Time.time;
		}
	
	}
	
	public void Trigger() {
		
		if(introState == 0) { 
			introState = 1;
			startTime = Time.time;
			this.GetComponent<AudioSource>().PlayDelayed (0.5f);
		}
		
	}
	public void Skip() {
		
		transform.position = startingPosition;
		introState = 3;
		
	}
}

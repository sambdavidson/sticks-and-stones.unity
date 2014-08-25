using UnityEngine;
using System.Collections;

public class IntroStoneScript : MonoBehaviour {

	public bool isTheO;

	private Vector3 startingPosition;
	private Vector3 outsidePosition;
	
	private Quaternion outsideRotation;
	
	private float travelLength;
	private float speed;
	private float startTime;
	
	private int introState;
	
	private bool triggeredPain = false;
	

	// Use this for initialization
	void Start () {
	
		introState = 0;
		
		//Get our starting position then move us out of screen in a random direction.
		startingPosition = this.transform.position;
		

		if(isTheO)
			this.transform.Translate(new Vector3(-10f, 0f, 0f));	
		else {
			float randomVal = Random.value * 2 * Mathf.PI;
			this.transform.Translate(new Vector3(Mathf.Cos(randomVal), Mathf.Sin(randomVal), 0f) * 20);
		}
		outsidePosition = this.transform.position;
		
		speed = 10.0f + (Random.value * 6f); 
		
		travelLength = Vector3.Distance(outsidePosition,startingPosition);
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(introState == 2 && !isTheO) {
	
			float distanceTraveled = (Time.time - startTime) * speed;
			float percentageTraveled = distanceTraveled / travelLength;
			float lerpPercentage = (1 / (1 + Mathf.Exp(-1 * percentageTraveled * 10)));
			
			this.transform.position = Vector3.Lerp(outsidePosition,startingPosition,lerpPercentage);
			
			if(percentageTraveled > 1f) {
				introState = 3;
			}
		} else if(introState == 2 && isTheO) {
		
			GameObject loserHead = GameObject.Find("LoserHead");
			float xAxis = (Time.time - startTime) * 7f;
			if(xAxis + outsidePosition.x < loserHead.transform.position.x) {
				this.transform.position = new Vector3(outsidePosition.x + xAxis, Mathf.Sin((xAxis * Mathf.PI) / ( loserHead.transform.position.x - outsidePosition.x )) * 5f, 0f);
			} else if(xAxis + outsidePosition.x < startingPosition.x){
				if(!triggeredPain) {
					
					triggeredPain = true;
					loserHead.GetComponent<IntroHeadScript>().ShowPain(1.5f);
					this.GetComponent<AudioSource>().Play();
					
				}
				this.transform.position = new Vector3(outsidePosition.x + xAxis, (Mathf.Sin(((xAxis * Mathf.PI) / ( loserHead.transform.position.x - outsidePosition.x )) + 3.0f ) * 3f) - (((xAxis - (loserHead.transform.position.x - outsidePosition.x))/(startingPosition.x - loserHead.transform.position.x)) * 1.1f) , 0f);
			}
			
		} else if(introState == 1 && Time.time - startTime > 1f) {
			if(!isTheO) {
				introState = 2;
				startTime = Time.time;
			} else if(isTheO && Time.time - startTime > 4f) {
				introState = 2;
				startTime = Time.time;
			}
		}
	
	}
	public void Trigger() {
	
		if(introState == 0) { 
			introState = 1;
			startTime = Time.time;
			if(isTheO) {
				GameObject.Find("Stones").GetComponent<AudioSource>().PlayDelayed(1.2f);
				GameObject.Find("Sticks").GetComponent<AudioSource>().PlayDelayed(0f);
			}
		}
		
	}
	public void Skip() {
		
		transform.position = startingPosition;
		introState = 3;
		
	}
}

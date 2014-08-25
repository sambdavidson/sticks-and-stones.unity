using UnityEngine;
using System.Collections;

public class IntroHeadScript : MonoBehaviour {

	public Sprite neutralExpression;
	public Sprite angryExpression;
	
	private Vector3 startingPosition;
	private Vector3 outsidePosition;
	private float travelLength;
	private float speed = 18f;
	private float startTime;
	
	private float rotateStartTime;
	
	// 0 - Neutral; 1 - Pain;
	private int facialExpression;
	private float expressionTimer;
	
	// 0 - Beginning; 1 - Waiting; 2 - Falling; 3 - Bouncing;
	private int introState;
	private float introTimer;
	
	private SpriteRenderer ourSpriteRenderer;

	// Use this for initialization
	void Start () {
	
		ourSpriteRenderer = this.GetComponent<SpriteRenderer>();
	
		facialExpression = 0;
		
		startingPosition = this.transform.position;
		this.transform.Translate(new Vector3(0f, 10f, 0f));
		outsidePosition = this.transform.position;
		travelLength = Vector3.Distance(outsidePosition,startingPosition);
		
		rotateStartTime = Time.time;
		
		introState = 0;
	
	}
	
	// Update is called once per frame
	void Update () {
	
		introTimer += Time.deltaTime;
		
		this.transform.Rotate ( new Vector3(0f,0f,Mathf.Sin((Time.time - rotateStartTime) * 1.5f * Mathf.PI)));
		
		//Facial expression resetting after duration
		if(facialExpression != 0 && Time.time > expressionTimer) {
			expressionTimer = 0;
			facialExpression = 0;
			ourSpriteRenderer.sprite = neutralExpression; 
		}
		
		//Fall and Bounce
		//Are we animating?
		if(introState > 1) {
			//Falling
			if(introState == 2) {
				float distanceTraveled = introTimer * speed;
				float percentageTraveled = distanceTraveled / travelLength;
				this.transform.position = Vector3.Lerp(outsidePosition,startingPosition,percentageTraveled);
				if(percentageTraveled >= 1f) {
					introState = 3;
					introTimer = -.5f;
				}
				
			}
			//Bouncing
			if(introState == 3) {
				float amplitude = 2f;
				float frequency = 1f;
				float decay = 1f;
				this.transform.position = new Vector3(this.transform.position.x,
												      startingPosition.y + (amplitude * (Mathf.Sin(introTimer*frequency*Mathf.PI*2)/Mathf.Exp(introTimer*decay))),
													  this.transform.position.z);
			}
			
		}
		
		//Are we done waiting?
		if(introState == 1 && introTimer > 2f) {
			introState = 2;
			introTimer  = 0;
		}
		
	
	}
	
	public void ShowPain(float duration) {
	
		ourSpriteRenderer.sprite = angryExpression;
		facialExpression = 1;
		expressionTimer = Time.time + duration;
	
	}
	public void Trigger() {
	
//		ShowPain(3f);
		introTimer = 0.0f;
		if(introState == 0)
			introState = 1;
	
	}
	public void Skip() {
		
		transform.position = startingPosition;
		introState = 4;
		
	}
}

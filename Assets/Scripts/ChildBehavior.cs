using UnityEngine;
using System.Collections;

public class ChildBehavior : MonoBehaviour {

	//Pause Var
	[HideInInspector]
	public bool isActive;
	
	//Objects and Content
	public GameObject speechBubble;
	public AudioClip[] painSounds;
	
	public enum AiType {Waving, Running, Attacking}
	public AiType aiType;
	public enum ChildID {FirstKid, SecondKid}
	public ChildID thisChildIs;
	
	private Animator animator;
	private GameObject player;
//	private AudioSource audiosource;

	//Running from player
	private Transform DPStart;
	private Transform[] RunningPoints;
	private Transform destinationPoint;
//	private Transform previousPoint;
	private bool hasReachedStart;

	// Use this for initialization
	void Start () {
	
		//Find Gameobjects and Components
		animator = this.GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player");
//		audiosource = this.GetComponent<AudioSource>();
		DPStart = GameObject.FindGameObjectWithTag("DPParent").transform.FindChild("StartingPoints").FindChild("StartingPoint1");
		RunningPoints = GameObject.FindGameObjectWithTag("DPParent").transform.FindChild("RunningPoints").GetComponentsInChildren<Transform>();
		
		speechBubble = (GameObject) GameObject.Instantiate(speechBubble);
		speechBubble.transform.position = this.transform.position;
//		speechBubble.transform.parent = this.transform;
		speechBubble.transform.Translate(new Vector3(-1f,0.55f,0f));

	}
	
	// Update is called once per frame
	void Update () {
		
		//Pause Handling
		if(!isActive) {
			if(animator.enabled) {
				animator.enabled = false;
				speechBubble.GetComponent<SpeechBubbleBehavior>().isActive = false;
			}
			return;
		} else if(!animator.enabled) {
			animator.enabled = true;
			speechBubble.GetComponent<SpeechBubbleBehavior>().isActive = true;
		}
		
		// Thanks robertbu 
		if(aiType == AiType.Waving) {
			if (Vector3.Distance(this.transform.position,player.transform.position) < 3f) {
	
				animator.SetBool("isWaving", true);
				var dir = player.transform.position - this.transform.position;
				var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
				transform.rotation = Quaternion.Lerp(this.transform.rotation,Quaternion.AngleAxis(angle, Vector3.forward),0.04f);
				speechBubble.GetComponent<Animator>().SetBool("isOpen",true);
				
			} else {
				animator.SetBool("isWaving", false);
				speechBubble.GetComponent<Animator>().SetBool("isOpen",false);
			}
		}
		//Keep the speechBubble's rotation fixed.
		speechBubble.transform.rotation = Quaternion.identity;
		
		if(aiType == AiType.Running) {
			//Look toward the destination;
			var dir = destinationPoint.position - this.transform.position;
			var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
			transform.rotation = Quaternion.Lerp(this.transform.rotation,Quaternion.AngleAxis(angle, Vector3.forward),0.04f);
		}


	}
	void FixedUpdate()
	{
		if(!isActive)
			return;

		
		// Move Us

		if(aiType == AiType.Running) {
		
			//Run to the Start if we havent
			if(!hasReachedStart) {
				//Check if we've reached the start
				if(Vector3.Distance(this.transform.position, DPStart.position) <=  0.4f) {

					hasReachedStart = true;
					float shortestDistance = 0.0f;
					//Find and assign the closest running point
					destinationPoint = RunningPoints[Random.Range(0,RunningPoints.Length)];
					shortestDistance = Vector3.Distance(this.transform.position,destinationPoint.position);
					foreach(Transform point in RunningPoints) {
						if(Vector3.Distance(this.transform.position,point.position) < shortestDistance) {
							destinationPoint = point;
							shortestDistance = Vector3.Distance(this.transform.position,point.position);
						}
						
					}
				}
			} else {
			
				if(Vector3.Distance(this.transform.position, destinationPoint.position) <=  0.4f) {
					
					destinationPoint = RunningPoints[Random.Range(0,RunningPoints.Length)];
					foreach(Transform point in RunningPoints) {
						break;
					}
				}
			}
			
			float speed = 1.0f;
			float HorVal = (destinationPoint.position.x - this.transform.position.x);
			float VerVal = (destinationPoint.position.y - this.transform.position.y);
			if(HorVal > 0.5f)
				HorVal = 5.0f;
			else if(HorVal < -0.5f)
				HorVal = -5.0f;
			if(VerVal > 0.5f)
				VerVal = 5.0f;
			else if(VerVal < -0.5f)
				VerVal = -5.0f;
			
			rigidbody2D.velocity = new Vector2(Mathf.Lerp( this.rigidbody2D.velocity.x, HorVal * speed, 0.8f), Mathf.Lerp(this.rigidbody2D.velocity.y, VerVal * speed, 0.3f));
			
		}

		
	}
	
	void OnDestroy() {
	
		if(Application.isEditor)
			return; //This is to prevent a bunch of code from being run when I un-play the game in the Editor
			
		int clipIndex = Random.Range(0,painSounds.Length);
		AudioSource.PlayClipAtPoint(painSounds[clipIndex],this.transform.position);
		Destroy(speechBubble);
		GameObject.FindGameObjectWithTag("GameController").GetComponent<ControllerScript>().refreshNPCs();
		GameObject.FindGameObjectWithTag("GameController").GetComponent<ControllerScript>().win();
		Destroy(gameObject);
	}
	public void ChangeState(AiType toType) {
	
		if(toType == AiType.Running) {
			aiType = toType;
			animator.SetBool("isWaving", false);
			speechBubble.GetComponent<Animator>().SetBool("isOpen",false);
			if(thisChildIs == ChildID.FirstKid) {
				hasReachedStart = false;
				destinationPoint = DPStart;
			}
			return;
		}
		if(toType == AiType.Waving) {
			aiType = toType;
		}
	
	}
}

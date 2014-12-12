using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	//Pause Var
	[HideInInspector]
	public bool isActive;
	public GameObject attackPrefab;
	public AudioClip[] weakAttackSound;
	
	private ControllerScript controller;
	
	//Thanks Flint Silver.
	public float baseSpeed = 3;
	public float baseSprint = 3;
	
	private float walkSpeed;
	private float curSpeed;
	
	//Sprinting Variables
	private bool isSpriting;
	private float stamina;
	private bool isRecovered;
	private float exhaustTimer;
	
	//Attacking
	private bool canAttack;
	private bool isAttacking;
	private bool isEquiped;
	
	//Mouse Position
	private Vector3 mousePosition;
	
	//Animation Controllers
	private Animator animator;
	private Animator legAnimator;
	
	//Our AudioSource
	private AudioSource audiosource;
	
	/// <summary>
    /// The object for the players legs
	/// </summary>
	private GameObject ourLegs;

    /// <summary>
    /// The controller component of the self esteem badge.
    /// Essentially the health bar.
    /// </summary>
    private SelfEsteemController selfEsteem;



	/// <summary>
    /// Use this for initialization
	/// </summary>
	void Start () {
		
		//Find Objects and Components
		controller = GameObject.Find("GlobalController").GetComponent<ControllerScript>();
		ourLegs = this.transform.FindChild("Legs").gameObject;
		animator = this.GetComponent<Animator>();
		legAnimator = ourLegs.GetComponent<Animator>();
		audiosource = this.GetComponent<AudioSource>();
        selfEsteem = GameObject.Find("SelfEsteemBadge").GetComponent<SelfEsteemController>();
		
		legAnimator.transform.localPosition = new Vector3(-0.15f,-0.1f,0.0f);
		legAnimator.transform.localScale = new Vector3(1.0f,0.8f,1.0f);
		
		//The baseSprint is our max stamina
		isSpriting = false;
		stamina = baseSprint;
		isRecovered = true;
		exhaustTimer = 0f;
		
		//sprintSpeed = walkSpeed + (walkSpeed / 2);
		walkSpeed = (float)(baseSpeed + (baseSprint / 3));
		
		//Attacking
		canAttack = true;
		isAttacking = false;
		isEquiped = false;

	}
	
	// Update is called once per frame
	void Update () {
	
		//Pause Handling
		if(!isActive) {
			if(animator.enabled) {
				animator.enabled = false;
			}
			return;
		} else if(!animator.enabled) {
			animator.enabled = true;
		}
		
		mousePosition = Input.mousePosition;           
		mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
		
		Quaternion rot = Quaternion.LookRotation(transform.position - mousePosition, Vector3.forward );
		transform.rotation = rot;  
		transform.eulerAngles = new Vector3(0, 0,transform.eulerAngles.z); 
		
		//Sprinting
		if(!isSpriting && exhaustTimer > 0)
		{
			exhaustTimer -= Time.deltaTime;
			if(exhaustTimer <= 0.0f) {
				exhaustTimer = 0.0f;
			} 
		} else if(!isSpriting && exhaustTimer == 0.0f && stamina < baseSprint) {
			stamina += Time.deltaTime;
			if(!isRecovered && stamina >= baseSprint * 0.5f) {
				isRecovered = true;
			}
			if(stamina >= baseSprint) {
				stamina = baseSprint;
			}
		}
		if(isSpriting) {
			stamina -= Time.deltaTime;
			if(stamina <= 0f) {
				isSpriting = false;
				isRecovered = false;
				exhaustTimer = 2f;
				stamina = 0.0f;
				animator.SetBool("isSprinting", false);
				legAnimator.SetBool("isSprinting", false);
				//Shrink the legs back down
				legAnimator.transform.localScale = new Vector3(1.0f,0.8f,1.0f);
			}	
		}
		if (Input.GetKey(KeyCode.LeftShift) && isRecovered) {
			isSpriting = true;
			animator.SetBool("isSprinting", true);
			legAnimator.SetBool("isSprinting", true);
			//Extend the legs
			legAnimator.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
		} else {
			isSpriting = false;
			animator.SetBool("isSprinting", false);
			legAnimator.SetBool("isSprinting", false);
			//Shrink the legs back down
			legAnimator.transform.localScale = new Vector3(1.0f,0.8f,1.0f);
		}
		
		//Attacking
		//Get State Info
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
		
		if(stateInfo.nameHash == Animator.StringToHash("Base Layer.WeakPunch")) {
			isAttacking = true;
		} else {
			isAttacking = false;	
		}
		if(canAttack){
			if(Input.GetKeyDown(KeyCode.Mouse0)){
				if(isAttacking == false) {
					isAttacking = true;
					//Create the Attack Prefab
					GameObject ourAttack = (GameObject) Instantiate(attackPrefab);
					ourAttack.transform.position = this.transform.position;
					ourAttack.transform.rotation = this.transform.rotation;
					
					if(!isEquiped){
						if(!audiosource.isPlaying)
						audiosource.clip = weakAttackSound[Random.Range(0,weakAttackSound.Length)];
						audiosource.Play();
					}

				} 
			}
			
		}
		animator.SetBool("isAttacking",isAttacking);
	
	}

	//"FixedUpdate should be used for rigidbodies"
	void FixedUpdate()
	{
		if(!isActive)
			return;
		curSpeed = walkSpeed;
		
		if(isSpriting) 
			curSpeed = curSpeed * 2.0f;
		
		// Move Us
		rigidbody2D.velocity = new Vector2(Mathf.Lerp(0, Input.GetAxis("Horizontal")* curSpeed, 0.8f),
		                                   Mathf.Lerp(0, Input.GetAxis("Vertical")* curSpeed, 0.8f));
		if(rigidbody2D.velocity.magnitude > 0.0f) {
			animator.SetBool("isRunning", true);
			legAnimator.SetBool("isRunning", true);
		}  else {
			animator.SetBool("isRunning", false);
			legAnimator.SetBool("isRunning", false);
		}

	}

    public void AddHealth(float health) 
    {
        selfEsteem.HPVal = selfEsteem.HPVal + health;
        if(health >0)
            selfEsteem.Bounce = 50f + (selfEsteem.HPVal / 2f);
    }
	void OnGUI() 
	{
		
	    if (controller.isDebug) //Draw some debug information.
	    {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
            GUIStyle ourStyle = new GUIStyle();
            ourStyle.alignment = TextAnchor.UpperLeft;
            GUI.Label(new Rect(screenPos.x, screenPos.y, 150, 150), "*Player Sprint Info*\n" + "isSprinting: " + isSpriting + "\nstamina: " + stamina + "\nisRecovered: " + isRecovered + "\nexhaustedTimer: " + exhaustTimer + "\nVelocity: " + rigidbody2D.velocity.magnitude, ourStyle);
	    }
		
	}

}

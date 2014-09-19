using UnityEngine;
using System.Collections;

public class SelfEsteemController : MonoBehaviour {

	public GameObject player;
	public ControllerScript controller;
	public Shader shaderFlashWhite;
	
	private float health = 100.0f;
    public float HPVal { get { return health; } set { health = value; if (health > 100.0) { health = 100.0f; } else if (health < 0.0f) { health = 0.0f; } } }
	
	//Shader Stuff
	private SpriteRenderer spriteRenderer;
	private float flashLength = 1.0f;
	private float flashTimer;
	private float flashAmount;
	private Color selfEsteemColor;
	
	private Vector3 localStartingPos;
	
	//Face
	private GameObject esteemFace;
	private SpriteRenderer faceSpriteRenderer;
	private Animator faceAnimator;

	
	//JUICY stuff
	private Quaternion startingRotation;
	private float juiceTimer;
	
	// Use this for initialization
	void Start () {
		localStartingPos = this.transform.localPosition;
		spriteRenderer = this.GetComponent<SpriteRenderer>();
		esteemFace = this.transform.FindChild("EsteemFaces").gameObject;
		faceSpriteRenderer = esteemFace.GetComponent<SpriteRenderer>();
		faceAnimator = esteemFace.GetComponent<Animator>();
		activate();
		
		//White Flash Shader
		spriteRenderer.material.shader = shaderFlashWhite;
		faceSpriteRenderer.material.shader = shaderFlashWhite;
		selfEsteemColor = new Color(.454f,1.0f,.070f,1.0f);
		spriteRenderer.material.color = selfEsteemColor;
		
		//Juicy
		startingRotation = this.transform.rotation;


	}
	
	// Update is called once per frame
	void Update () {
	
		//DEBUG KEYCODES
		if(Input.GetKeyDown(KeyCode.O)) {
			activate();
		}
		if(Input.GetKeyDown(KeyCode.P)) {
			deactivate();
		}
		if(Input.GetKey(KeyCode.L)) {
			if(health < 100.0f) {
				health += Time.deltaTime*30.0f;
			}
			if(health > 100.0f) {
				health =  100.0f;

			}
		}
		if(Input.GetKey(KeyCode.K)) {
			if(health > 0.0f) {
				health -= Time.deltaTime*30.0f;
			}
			if(health < 0.0f) {
				health =  0.0f;
			}
		}
		
		//Check if we are flashing
		if(flashTimer > 0.0f) {
			flashTimer -= Time.deltaTime;
			flashAmount = (flashTimer / flashLength);
			spriteRenderer.material.SetFloat("_FlashAmount",flashAmount);
			faceSpriteRenderer.material.SetFloat("_FlashAmount",flashAmount);
				
		}
		spriteRenderer.material.color = TransformHSV(selfEsteemColor, 110.0f - (health * 1.1f),1.0f,1.0f);
		//Update the Face
		switch((int)(health/10.0f)) {
			case 10:
			case 9:
			case 8:
				faceAnimator.SetInteger("Emotion", 4);
				break;
			case 7:
			case 6:
				faceAnimator.SetInteger("Emotion", 3);
				break;
			case 5:
			case 4:
				faceAnimator.SetInteger("Emotion", 2);
				break;
			case 3:
			case 2:
			case 1:
				faceAnimator.SetInteger("Emotion", 1);
				break;
			case 0:
				faceAnimator.SetInteger("Emotion", 0);
				break;	
			default:
				print("Emotion Switch Defaulted");
				break;
		}
		//JUICY rotation
		juiceTimer += Time.deltaTime;
		this.transform.Rotate(Vector3.forward, 0.05f * Mathf.Sin((3.0f * juiceTimer) + (Mathf.PI/2)));
		
	}
	// Does a fancy enable animation
	public void activate() {
		spriteRenderer.enabled = true;
		faceSpriteRenderer.enabled = true;
		flashTimer = flashLength;
		flashAmount = 1.0f;
	
	}
	//Disables the banner
	public void deactivate() {
		spriteRenderer.enabled = false;
		faceSpriteRenderer.enabled = false;
		
	}
    public void bounce(float amount)
    {

    }
	//Hue shifting code. Stolen from http://beesbuzz.biz/code/hsv_color_transforms.php
	private Color TransformHSV(
		Color color,  // color to transform
		float H,          // hue shift (in degrees)
		float S,          // saturation multiplier (scalar)
		float V           // value multiplier (scalar)
		)
	{
		float VSU = V*S*Mathf.Cos(H*Mathf.PI/180);
		float VSW = V*S*Mathf.Sin(H*Mathf.PI/180);
		
		Color ret = new Color(1.0f,1.0f,1.0f,1.0f);
		ret.r = (.299f*V+.701f*VSU+.168f*VSW)*color.r
			+ (.587f*V-.587f*VSU+.330f*VSW)*color.g
				+ (.114f*V-.114f*VSU-.497f*VSW)*color.b;
		ret.g = (.299f*V-.299f*VSU-.328f*VSW)*color.r
			+ (.587f*V+.413f*VSU+.035f*VSW)*color.g
				+ (.114f*V-.114f*VSU+.292f*VSW)*color.b;
		ret.b = (.299f*V-.3f*VSU+1.25f*VSW)*color.r
			+ (.587f*V-.588f*VSU-1.05f*VSW)*color.g
				+ (.114f*V+.886f*VSU-.203f*VSW)*color.b;
		return ret;
	}
}

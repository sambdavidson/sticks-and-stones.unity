using UnityEngine;
using System.Collections;

public class SpeechBubbleBehavior : MonoBehaviour {
	
	//Pause Var
	[HideInInspector]
	public bool isActive;
	
	public AudioClip[] PhraseSounds;
	private AudioSource audiosource;
	
	private Animator animator;
	private GameObject ourPhrase;
	private float textTimer;
	private float textTimerStart = 0.35f;

	private bool hasOpened;

	// Use this for initialization
	void Start () {
	
		isActive = true;
		
		//Find Gameobjects and Components
		animator = this.GetComponent<Animator>();
		ourPhrase = this.transform.FindChild("BubblePhrase").gameObject;
		audiosource = this.GetComponent<AudioSource>();
		
		textTimer = textTimerStart;
		ourPhrase.GetComponent<SpriteRenderer>().enabled = false;

		hasOpened = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Pause Handling
		if(!isActive) {
			if(animator.enabled) {
				animator.enabled = false;
				ourPhrase.GetComponent<Animator>().enabled = false;
			}
			return;
		} else if(!animator.enabled) {
			animator.enabled = true;
			ourPhrase.GetComponent<Animator>().enabled = true;
		}
		
	
		if(animator.GetBool("isOpen") && textTimer  > 0.0f) {
			textTimer -= Time.deltaTime;
			ourPhrase.GetComponent<Animator>().SetInteger("currentPhrase",Random.Range(0,2));
		} else if(animator.GetBool("isOpen") && textTimer <= 0.0f) {
			textTimer = 0.0f;
			ourPhrase.GetComponent<SpriteRenderer>().enabled = true;
			if(!hasOpened) {
				hasOpened = true;
				int currentPhrase = ourPhrase.GetComponent<Animator>().GetInteger("currentPhrase");
				if(currentPhrase == 0) {
					audiosource.clip = PhraseSounds[0];
					audiosource.Play();
				}
				else if(currentPhrase == 1) {
					audiosource.clip = PhraseSounds[1];
					audiosource.Play();
				}
					
			}
		} else if(!animator.GetBool("isOpen")) {
			textTimer = textTimerStart;
			ourPhrase.GetComponent<SpriteRenderer>().enabled = false;
			if(hasOpened) {
				hasOpened = false;
			}
		}
	}
	void OnDestroy() {
		Destroy (ourPhrase);
	}

}

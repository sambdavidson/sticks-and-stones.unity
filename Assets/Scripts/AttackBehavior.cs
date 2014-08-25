using UnityEngine;
using System.Collections;

public class AttackBehavior : MonoBehaviour {

	public float lifeSpan = 0.1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		lifeSpan -= Time.deltaTime;
		
		if(lifeSpan <= 0.0f)
			Destroy (gameObject);
	}
	void OnTriggerEnter2D (Collider2D col) {
		if(col.tag == "NPC") {
			if(col.gameObject.GetComponent<ChildBehavior>().aiType == ChildBehavior.AiType.Running)
				Destroy (col.gameObject);
		}
	}
}

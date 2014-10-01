using UnityEngine;
using System.Collections;

public class HealthBubbleController : MonoBehaviour {

    public float health = 10.0f;
    public float scale = 4.0f;
    public bool persistent = false;

    private float timer = 0.0f;

	// Use this for initialization
	void Start () {
	    


	}
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;

        this.transform.localScale = new Vector3(scale + Mathf.Sin(timer), scale + Mathf.Sin(timer), scale + Mathf.Sin(timer));

	}

    void OnTriggerStay2D( Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerControl>().AddHealth(health);
            if(!persistent)
                Destroy(this.gameObject);
        }
    }
}

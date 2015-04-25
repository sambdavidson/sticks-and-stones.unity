using UnityEngine;
using System.Collections;
using System.Threading;

public class CutsceneHandler : MonoBehaviour
{

    public delegate void CutsceneCallback(Cutscene finishedCutscene);

    public CutsceneCallback CutsceneFinishedCallback;
    
    public AudioClip GetHim;
    public GameObject cutscenePrefab;

    /// <summary>
    /// First Kid Cutscene
    /// </summary>
    public Sprite PlayerSpriteNeutral;
    public Sprite GreensterSpriteNeutral;


    /// <summary>
    /// Cutscene stuff
    /// </summary>
    private bool inCutscene;
    private bool cutsceneStarted = false;
    private bool skip = false;
    private Cutscene cutsceneID;
    private float cutsceneBlackBarPercent;

    /// <summary>
    /// GUI Stuff
    /// </summary>
    private GameObject largeWords;
    private float largeWordsTimer;

    private GameObject player;

    /// <summary>
    /// Controller
    /// </summary>
    private ControllerScript controller;

	// Use this for initialization
	void Start ()
	{
	    controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<ControllerScript>();
        largeWords = GameObject.Find("LargeWords");
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

        //Cutscene stuff
        if (inCutscene) //Check if we are in a cutscene
        {
            if (cutsceneStarted) //Check if the cutscene has actually begun.
            {
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    skip = true;
                }
                return;
            }
            if (cutsceneID == Cutscene.FirstKid) //Check the cutscene and start it.
            {
                cutsceneStarted = true;
                StartCoroutine(SceneFirstKid());
            }
        }
        else 
        {
            if (largeWordsTimer > 0.0f)
            {
                largeWordsTimer -= Time.deltaTime;
                if (largeWordsTimer <= 0.0f)
                {
                    largeWordsTimer = 0.0f;
                    largeWords.GetComponent<SpriteRenderer>().enabled = false;
                }
            }

        }
	
	}

    void OnGUI()
    {
        //Cutscene Prep
        if (inCutscene)
        {

            //configure gui for cutscene black bars
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.black);
            texture.Apply();
            GUI.skin.box.normal.background = texture;
            if (cutsceneBlackBarPercent < 1.0f)
            {
                //Expand out the black bars
                cutsceneBlackBarPercent = Mathf.Lerp(cutsceneBlackBarPercent, 1.0f, 0.03f);

            }
        }
        else
        {
            //Shrink the black bars
            cutsceneBlackBarPercent = Mathf.Lerp(cutsceneBlackBarPercent, 0.0f, 0.1f);
        }
        //Draw black bars
        GUI.Box(new Rect(0, 0, Screen.width, cutsceneBlackBarPercent * 75.0f), GUIContent.none);
        GUI.Box(new Rect(0, Screen.height - (75.0f * cutsceneBlackBarPercent), Screen.width, 100), GUIContent.none);
    }
    /// <summary>
    /// Triggers the specified cutscene.
    /// </summary>
    /// <param name="scene">scene to trigger</param>
    public void TriggerCutscene(Cutscene scene)
    {
        if (CutsceneFinishedCallback == null)
        {
            print("Cutscene callback not set.");
            return;
        }

        //First time with the first kid
        if (scene == Cutscene.FirstKid)
        {
            GameObject.Find("SelfEsteemBadge").GetComponent<SelfEsteemController>().deactivate();
            inCutscene = true;
            cutsceneID = scene;
            controller.PauseGameplay();
        }


    }

    private void FinishedCutscene(Cutscene scene)
    {
        GameObject.Find("SelfEsteemBadge").GetComponent<SelfEsteemController>().activate();
        inCutscene = false;
        cutsceneStarted = false;
        controller.UnPauseGameplay();
        CutsceneFinishedCallback(scene);
    }

    private IEnumerator SceneFirstKid()
    {
        // Create the player object.
        GameObject playerObject = (GameObject)Instantiate(cutscenePrefab);
        SpriteRenderer playerRenderer = playerObject.GetComponent<SpriteRenderer>();
        playerRenderer.enabled = true;
        playerRenderer.sprite = PlayerSpriteNeutral;
        Transform playerTransform = playerObject.GetComponent<Transform>();

        // Create the kid object.
        GameObject kidObject = (GameObject)Instantiate(cutscenePrefab);
        SpriteRenderer kidRenderer = kidObject.GetComponent<SpriteRenderer>();
        kidRenderer.enabled = true;
        kidRenderer.sprite = GreensterSpriteNeutral;
        Transform kidTransform = kidObject.GetComponent<Transform>();
        yield return new WaitForSeconds(1.0f);
        Vector3 kidDestination = new Vector3(Camera.main.GetComponent<Transform>().position.x + 4.0f,
            Camera.main.GetComponent<Transform>().position.y - 2.0f);
        kidTransform.position = new Vector3(kidDestination.x + 14.0f, kidDestination.y);
        Vector3 playerDestination = new Vector3(Camera.main.GetComponent<Transform>().position.x - 4.0f,
    Camera.main.GetComponent<Transform>().position.y - 2.0f);
        playerTransform.position = new Vector3(playerDestination.x - 14.0f, playerDestination.y);
        while (playerTransform.position.x < playerDestination.x - 0.01f) //Move the characters into position
        {        
            playerTransform.position = Vector3.Lerp(new Vector3(playerTransform.position.x, playerTransform.position.y), playerDestination, 0.05f);
            kidTransform.position = Vector3.Lerp(new Vector3(kidTransform.position.x, kidTransform.position.y), kidDestination, 0.1f);

            yield return new WaitForSeconds(0.016f);//60fps
        }
        yield return new WaitForSeconds(2.0f);
        largeWords.GetComponent<SpriteRenderer>().enabled = true;
        // AudioSource.PlayClipAtPoint(GetHim, player.transform.position);     
        largeWords.GetComponent<SpriteRenderer>().enabled = false;
        Destroy(playerObject);
        Destroy(kidObject);
        FinishedCutscene(Cutscene.FirstKid);
    }
}

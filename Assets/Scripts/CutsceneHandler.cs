using UnityEngine;
using System.Collections;

public class CutsceneHandler : MonoBehaviour
{

    public delegate void CutsceneCallback(Cutscene finishedCutscene);

    public CutsceneCallback CutsceneFinishedCallback;
    
    public AudioClip GetHim;

    /// <summary>
    /// Cutscene stuff
    /// </summary>
    private bool isInCutscene;

    private bool cutsceneStarted = false;
    private float cutsceneLength;
    private Cutscene cutsceneID;
    private float cutsceneBlackBarPercent;

    //GUI Stuff
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
        if (isInCutscene) //Check if we are in a cutscene
        {
            if (cutsceneStarted) //Check if the cutscene has actually begun.
            {
                return;
            }
            if (cutsceneID == Cutscene.FirstKid) //Check the cutscene and start it.
            {
                cutsceneStarted = true;
                StartCoroutine("SceneFirstKid"); //Blocking cutscene.
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
        if (isInCutscene)
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
            isInCutscene = true;
            cutsceneLength = 3.0f;
            cutsceneID = scene;
            controller.PauseGameplay();
        }


    }

    private void FinishedCutscene(Cutscene scene)
    {
        isInCutscene = false;
        controller.UnPauseGameplay();
        CutsceneFinishedCallback(scene);
    }

    private IEnumerator SceneFirstKid()
    {
        yield return new WaitForSeconds(2.0f);
        largeWords.GetComponent<SpriteRenderer>().enabled = true;
        AudioSource.PlayClipAtPoint(GetHim, player.transform.position);     
        yield return new WaitForSeconds(2.0f);
        largeWords.GetComponent<SpriteRenderer>().enabled = false;
        cutsceneStarted = false;
        FinishedCutscene(Cutscene.FirstKid);
    }
}

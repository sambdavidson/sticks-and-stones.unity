using UnityEngine;
using System.Collections;

public class CutsceneHandler : MonoBehaviour
{

    public delegate void CutsceneCallback(Cutscene finishedCutscene);

    public CutsceneCallback CutsceneFinishedCallback;

    private bool isInCutscene;
    private float cutsceneTimer;
    private float cutsceneLength;
    private Cutscene cutsceneID;
    private float cutsceneBlackBarPercent;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void BeginCutscene(Cutscene scene)
    {
        if (CutsceneFinishedCallback == null)
        {
            print("Cutscene callback not set.");
            return;
        }
        CutsceneFinishedCallback(scene);
        
    }
}

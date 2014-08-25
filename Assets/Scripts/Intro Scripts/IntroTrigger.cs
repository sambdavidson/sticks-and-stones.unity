using UnityEngine;
using System.Collections;

public class IntroTrigger : MonoBehaviour {
	
	//The entire purpose of this script is to trigger the subscript within this object since there are multiple kinds.
	
	public void Trigger() {
	
		IntroStickScript ourStickScript = this.GetComponent<IntroStickScript>();
		IntroHeadScript ourHeadScript = this.GetComponent<IntroHeadScript>();
		IntroStoneScript ourStoneScript = this.GetComponent<IntroStoneScript>();
		IntroPaperScript ourPaperScript = this.GetComponent<IntroPaperScript>();
		
		if(ourStoneScript != null) {
			ourStoneScript.Trigger();
			return;
		}
		if(ourStickScript != null) {
			ourStickScript.Trigger();
			return;
		}

		if(ourHeadScript != null) {
			ourHeadScript.Trigger();
			return;
		}
		if(ourPaperScript != null) {
			ourPaperScript.Trigger();
			return;
		}
	}
	public void Skip() {
		
		IntroStickScript ourStickScript = this.GetComponent<IntroStickScript>();
		IntroHeadScript ourHeadScript = this.GetComponent<IntroHeadScript>();
		IntroStoneScript ourStoneScript = this.GetComponent<IntroStoneScript>();
		IntroPaperScript ourPaperScript = this.GetComponent<IntroPaperScript>();
		
		if(ourStoneScript != null) {
			ourStoneScript.Skip();
			return;
		}
		if(ourStickScript != null) {
			ourStickScript.Skip();
			return;
		}
		
		if(ourHeadScript != null) {
			ourHeadScript.Skip();
			return;
		}
		if(ourPaperScript != null) {
			ourPaperScript.Skip();
			return;
		}
	}

}

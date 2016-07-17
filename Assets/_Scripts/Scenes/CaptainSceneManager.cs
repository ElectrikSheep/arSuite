using UnityEngine;
using UnityEngine.UI ;
using System.Collections;

using Vuforia ;

public class CaptainSceneManager : MonoBehaviour {

	[SerializeField] private GameObject arCommonObjetcs ;

	// Use this for initialization
	void Start () {
		
		// Register AR tracking delegates
		CaptainTrackableEventHandler.gainedTracking += localGainedTracking;
		CaptainTrackableEventHandler.lostTracking += localLostTracking;
	}
	
	private void OnDisable () {
		CaptainTrackableEventHandler.gainedTracking -= localGainedTracking;
		CaptainTrackableEventHandler.lostTracking -= localLostTracking;
	}
	
	
	
	
	void localGainedTracking () {
		arCommonObjetcs.SetActive (true);
	}
	
	void localLostTracking () {
		arCommonObjetcs.SetActive (false);
	}
}

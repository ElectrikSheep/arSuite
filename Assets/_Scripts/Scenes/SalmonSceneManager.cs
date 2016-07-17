using UnityEngine;
using UnityEngine.UI ;
using System.Collections;

using Vuforia ;
public class SalmonSceneManager : MonoBehaviour {
	
	[SerializeField] private GameObject fishSchool_AR ;
	void Awake () {
		fishSchool_AR.SetActive (false);
	}

	// Use this for initialization
	void Start () {
		
		ImageTrackerEvent.gainedTracking += localGainedTracking;
		ImageTrackerEvent.lostTracking += localLostTracking;
	}

	private void localGainedTracking () {
		fishSchool_AR.SetActive (true);
	}
	private void localLostTracking () {

	}
}

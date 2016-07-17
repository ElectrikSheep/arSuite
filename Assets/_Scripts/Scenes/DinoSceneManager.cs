using UnityEngine;
using UnityEngine.UI ;
using System.Collections;

using Vuforia ;

public class DinoSceneManager : MonoBehaviour {

	[SerializeField]
	private AudioSource audioSourceBG ;

	[SerializeField]
	private GameObject arScene ;

	private float audioCoeff = 1f ;

	// Use this for initialization
	void Start () {
		arScene.SetActive (false);
		audioSourceBG.volume = 0f;
		FishTrackablEventHandler.gainedTracking += trackingGain;
		FishTrackablEventHandler.lostTracking += trackingLost;
	}


	private void OnDestroy () {
		
		FishTrackablEventHandler.gainedTracking -= trackingGain;
		FishTrackablEventHandler.lostTracking -= trackingLost;
		StopAllCoroutines ();
		
	}


	private void trackingGain () {
		Debug.Log ("Tracking found");
		arScene.SetActive (true);

		StopCoroutine ("interpolateAudioStop");
		StartCoroutine ("interpolateAudioPlay");
	}

	private void trackingLost () {
		Debug.Log ("Tracking lost");
		arScene.SetActive (false);

		StopCoroutine ("interpolateAudioPLay");
		StartCoroutine ("interpolateAudioStop");
	}

	private IEnumerator interpolateAudioStop () {
	
		while (audioSourceBG.volume > 0f) {

			audioSourceBG.volume -= ( Time.deltaTime * audioCoeff ) ;
			yield return null ;

		}
		audioSourceBG.volume = 0f;
	}

	private IEnumerator interpolateAudioPlay () {
		
		while (audioSourceBG.volume < 1f) {
			
			audioSourceBG.volume += ( Time.deltaTime * audioCoeff ) ;
			yield return null ;
			
		}
		audioSourceBG.volume = 1f;
	}

}

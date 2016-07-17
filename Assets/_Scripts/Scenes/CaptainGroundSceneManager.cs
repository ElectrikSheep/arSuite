using UnityEngine;
using System.Collections;

using Vuforia ;

public class CaptainGroundSceneManager : MonoBehaviour {

	[SerializeField] private GameObject arCommonObjetcs ;
	[SerializeField] private ParticleSystem smokeSystem ;

	[SerializeField] private GameObject Scene ;

	[SerializeField] private float delay = 1f ;
	[SerializeField] private float scale = 5f ;

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
	
	


	private void SceneActivate ( bool withSmoke ) {
		if (withSmoke) {
			StartCoroutine("Coroutine_StartSmokeScene");
		} else {
			Scene.SetActive (true);
		}
	}


	private IEnumerator Coroutine_StartSmokeScene () {

		float time = 0.3f;
		float duration = 1f;

		Scene.transform.localScale = Vector3.zero;
		Scene.SetActive (true);
		smokeSystem.Play ();
		while (time < delay) {
			time+= Time.deltaTime ;
			yield return null ;
		}
		time = 0f;
		while (time<.6f) {
			time+= Time.deltaTime ;
			Scene.transform.localScale = scale *(time/duration)*Vector3.one ;
			
			Debug.LogWarning ( "My scale is : " + (time/duration) ) ;
			yield return null ;
		}
		Scene.transform.localScale = Vector3.one*scale;
	}

	private IEnumerator Coroutine_FadeIn () {
		
		float time = 0.3f;
		float duration = 1f;
		
		Scene.transform.localScale = Vector3.one*scale;
		Scene.SetActive (true);
		smokeSystem.Play ();
		while (time < delay) {
			time+= Time.deltaTime ;
			yield return null ;
		}
		time = 0f;
		while (time<.6f) {
			time+= Time.deltaTime ;
			Scene.transform.localScale = scale *(time/duration)*Vector3.one ;
			
			Debug.LogWarning ( "My scale is : " + (time/duration) ) ;
			yield return null ;
		}
		Scene.transform.localScale = Vector3.one*scale;

	}
	
	void localGainedTracking () {
		Debug.Log ("tracking gained");

		bool smoke = string.Equals (CaptainTrackableEventHandler.trackerName, "Ground_CaptainSmoke");
		SceneActivate (smoke);
	}
	
	void localLostTracking () {
		
		Scene.SetActive (false);
		StopAllCoroutines ();

		smokeSystem.Stop ();
		smokeSystem.time = 0;

	}
}

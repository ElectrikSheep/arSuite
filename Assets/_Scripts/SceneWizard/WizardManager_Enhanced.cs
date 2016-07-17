using UnityEngine;
using System.Collections;

// Tracking 
using Vuforia;

public class WizardManager_Enhanced : MonoBehaviour {

	[SerializeField] private GameObject[] ModelList ;

	[SerializeField]
	private ActionTrackableEventHandler WizardTracker ;

	[SerializeField]
	private Transform Scene ;

	private GameObject CurrentModel ;


	[SerializeField]
	private float factorDelay = .5f ;
	[SerializeField]
	private float factorBounce = .5f ;
	[SerializeField]
	private float factorEntrance = 1.0f;
	private float bounceDuration ;

	// Use this for initialization
	void Start () {

		WizardTracker.TrackingGained += ActionTrackingGained;
		WizardTracker.TrackingLost += ActionTrackingLost;

	}

	private void OnDestroy () {

		WizardTracker.TrackingGained += ActionTrackingGained;
		WizardTracker.TrackingLost += ActionTrackingLost;

	}

	private void ActionTrackingGained () {
		Debug.Log ("Tracking found");
		StartCoroutine ("Coroutine_EntranceAnimation");

	}
	private void ActionTrackingLost () {
		Debug.Log ("Tracking lost");
		StopAllCoroutines ();

		//EntranceEffect.Stop ();
		Scene.localScale = Vector3.zero;
	}


	private void Update () {
		if( Input.GetKeyUp(KeyCode.Space) ) {

			ActionTrackingGained() ;
			return ;

		}
		if( Input.GetKeyUp(KeyCode.B) ) {

			ActionTrackingLost() ;

		}

	}


	private void RefreshModel () {
		if (ModelList.Length == 0)
			return ;

		if( ModelList.Length == 1 && CurrentModel!= null ) {
			return ;
		}

		Destroy (CurrentModel);

		int indexToCreate = ModelList.Length == 1 ? 0 : Random.Range (0, ModelList.Length );
		Debug.Log ("This is my index :" + indexToCreate);
		CurrentModel = Instantiate (ModelList [indexToCreate]);

		CurrentModel.transform.parent = Scene.transform;
		CurrentModel.transform.localPosition = Vector3.zero;
		CurrentModel.transform.localScale = Vector3.one;
		CurrentModel.transform.localRotation = Quaternion.identity;

	}


	private IEnumerator Coroutine_EntranceAnimation () {

		RefreshModel ();

		//EntranceEffect.Stop ();
		Scene.localScale = Vector3.zero;

		float elapsed = 0f;
		float scale = 0;
		bounceDuration = factorEntrance * factorBounce;
		//EntranceEffect.Play ();

		yield return new WaitForSeconds (factorDelay);

		Vector3 temp = new Vector3 ();

		while (elapsed < factorEntrance) {
			yield return null;

			elapsed+=Time.deltaTime ;
			scale =  Mathf.Lerp( 0f, 1f, elapsed/factorEntrance ) ;

			temp.x = scale ;
			temp.y = scale ;
			temp.z = scale ;

			Scene.localScale = temp;
		}

		elapsed = 0.0f;
		Scene.localScale = Vector3.one;

		while (elapsed < bounceDuration ) {

			yield return null ;
			elapsed+=Time.deltaTime ;
			scale =  Mathf.Sin( (elapsed/bounceDuration) * Mathf.PI *2 ) /10.0f;

			temp.x = 1.0f + scale ;
			temp.y = 1.0f + scale ;
			temp.z = 1.0f + scale ;

			Scene.localScale = temp;
		}

		Scene.localScale = Vector3.one;
		//EntranceEffect.Stop ();

	}
}

using UnityEngine;
using UnityEngine.UI ;
using System.Collections;

using Vuforia ;

public class SceneManager_Salmon : MonoBehaviour {

	[SerializeField] private GameObject mainCamera ;

	[SerializeField] private GameObject arFishSchool ;
	[SerializeField] private ImageTargetBehaviour arImageTarget ;

	[SerializeField] private GameObject gyroPivot ;
	[SerializeField] private GameObject gyroFishSchool ;

	private bool arSceneShuttingDown = false ;

	// Deactivate Scropts/Objects before scenes is fully loaded
	private void Awake() {
		arFishSchool.SetActive (false);
	}


	// Use this for initialization
	void Start () {
	
		// Register AR tracking delegates
		FishTrackablEventHandler.gainedTracking += localGainedTracking;
		FishTrackablEventHandler.lostTracking += localLostTracking;
	}

	private void OnDisable () {
		Debug.Log ("Called");
		FishTrackablEventHandler.gainedTracking -= localGainedTracking;
		FishTrackablEventHandler.lostTracking -= localLostTracking;
	}




	void localGainedTracking () {
		arFishSchool.SetActive (true);
	}
	
	void localLostTracking () {

		if( arFishSchool) 
			arFishSchool.SetActive (false);
		ARShutDown ();
	}

	void ARShutDown () {
		if (arSceneShuttingDown){
			return;
		}
		arSceneShuttingDown = true;
		StartCoroutine ("coroutine_ARShutDown");
	}

	private IEnumerator coroutine_ARShutDown () {

		yield return new WaitForSeconds (2f);

		while (arFishSchool.activeSelf) {
			yield return null ;
		}

		GyroSceneSetEnable ();
		arFishSchool.SetActive (false);
	}



	/// <summary>
	/// Prepare the scene for the gyroscope part of the experience
	/// </summary>
	void GyroSceneSetEnable () {

		if (gyroPivot.activeSelf) {
			return;
		}

		arImageTarget.enabled = false;
		arFishSchool.SetActive (false);

		gyroPivot.SetActive (true);
		mainCamera.SetActive (true);

		// Place the camera under the gyro scene
		mainCamera.transform.SetParent (gyroPivot.transform);
		mainCamera.transform.localRotation = Quaternion.identity;
		mainCamera.transform.localPosition = Vector3.zero;

		// Enable the fishes
		gyroFishSchool.SetActive (true);
	}


}

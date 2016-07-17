using UnityEngine;
using UnityEngine.UI ;
using System.Collections;

using Vuforia ;

public class FishSceneManager : MonoBehaviour {

	// Fishschool object to instantiate the 2 needed schools
	// AR is the entrance animation and will be destroyed opnce the user loses tracking 
	// Gyro is triggered once the user looses tracking and can observe it with the gyroscope
	[SerializeField] private GameObject fishSchool_AR ;
	[SerializeField] private GameObject fishSchool_Gyro ;

	// The ARCamera, we need it to enable the Gyroscope and stuff
	[SerializeField] private GameObject cameraTransform ;

	// 
	[SerializeField] private GameObject cameraButton ;

	[SerializeField] private iVidCapPro vr ; 
	[SerializeField] private Text vrText ;
	private bool isRecordingVideo = false ;

	// Camera primitives to get on Start
	[SerializeField] private ImageTargetBehaviour imageTarget ;
	[SerializeField]
	private GameObject 		gyroscopeObject ;

	private GyroController 	cameraGyroscope;

	private bool gainedTracking ;

	private void Awake () {
		fishSchool_AR.SetActive (false);
		fishSchool_Gyro.SetActive (false);

	}

	// Use this for initialization
	void Start () {
		// Prepare the video recording delegates 
		// Register a delegate to be called when the video is complete.
		vr.RegisterSessionCompleteDelegate(HandleSessionComplete);
		// Register a delegate in case an error occurs during the recording session.
		vr.RegisterSessionErrorDelegate(HandleSessionError);
		vrText.text = "";

		cameraGyroscope = gyroscopeObject.GetComponentInParent<GyroController> ();

		cameraButton.SetActive (false);

		gainedTracking = false;
		FishTrackablEventHandler.gainedTracking += localGainedTracking;
		FishTrackablEventHandler.lostTracking += localLostTracking;
	}

	private void OnDestroy() {
		
		FishTrackablEventHandler.gainedTracking -= localGainedTracking;
		FishTrackablEventHandler.lostTracking -= localLostTracking;
	}

	void localGainedTracking () {
		Debug.Log ("Tracking gained");
		if (Time.time <= 1f) 
			return;

		if (gainedTracking) 
			return;

		gainedTracking = true;

		fishSchool_AR.SetActive (true);
		StartCoroutine ("delayedStartfishes");
	}

	void localLostTracking () {
		
		if (Time.time <= 1f) 
			return;

		imageTarget.enabled = false;
		if (fishSchool_AR != null ) {
			fishSchool_AR.SetActive ( false ) ;
		}
	}

	public void actionRecordingButtonPressed () {
		if (isRecordingVideo) {
			StopRecordingVideo ();
		} else {
			StartRecoringVideo ();
		}

	}


	private IEnumerator delayedStartfishes () {
		float duration = 8f;
		bool keepWaiting = true ;


		while (keepWaiting) {
			yield return null ;
			duration -= Time.deltaTime ;

			if( !fishSchool_AR.activeSelf ) keepWaiting = false ;
			else {
				keepWaiting = duration>0f ;
			}
		}

		GameObject.Destroy (fishSchool_AR);

		imageTarget.enabled = false;

		// We restart the gyroscope object to be safe
		cameraGyroscope.transform.rotation = Quaternion.identity;
		cameraGyroscope.CalibrateYAngle ();
		
		// We place the Camera in the gyroscope object 
		cameraTransform.transform.SetParent (gyroscopeObject.transform);
		cameraTransform.transform.rotation = Quaternion.identity;
		cameraTransform.transform.localPosition = Vector3.zero;

		// We activate the Gyro fishes 
		fishSchool_Gyro.SetActive (true);

		// We enable UI to capture the scene 
		cameraButton.SetActive (true);
	}





	private void StartRecoringVideo () {
		isRecordingVideo = true;
		Debug.Log ("Starting to record Video");

		vr.BeginRecordingSession(
			"newVideoName",                             // name of the video
			Screen.width, Screen.height,                       // video width & height in pixels
			30,                                        // frames per second when frame rate Locked/Throttled
			iVidCapPro.CaptureAudio.No_Audio,          // whether or not to record audio
			iVidCapPro.CaptureFramerateLock.Unlocked); // capture type: Unlocked, Locked, Throttled

	}
	private void StopRecordingVideo () {
		isRecordingVideo = false;
		int framesRecorded;
		vr.EndRecordingSession(
			iVidCapPro.VideoDisposition.Save_Video_To_Album ,  // where to put the finished video 
			out framesRecorded);   

		vrText.text = "Processing and saving the video";
		vrText.enabled = true;
		cameraButton.SetActive( false ) ;
		Debug.Log ("Stopped recording video");
	}


	// This delegate function is called when the recording session has completed successfully
	// and the video file has been written.
	public void HandleSessionComplete() {
		Debug.Log ("Video has been succesfully saved");

		vrText.text = "Video has been succesfully saved";
		vrText.enabled = true ;
		StartCoroutine ("hideRecordingText");

		// Do UI stuff when video is complete.
	}
	// This delegate function is called if an error occurs during the recording session.
	public void HandleSessionError(iVidCapPro.SessionStatusCode errorCode) {
		Debug.Log ("Failed saving the video : " + errorCode);
		vrText.text = "Failed to save the video";
		vrText.enabled = true ;
		StartCoroutine ("hideRecordingText");
		// Do stuff when an error occurred.
	}

	private IEnumerator hideRecordingText() {
		float duration = 3f;

		while (duration > 0) {
			yield return null ;
			duration -= Time.deltaTime ;
		}
		vrText.enabled = false;
		vrText.text = "";

		cameraButton.SetActive (true);
	}
}

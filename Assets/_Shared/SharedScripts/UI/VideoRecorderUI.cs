using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI ;



using Vuforia ;

public class VideoRecorderUI : MonoBehaviour {

	[SerializeField] private Sprite ImageNonRecording ;
	[SerializeField] private Sprite ImageRecording ;

	[SerializeField] private Button CameraButton ;

	[SerializeField] private iVidCapPro vr ; 

	[SerializeField] private string VuforiaLicence;

	private bool isRecordingVideo = false ;

	// Use this for initialization
	void Start () {

		vr.RegisterSessionCompleteDelegate (HandleSessionComplete);
		// Register a delegate in case an error occurs during the recording session.
		vr.RegisterSessionErrorDelegate (HandleSessionError);

		CameraButton.image.overrideSprite = ImageNonRecording;

		Camera arCamera = ARCamera ();

		vr = gameObject.AddComponent<iVidCapPro> ();

		if (arCamera) {
			arCamera.gameObject.AddComponent<iVidCapProVideo> ();
			vr.videoCameras = new iVidCapProVideo[] {arCamera.GetComponent<iVidCapProVideo>()} ;

			VuforiaBehaviour behaviour = arCamera.GetComponentInParent<VuforiaBehaviour> ();
			behaviour.SetAppLicenseKey (VuforiaLicence);
		}
	}

	
	public void actionRecordingButtonPressed () {
		if (isRecordingVideo) {
			StopRecordingVideo ();
		} else {
			StartRecoringVideo ();
		}
	}
	
	private void StartRecoringVideo () {
		isRecordingVideo = true;
		Debug.Log ("Starting to record Video");

		CameraButton.image.overrideSprite = ImageRecording;

		vr.BeginRecordingSession(
			"newVideoName",                             // name of the video
			Screen.width, Screen.height,                       // video width & height in pixels
			30,                                        // frames per second when frame rate Locked/Throttled
			iVidCapPro.CaptureAudio.No_Audio,          // whether or not to record audio
			iVidCapPro.CaptureFramerateLock.Unlocked); // caspture type: Unlocked, Locked, Throttled
		
	}
	private void StopRecordingVideo () {
		isRecordingVideo = false;
		int framesRecorded;
		vr.EndRecordingSession(
			iVidCapPro.VideoDisposition.Save_Video_To_Album ,  // where to put the finished video 
			out framesRecorded);   

		CameraButton.gameObject.SetActive( true ) ;
		Debug.Log ("Stopped recording video");
	}

	
	// This delegate function is called when the recording session has completed successfully
	// and the video file has been written.
	public void HandleSessionComplete() {
		CameraButton.image.overrideSprite = ImageNonRecording;
		
		Debug.Log ("Video has been succesfully saved");

		StartCoroutine ("hideRecordingText");
		
		// Do UI stuff when video is complete.
	}
	// This delegate function is called if an error occurs during the recording session.
	public void HandleSessionError(iVidCapPro.SessionStatusCode errorCode) {
		Debug.Log ("Failed saving the video : " + errorCode);
		StartCoroutine ("hideRecordingText");
		// Do stuff when an error occurred.
	}
	
	private IEnumerator hideRecordingText() {
		float duration = 3f;
		
		while (duration > 0) {
			yield return null ;
			duration -= Time.deltaTime ;
		}
		
		CameraButton.gameObject.SetActive (true);
	}



	/// <summary>
	/// Return the AR Camera of the scene if it exists
	/// Returns null otherwise 
	/// </summary>
	/// <returns>The camera.</returns>
	private Camera ARCamera () {
		
		foreach (Camera cam in Camera.allCameras) {
			if (cam.GetComponent<VideoBackgroundBehaviour> ()) {
				return cam;
			}
		}
			
		return null;
	}
		
}

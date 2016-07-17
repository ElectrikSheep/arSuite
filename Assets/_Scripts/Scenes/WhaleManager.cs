using UnityEngine;
using System.Collections;

public class WhaleManager: MonoBehaviour {



	// to start the animations at a differentTime 
	[SerializeField] private Animation whale_1 ;
	[SerializeField] private Animation whale_2 ;


	// Place the whales
	[SerializeField] private Transform groupTransform ;

	// The elipse caracteristics( I suggest 30,250,0.05 for the current scale
	[SerializeField] private float elipseA = 1.5f ;
	[SerializeField] private float elipseB = 0.5f ;
	[SerializeField] private float elipseH = 10f ;
	[SerializeField] private float elipseSpeed = .005f ;


	// Use this for initialization
	void Start () {

		StartCoroutine ("Coroutine_StartAnimating");

	}



	/// <summary>
	/// Here we set the Whales position and rotation 
	/// </summary>
	private void Update () {
		// Setting the position
		Vector3 pos = groupTransform.position;
		Vector3 target = groupTransform.position;

		pos.x = (elipseA * Mathf.Cos (Time.time * elipseSpeed));
		pos.z = (elipseB * Mathf.Sin (Time.time * elipseSpeed));
		pos.y =  (elipseH * Mathf.Abs (Mathf.Sin (Time.time * elipseSpeed)*Mathf.Sin (Time.time * elipseSpeed)));
		groupTransform.position = pos;

		// Necessary for debug, making sure the values are evolving the way they do.
//		Debug.Log("X : " + Mathf.Cos (Time.time * elipseSpeed) + " Y : "  + Mathf.Sin (Time.time * elipseSpeed)) ;

		// Rotation 
		target.x = 2.0f*target.x - pos.x;
		target.y = 2.0f*target.y - pos.y;
		target.z = 2.0f*target.z - pos.z;

		groupTransform.LookAt (target);
	}

	/// <summary>
	/// Start the animations for the 2 whales to avoid having them completely synced (and looking robotic) 
	/// </summary>
	private IEnumerator Coroutine_StartAnimating () {
		whale_1.Play ("swim");
		yield return new WaitForSeconds( 0.8f ) ;
		whale_2.Play ("swim");
	}



}

using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;

public class BackButton : MonoBehaviour {

	private bool isLoadingMenuScene = false ;


	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;
		isLoadingMenuScene = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void actionBackButtonClicked () {
		if (isLoadingMenuScene)
			return;

		isLoadingMenuScene = true;
		SceneManager.LoadScene (0);
	}
}

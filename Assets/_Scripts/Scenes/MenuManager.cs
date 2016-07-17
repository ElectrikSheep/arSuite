using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;

public class MenuManager : MonoBehaviour {

	private bool isLoadingLevel = false ;

	// Use this for initialization
	void Start () {
		isLoadingLevel = false;
	}


	/// <summary>
	/// Loads the bass scene.
	/// </summary>
	public void LoadBassScene () {
		LoadScene ("BassScene");
	}	
	public void LoadDolphin () {
		LoadScene ("DolphinScene");
	}	
	public void LoadKoiPoind () {
		LoadScene ("KoiPondScene");
	}
	public void LoadButterflies () {
		LoadScene ("ButterflyScene");
	}
	public void LoadSalmon_AR () {
		LoadScene ("SalmonScene_AR");
	}
	public void LoadSalmon_Gyro () {
		LoadScene ("SalmonScene_Gyro");
	}
	public void LoadComparison () {
		LoadScene ("ComparisonScene");
	}
	public void LoadWhale () {
		LoadScene ("WhaleScene");
	}
	public void LoadCaptainGround () {
		LoadScene ("CaptainAmericaGround");
	}
	public void LoadCaptainWall () {
		LoadScene ("CaptainAmericaWall");
	}
	public void LoadDino () {
		LoadScene ("DinoScene");
	}
	public void LoadExtendedCaptain() {
		LoadScene ("CaptainAmerica_GroundExtended");
	}
	
	public void LoadWizard() {
		LoadScene ("WizardScene");
	}

	void LoadScene( string sceneName ) {

		if (isLoadingLevel)
			return;

		isLoadingLevel = true;
		SceneManager.LoadScene (sceneName);
	}
}

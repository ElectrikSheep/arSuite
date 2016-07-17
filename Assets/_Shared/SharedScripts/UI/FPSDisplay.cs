using UnityEngine;
using System.Collections;
using UnityEngine.UI ;

public class FPSDisplay : MonoBehaviour
{

	private Text fpsLabel ;
	private float deltaTime = 0.0f;

	private void Start() {
			
		fpsLabel = GetComponent<Text> ();

	}

	void Update()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		fpsLabel.text = string.Format("{0:0.0} ms\n({1:0.} fps)", msec, fps);

		if (fps < 15.0f) {
			fpsLabel.color = Color.red;
			return;
		} else if (fps < 25.0f) {
			fpsLabel.color = Color.yellow;
		} else {
			fpsLabel.color = Color.green ;
		}
	}

}
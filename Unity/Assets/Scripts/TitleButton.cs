using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour {

	void Start(){
		QualitySettings.antiAliasing = 3;
//		Camera.main.renderingPath = RenderingPath.DeferredShading;
		Debug.Log (QualitySettings.antiAliasing);
	}

	public void OnClick(){
		if (gameObject.name == "StartButton") {
			SceneManager.LoadScene ("stage");
//			Application.LoadLevel("stage");
		} else if (gameObject.name == "ArsenalButton") {
			SceneManager.LoadScene ("Customize");
//			Application.LoadLevel("stage");
		} else if (gameObject.name == "EndButton") {
			Application.Quit ();
		}
	}
}

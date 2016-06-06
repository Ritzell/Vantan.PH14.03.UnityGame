using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour {

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

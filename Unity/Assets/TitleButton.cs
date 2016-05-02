using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour {

	public void OnClick(){
		if (gameObject.name == "StartButton") {
			SceneManager.LoadScene ("stage");
		} else if (gameObject.name == "ConfigButton") {
			SceneManager.LoadScene ("stage");
		} else if (gameObject.name == "EndButton") {
			Application.Quit ();
		}
	}
}

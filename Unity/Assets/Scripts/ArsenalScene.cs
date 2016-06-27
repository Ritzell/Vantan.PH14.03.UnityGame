using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ArsenalScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (SceneTransition ());
		FindObjectOfType<CameraSetting> ().OnScene (Scenes.Customize);

	}
	
	private IEnumerator SceneTransition(){
		while (true) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				SceneManager.LoadScene ("title");
				yield return null;
			} else if (Input.GetKeyDown (KeyCode.Space)) {
				SceneManager.LoadScene ("stage");
				yield return null;
			}
			yield return null;
		}
	}
}

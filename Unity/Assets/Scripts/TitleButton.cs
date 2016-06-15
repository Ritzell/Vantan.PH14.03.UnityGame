using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour {

	public static CameraSystem CameraS;

	void Start(){
		CameraS = FindObjectOfType<CameraSystem> ();
		QualitySettings.antiAliasing = 3;
//		Camera.main.renderingPath = RenderingPath.DeferredShading;
	}

	public void OnClick(){
		if (gameObject.name == "StartButton") {
			StartCoroutine (GameManager.FlashLoadScene ("stage"));
		} else if (gameObject.name == "ArsenalButton") {
			StartCoroutine (GameManager.FlashLoadScene ("Customize"));
		} else if (gameObject.name == "EndButton") {
			Application.Quit ();
		}
	}

//	private IEnumerator FlashLoadScene(string SceneName){
//		yield return StartCoroutine(CameraS.Flash(1,false,1));
//		SceneManager.LoadScene (SceneName);
//		yield return null;
//	}
}

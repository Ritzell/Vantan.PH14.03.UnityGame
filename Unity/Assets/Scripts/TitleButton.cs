using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleButton : MonoBehaviour {

	public static CameraSystem CameraS;
	[SerializeField]
	private GameObject SettingWindow;

	public enum Buttons {
		Start,
		Cusomize,
		Setting,
		End
	}

	public Buttons buttons;

	void Start(){
		SettingWindow.SetActive (false);
		CameraS = FindObjectOfType<CameraSystem> ();
		QualitySettings.antiAliasing = 3;
		FindObjectOfType<CameraSetting> ().OnScene (Scenes.Title);
	}

	public void OnClick(){
		ClickEvent (buttons, gameObject.GetComponent<TitleButton> (), SettingWindow);
	}

	private static void ClickEvent(Buttons Button,TitleButton origin,GameObject SettingWindow){
		if (Button == Buttons.Start) {
			origin.StartCoroutine (GameManager.FlashLoadScene (GameManager.Scenes.stage));
		} else if (Button == Buttons.Cusomize) {
			origin.StartCoroutine (GameManager.FlashLoadScene (GameManager.Scenes.customize));
		} else if (Button == Buttons.Setting) {
			SettingWindow.SetActive(true);
			origin.StartCoroutine (CloseSettingWindow (SettingWindow));
		}else if (Button == Buttons.End) {
			Application.Quit ();
		}
	}
//	[SerializeField]


	private static IEnumerator CloseSettingWindow(GameObject SettingWindow){
		while (SettingWindow.activeSelf) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				SettingWindow.SetActive (false);
			}
			yield return null;
		}
	}


//	private IEnumerator FlashLoadScene(string SceneName){
//		yield return StartCoroutine(CameraS.Flash(1,false,1));
//		SceneManager.LoadScene (SceneName);
//		yield return null;
//	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class NotificationSystem : MonoBehaviour {
	private static int MaxSentences = 5;
	private static float ScreenRatio;
	private static GameObject Notification;
	private static NotificationSystem NotiSystem;
	private static List<RectTransform> Sentences = new List<RectTransform> ();
	// Use this for initialization
	void Awake () {
		ScreenRatio = (float)Screen.height / 332;
		Notification = gameObject;//.GetComponent<Text> ();
		NotiSystem = gameObject.GetComponent<NotificationSystem>();
	}

	void Start(){
//		StartCoroutine (test ());
	}

	private IEnumerator test(){
		int i = 0;
		while (true) {
			i++;
			yield return StartCoroutine (UpdateNotification("羊が"+i+"匹"));
			yield return new WaitForSeconds (1f);
			yield return null;
		}
	}

	public static IEnumerator UpdateNotification(string text){
		GameObject TextBox = NewTextBox ();
		yield return NotiSystem.StartCoroutine (MoveUpSentences());
		yield return NotiSystem.StartCoroutine (TextBoxStartUp(TextBox));
		yield return NotiSystem.StartCoroutine(NotificationSystem.FadeInText(TextBox.GetComponent<Text> (),text));
		yield return null;
	}

	private static GameObject NewTextBox(){
		GameObject TextBox = new GameObject ("Text");
		TextBox.transform.parent = Notification.transform;
		TextBox.AddComponent<Text> ();
		return TextBox;
	}

	public static IEnumerator FadeInText(Text TextBox,string text){
		TextBox.text = text;
		while(TextBox.color.a < 1){
			TextBox.color = new Color (TextBox.color.r,TextBox.color.g,TextBox.color.b,TextBox.color.a + (Time.deltaTime/0.1f));
				yield return null;
		}
	}

	public static IEnumerator MoveUpSentences(){
		MoveTrashSentence ();
		for (float time = 0f; time < 0.1f; time += Time.deltaTime) {
			foreach (RectTransform Sentence in Sentences) {
				Sentence.Translate (0, (ScreenRatio * 10) * (Time.deltaTime / 0.1f), 0);
			}
			yield return null;
		}
		yield return null;
	}

	private static void MoveTrashSentence(){
		if(Sentences.Count > MaxSentences){
			Destroy(Sentences[0].gameObject);
			Sentences.RemoveAt(0);
		}
	}

	public static IEnumerator TextBoxStartUp(GameObject TextBox){
		var TextRect = TextBox.GetComponent<RectTransform> ();
		var TextComponent = TextBox.GetComponent<Text> ();
		TextRect.pivot = new Vector2 (1,1);
		TextRect.transform.localPosition = new Vector3 (0,0,0);
		TextRect.sizeDelta = new Vector2 (170,ScreenRatio * 20);
		TextRect.anchorMax = new Vector2 (1,0);
		TextRect.anchorMin = new Vector2 (1,0);
		TextComponent.alignment = TextAnchor.MiddleRight;
		TextComponent.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
		TextComponent.fontSize = (int)(ScreenRatio * 10);
		TextComponent.color = new Color (TextComponent.color.r,TextComponent.color.g,TextComponent.color.b,0);
		Sentences.Add (TextBox.GetComponent<RectTransform>());
		yield return null;
	}

	private static void EntrySentence(string text,Text NewSentence){
		NewSentence.text = text;
	}
}

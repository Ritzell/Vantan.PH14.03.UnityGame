using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class NotificationSystem : MonoBehaviour {
	private static int MaxSentences = 5;
	private static float ScreenRatioX;
	private static float ScreenRatioY;
	private static Transform AnnounceNotification;
	private static Transform MissileNotification;
	private static NotificationSystem NotiSystem;
	private static List<RectTransform> Sentences = new List<RectTransform> ();
	private static List<RectTransform> MissileSentences = new List<RectTransform> ();
	// Use this for initialization
	void Awake () {
		ScreenRatioY = (float)Screen.height / 332;
		ScreenRatioX = (float)Screen.width / 588;
		AnnounceNotification = GameObject.Find("Notification").transform;
		MissileNotification = GameObject.Find("MissileNotification").transform;
		NotiSystem = gameObject.GetComponent<NotificationSystem>();
	}

	void Start(){
	}

	public enum NotificationType{
		Announce,
		Missile
	}

	private IEnumerator test(){
		int i = 0;
		while (true) {
			i++;
			Debug.Log ("aaaa");
			StartCoroutine (UpdateMissileNotification());
			yield return new WaitForSeconds (1f);
			yield return null;
		}
	}

	public static IEnumerator UpdateNotification(string text){
		GameObject TextBox = NewTextBox (AnnounceNotification);
		yield return NotiSystem.StartCoroutine (MoveUpSentences(true,NotificationType.Announce));
		yield return NotiSystem.StartCoroutine (TextBoxStartUp(TextBox,NotificationType.Announce));
		yield return NotiSystem.StartCoroutine(NotiSystem.FadeInText(TextBox.GetComponent<Text> (),text));
		yield return null;
	}
	public static IEnumerator UpdateMissileNotification(){
		GameObject TextBox = NewTextBox (MissileNotification);
		yield return NotiSystem.StartCoroutine (MoveUpSentences(false,NotificationType.Missile));
		yield return NotiSystem.StartCoroutine (TextBoxStartUp(TextBox,NotificationType.Missile));
		EntrySentence ("Hit!", TextBox.GetComponent<Text> ());
		yield return NotiSystem.StartCoroutine(NotiSystem.FadeOutText(TextBox.GetComponent<Text> (),1f));
		yield return null;
	}

	private static GameObject NewTextBox(Transform Notification){
		GameObject TextBox = new GameObject ("Text");
		TextBox.transform.parent = Notification.transform;
		TextBox.AddComponent<Text> ();
		return TextBox;
	}

	public IEnumerator FadeInText(Text TextBox,string text){
		TextBox.text = text;
		while(TextBox.color.a < 1){
			TextBox.color = new Color (TextBox.color.r,TextBox.color.g,TextBox.color.b,TextBox.color.a + (Time.deltaTime/0.1f));
				yield return null;
		}
	}
	public IEnumerator FadeOutText(Text TextBox,float delay){
		yield return new WaitForSeconds (delay);
		while (TextBox.color.a > 0) {
			TextBox.color = new Color (TextBox.color.r,TextBox.color.g,TextBox.color.b,TextBox.color.a - Time.deltaTime);
			yield return null;
		}
		RectTransform TextTransform = TextBox.GetComponent<RectTransform> ();
		if(Sentences.Contains(TextTransform)){
			Sentences.Remove(TextTransform);
		} else if(MissileSentences.Contains(TextTransform)){
			MissileSentences.Remove(TextTransform);
		}
		Destroy (TextBox.gameObject);
		yield return null;
	}

	public static IEnumerator MoveUpSentences(bool isEnableTrash,NotificationType type){//List<RectTransform> TextList){
		List<RectTransform> TextList = new List<RectTransform>();
		if (type == NotificationType.Announce) {
			TextList = Sentences;
		} else {
			TextList = MissileSentences;
		}
		if (isEnableTrash) {
			MoveTrashSentence ();
			if (TextList.Count != 0) {
				TextList [TextList.Count - 1].GetComponent<Text> ().color = Color.white;
			}
		}
		
		for (float time = 0f; time < 0.1f; time += Time.deltaTime) {
			foreach (RectTransform text in TextList) {
				text.Translate (0, (ScreenRatioY * (isEnableTrash ? 10 : 25)) * (Time.deltaTime / 0.1f), 0);
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



	public static IEnumerator TextBoxStartUp(GameObject TextBox,NotificationType type){
		bool isAnnounce = type == NotificationType.Announce ? true : false;

		var TextRect = TextBox.GetComponent<RectTransform> ();
		var TextComponent = TextBox.GetComponent<Text> ();
		TextRect.pivot = isAnnounce ? new Vector2 (1,1) : new Vector2 (0.5f,0.5f);
		TextRect.transform.localPosition = new Vector3 (0,0,0);
		TextRect.sizeDelta = isAnnounce ? new Vector2 (ScreenRatioX*300,ScreenRatioY * 20) : new Vector2 (ScreenRatioX*300,ScreenRatioY * 28);
		TextRect.anchorMax = isAnnounce ? new Vector2 (1,0) : new Vector2 (0.5f,0);
		TextRect.anchorMin = TextRect.anchorMax;
		TextComponent.alignment = isAnnounce ? TextAnchor.MiddleRight : TextAnchor.MiddleCenter;
		TextComponent.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
		TextComponent.fontSize = isAnnounce ? (int)(ScreenRatioY * 10) : (int)(ScreenRatioY * 25);
		TextComponent.color = isAnnounce ? new Color (1,0.92f,0.16f,0) : new Color (1,0f,0f,0.8f) ;
		if (isAnnounce) {
			Sentences.Add (TextBox.GetComponent<RectTransform> ());
		} else {
			MissileSentences.Add (TextBox.GetComponent<RectTransform> ());
		}
		yield return null;
	}

	private static void EntrySentence(string text,Text NewSentence){
		NewSentence.text = text;
	}
}

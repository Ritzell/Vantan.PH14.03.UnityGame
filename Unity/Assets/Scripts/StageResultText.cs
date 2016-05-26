using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StageResultText : MonoBehaviour {
	private static Text ForwardText;
	private static Text BackText;

	void Awake () {
		ForwardText = transform.FindChild ("ResultForward").GetComponent<Text>();
		BackText = transform.FindChild ("ResultBack").GetComponent<Text>();
	}
	
	public static void DisplayResult(bool Victory){
		GameObject.Find ("ResultText").GetComponent<StageResultText>().StartCoroutine(ChangeColor(Victory));
		ChangeText (Victory);
	}
	private static IEnumerator ChangeColor(bool Victory){
		Color red = new Color(1,0,0,0);
		Color yellow = new Color(1,1,0,0);
		Color blue = new Color (0,0,1,0);
		Color dark = new Color (0f,0f,0f,0);

		ForwardText.color = Victory ? red : blue;
		BackText.color = Victory ? yellow : dark;

		while(ForwardText.color.a < 1){
			FadeIn ();
			yield return null;
		}
		yield return null;
	}
	private static void ChangeText(bool Victory){
		ForwardText.text = Victory ? "Victory" : "Defeat";
		BackText.text = Victory ? "Victory" : "Defeat";
	}
	private static void FadeIn(){
		ForwardText.color = new Color (ForwardText.color.r, ForwardText.color.g, ForwardText.color.b, ForwardText.color.a +(10f*Time.deltaTime));
		BackText.color = new Color(BackText.color.r,BackText.color.g,BackText.color.b,BackText.color.a + (10f*Time.deltaTime));
	}
}

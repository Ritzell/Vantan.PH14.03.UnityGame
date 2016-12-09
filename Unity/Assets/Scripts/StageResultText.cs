using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StageResultText : MonoBehaviour {
	private static Text ForwardText;
	private static Text BackText;
	private static StageResultText Result;

	void Awake () {
		ForwardText = transform.FindChild ("ResultForward").GetComponent<Text>();
		BackText = transform.FindChild ("ResultBack").GetComponent<Text>();
		Result = GameObject.FindObjectOfType<StageResultText> ();
	}
		
	
	public static void DisplayResult(bool isVictory){
		try{
		Result.StartCoroutine(Result.ChangeColor(isVictory));
		ChangeText (isVictory);
		}catch{
			Debug.Log ("DisplayResult Error");
		}
	}
	private IEnumerator ChangeColor(bool isVictory){
		Color red = new Color(1,0,0,0);
		Color yellow = new Color(1,1,0,0);
		Color blue = new Color (0,0,1,0);
		Color dark = new Color (0f,0f,0f,0);

		ForwardText.color = isVictory ? red : blue;
		BackText.color = isVictory ? yellow : dark;

		while(ForwardText.color.a < 1){
			yield return StartCoroutine(FadeIn());
		}
		yield return null;
	}
	private static void ChangeText(bool isVictory){
		ForwardText.text = isVictory ? "Victory" : "Defeat";
		BackText.text = isVictory ? "Victory" : "Defeat";
	}
	private IEnumerator FadeIn(){
		ForwardText.color = new Color (ForwardText.color.r, ForwardText.color.g, ForwardText.color.b, ForwardText.color.a +(Time.deltaTime * (1 / Time.timeScale)));
		BackText.color = new Color(BackText.color.r,BackText.color.g,BackText.color.b,BackText.color.a + (Time.deltaTime * (1 / Time.timeScale )));
		yield return null;
	}
}

using UnityEngine;
using System.Collections;

public class explosion : MonoBehaviour {
	AudioSource AudioBox;
	void Start () {
		AudioBox = GetComponent<AudioSource> ();
		AudioBox.Play ();
		StartCoroutine (Deth ());
	}
	private IEnumerator Deth(){
		while (AudioBox.isPlaying) {
			yield return null;
		}
		Destroy (gameObject);
		yield return null;
	}
}

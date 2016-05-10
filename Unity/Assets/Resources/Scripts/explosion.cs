using UnityEngine;
using System.Collections;

public class explosion : MonoBehaviour {
	AudioSource audioBox;
	void Start () {
		audioBox = GetComponent<AudioSource> ();
		audioBox.Play ();
		StartCoroutine (deth ());
	}
	private IEnumerator deth(){
		while (audioBox.isPlaying) {
			yield return null;
		}
		Destroy (gameObject);
		yield return null;
	}
}

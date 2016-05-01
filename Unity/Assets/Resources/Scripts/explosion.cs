using UnityEngine;
using System.Collections;

public class explosion : MonoBehaviour {
	AudioSource audio;
	// Use this for initialization
	void Start () {
		gameObject.AddComponent<AudioSource> ();
		audio = GetComponent<AudioSource> ();
		audio.clip = (AudioClip)Resources.Load ("Sounds/MissileHit!");
		audio.Play ();
		StartCoroutine (deth ());
	}
	private IEnumerator deth(){
		while (audio.isPlaying) {
			yield return null;
		}
		Destroy (gameObject);
		yield return null;
	}
}

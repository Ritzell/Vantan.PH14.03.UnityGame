using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
	AudioSource AudioBox;

	void Awake(){
		AudioBox = GetComponent<AudioSource> ();
	}

	void Start ()
	{
		AudioBox.Play ();
		StartCoroutine (Deth ());
	}

	private IEnumerator Deth ()
	{
		while (AudioBox.isPlaying) {
			yield return null;
		}
		Destroy (gameObject);
		yield return null;
	}
}

using UnityEngine;
using System.Collections;

public class muzzle : MonoBehaviour {
	AudioSource audioBox;
	// Use this for initialization
	void Start () {
		audioBox = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	public IEnumerator Ignition(){
		StartCoroutine (transform.FindChild ("bullet").GetComponent<bullet> ().shot ());
		audioBox.PlayOneShot (audioBox.clip);
		yield return null;
	}
}

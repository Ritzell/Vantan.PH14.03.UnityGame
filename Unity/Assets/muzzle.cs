using UnityEngine;
using System.Collections;

public class muzzle : MonoBehaviour {
	AudioSource audio;
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	}
	public IEnumerator Ignition(){
		StartCoroutine (transform.FindChild ("bullet").GetComponent<bullet> ().shot ());
		audio.PlayOneShot (audio.clip);
		yield return null;
	}
}

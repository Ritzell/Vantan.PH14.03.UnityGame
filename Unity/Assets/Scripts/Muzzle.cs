using UnityEngine;
using System.Collections;

public class Muzzle : MonoBehaviour
{
	AudioSource AudioBox;
	// Use this for initialization
	void Awake ()
	{
		AudioBox = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	public IEnumerator Ignition ()
	{
		try{
		StartCoroutine (transform.FindChild ("bullet").GetComponent<Bullet> ().Shot ());
		}catch{
		}
		AudioBox.PlayOneShot (AudioBox.clip);
		yield return null;
	}
}

using UnityEngine;
using System.Collections;

public class Muzzle : MonoBehaviour
{
	AudioSource AudioBox;
	// Use this for initialization
	void Start ()
	{
		AudioBox = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	public IEnumerator Ignition ()
	{
		StartCoroutine (transform.FindChild ("bullet").GetComponent<Bullet> ().Shot ());
		AudioBox.PlayOneShot (AudioBox.clip);
		yield return null;
	}
}

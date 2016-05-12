using UnityEngine;
using System.Collections;

public class PlayerSound : MonoBehaviour {
	private static AudioSource AudioBox;
	public AudioClip AudioClip;
	// Use this for initialization
	void Start () {
		AudioBox = GetComponent<AudioSource>();
		AudioBox.clip = AudioClip;
	}
	public static void AudioPlay(){
		AudioBox.volume = 0.3f;
		AudioBox.pitch = 2;
		AudioBox.Play ();
	}
}

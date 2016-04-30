using UnityEngine;
using System.Collections;

public class PlayerSound : MonoBehaviour {
	private static AudioSource audio;
	public AudioClip audioclip1;
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
		audio.clip = audioclip1;
	}
	public static void audioPlay(){
		audio.volume = 0.3f;
		audio.pitch = 2;
		audio.Play ();
	}
}

using UnityEngine;
using System.Collections;

public class PlayerSound : MonoBehaviour {
	private static AudioSource audioBox;
	public AudioClip audioclip1;
	// Use this for initialization
	void Start () {
		audioBox = GetComponent<AudioSource>();
		audioBox.clip = audioclip1;
	}
	public static void audioPlay(){
		audioBox.volume = 0.3f;
		audioBox.pitch = 2;
		audioBox.Play ();
	}
}

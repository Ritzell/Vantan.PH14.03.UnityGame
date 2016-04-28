using UnityEngine;
using System.Collections;

public class engineSound : MonoBehaviour {
	AudioSource audio;
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();
		audio.pitch = audio.pitch;
	}

	public float Pitch{
		set{
			float v = (value - 300f)*0.005f;
			if (audio.pitch <= 3 && audio.pitch >= 0.5f) {
				audio.pitch = v + 1;
				if (audio.pitch > 3) {
					audio.pitch = 3;
				} else if (audio.pitch < 0.5f) {
					audio.pitch = 0.5f;
				}
			}
		}get{
			return audio.pitch;
		}
	}
}

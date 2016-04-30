using UnityEngine;
using System.Collections;

public class engineSound : MonoBehaviour {
	AudioSource audio;
	// Use this for initialization
	public struct engineConfig
	{
		public const float normalSpeed = 300;
		public const float pitchUp = 0.005f;
	}
	void Start () {
		audio = GetComponent<AudioSource> ();
		audio.pitch = audio.pitch;
	}

	public float Pitch{
		set{
			float v = (value - engineConfig.normalSpeed)*engineConfig.pitchUp;
			try{
			if (audio.pitch <= 3 && audio.pitch >= 0.5f) {
				audio.pitch = v + 1;
				if (audio.pitch > 3) {
					audio.pitch = 3;
				} else if (audio.pitch < 0.5f) {
					audio.pitch = 0.5f;
				}
			}
			}catch{
			}
		}get{
			return audio.pitch;
		}
	}
}

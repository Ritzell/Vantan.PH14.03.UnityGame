using UnityEngine;
using System.Collections;

public class engineSound : MonoBehaviour {
	AudioSource audioBox;
	// Use this for initialization

	public struct engineConfig
	{
		public const float normalSpeed = 300;
		public const float pitchUpSpeed = 0.005f;
	}

	void Start () {
		audioBox = GetComponent<AudioSource> ();
		audioBox.pitch = audioBox.pitch;
	}

	public float Pitch{
		set{
			float v = (value - engineConfig.normalSpeed)*engineConfig.pitchUpSpeed;
			try{
				changePitch(v);
			}catch{
			}
		}get{
			return audioBox.pitch;
		}
	}

	public void changePitch(float v){
		if (audioBox.pitch <= 3 && audioBox.pitch >= 0.5f) {
			audioBox.pitch = v + 1;
			if (audioBox.pitch > 3) {
				audioBox.pitch = 3;
			} else if (audioBox.pitch < 0.5f) {
				audioBox.pitch = 0.5f;
			}
		}
	}
}

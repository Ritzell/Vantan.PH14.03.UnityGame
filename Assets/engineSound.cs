using UnityEngine;
using System.Collections;

public class engineSound : MonoBehaviour {
	AudioSource audio;
	private float pitch = 1;
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();
		audio.pitch = pitch;
	}

	public float Pitch{
		set{
			float v = (value - 80f)*0.01f;
			if (pitch <= 3 && pitch >= 0.75f) {
				pitch = v + 1;
				if (pitch > 3) {
					pitch = 3;
				} else if (pitch < 0.75f) {
					pitch = 0.75f;
				}
			}
			audio.pitch = this.pitch;
		}get{
			return pitch;
		}
	}
}

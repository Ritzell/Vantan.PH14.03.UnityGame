using UnityEngine;
using System.Collections;

public class PlayerSound : MonoBehaviour
{
	private static AudioSource AudioBox;

	[SerializeField]
	private AudioClip AudioClip;

	void Awake(){
		AudioBox = GetComponent<AudioSource> ();
	}

	void Start ()
	{
		AudioBox.clip = AudioClip;
	}

	public static void HitSound ()
	{
//		AudioBox.volume = 0.3f;
//		AudioBox.pitch = 2;
		AudioBox.Play ();
	}
}

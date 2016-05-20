using UnityEngine;
using System.Collections;

public class Airframe : MonoBehaviour
{
	private static int HP = 3;

	public void Reload (Vector3 StartPos, Quaternion StartRot)
	{
		StartCoroutine (GameObject.Find ("GameManager").GetComponent<GameManager> ().ReloadMissile (StartPos, StartRot));
	}

	private void OnTriggerEnter (Collider Col)
	{
		HP--;
		StartCoroutine( CameraSystem.SwayCamera ());
		if (HP <= 0) {
			Deth ();
		}
	}

	private void Deth ()
	{
		GameManager.loadScene ();
		//PlayerSound.AudioPlay ();
		Destroy (gameObject);
	}
}
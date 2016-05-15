using UnityEngine;
using System.Collections;

public class Airframe : MonoBehaviour
{

	public void Reload (Vector3 StartPos, Quaternion StartRot)
	{
		StartCoroutine (GameObject.Find ("GameManager").GetComponent<GameManager> ().ReloadMissile (StartPos, StartRot));
	}

	private void OnTriggerEnter (Collider Col)
	{
		Deth ();
	}

	private void Deth ()
	{
		GameManager.loadScene ();
		//PlayerSound.AudioPlay ();
		Destroy (gameObject);
	}
}
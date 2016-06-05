using UnityEngine;
using System.Collections;

public class Airframe : MonoBehaviour
{
	private static int HP = 5;



	public void Reload (Vector3 StartPos, Quaternion StartRot)
	{
		StartCoroutine (GameObject.Find ("GameManager").GetComponent<GameManager> ().ReloadMissile (StartPos, StartRot));
	}

	private void OnTriggerEnter (Collider Col)
	{
		HP -= 1;
		StartCoroutine (LightingControlSystem.TurningOff (UIType.HP));
		StartCoroutine( CameraSystem.SwayCamera ());
		PlayerSound.HitSound ();
		if (HP <= 0 || Col.gameObject.layer == 10) {
			Instantiate (Resources.Load ("prefabs/Explosion"), transform.position, Quaternion.identity);
			StartCoroutine (Deth ());
		}
	}

	private IEnumerator Deth ()
	{
		if(GameManager.GameOver){
			yield break;
		}
		Destroy (gameObject);
		yield return StartCoroutine( GameManager.GameEnd (false));
	}
}
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
	private float Speed = 800f;
	public static LightingControlSystem Lighting;// = FindObjectOfType<LightingControlSystem> ();
	private static Image GunHitImage;
	public static Image GunHitImages{
		set{
			GunHitImage = value;
		}
	}

	public IEnumerator Shot ()
	{
		transform.parent = null;
		StartCoroutine (TimeLimit ());
		while (!GameManager.GameOver) {
			try {
				MoveForward ();
			} catch {
			}
			yield return null;
		}
	}

	private void MoveForward ()
	{
		transform.Translate (Vector3.forward * Time.deltaTime * Speed);
	}

	private IEnumerator  TimeLimit ()
	{
		yield return new WaitForSeconds (15);
		Destroy (gameObject);
		yield return null;
	}

	private static Coroutine stopCoroutine;
		
	void OnTriggerStay (Collider col)
	{
		if (col.gameObject.layer == 11 || col.gameObject.layer == 12) {
			if (stopCoroutine != null) {
				Lighting.StopCoroutine (stopCoroutine);
			}
			stopCoroutine = Lighting.StartCoroutine (LightingControlSystem.TurningOnGunHit());
		}
		Destroy (gameObject);
	}
}

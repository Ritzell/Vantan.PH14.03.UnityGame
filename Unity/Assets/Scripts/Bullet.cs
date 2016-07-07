using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
	private float Speed = 800f;
	public static LightingControlSystem Lighting;// = FindObjectOfType<LightingControlSystem> ();


	public IEnumerator Shot ()
	{
		transform.parent = null;
		StartCoroutine (TimeLimit ());
		while (!GameManager.IsGameOver) {
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
		if (col.gameObject.layer == (int)Layers.Enemy || col.gameObject.layer == (int)Layers.EnemyMissile) {
			if (stopCoroutine != null) {
				Lighting.StopCoroutine (stopCoroutine);
			}
			stopCoroutine = Lighting.StartCoroutine (LightingControlSystem.TurningOnGunHit());
		}
		Destroy (gameObject);
	}
}

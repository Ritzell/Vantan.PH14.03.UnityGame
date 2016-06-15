using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	private float Speed = 800f;

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
		float time = 0f;
		while (time < 7f && !GameManager.GameOver) {
			time += Time.deltaTime;
			yield return null;
		}
		Destroy (gameObject);
		yield return null;
	}

	void OnTriggerEnter (Collider col)
	{
		Destroy (gameObject);
	}
}

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
		yield return new WaitForSeconds (15);
		StartCoroutine (BreakBullet ());
		yield return null;
	}

	void OnTriggerEnter (Collider col)
	{
		StartCoroutine (BreakBullet ());
	}

	private IEnumerator BreakBullet ()
	{
		Destroy (gameObject);
		yield return null;
	}
}

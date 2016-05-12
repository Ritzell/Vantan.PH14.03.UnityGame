using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	private float speed = 800f;

	public IEnumerator Shot(){
		transform.parent = null;
		StartCoroutine (TimeLimit ());
		while (!GameManager.GameOver) {
		try{
			MoveForward();
		}catch{
		}
			yield return null;
		}
	}
	public void MoveForward(){
		transform.Translate(Vector3.forward * Time.deltaTime * speed);
	}

	public IEnumerator  TimeLimit(){
		float time = 0f;
		while (time < 5.5f && !GameManager.GameOver) {
			time += Time.deltaTime;
			yield return null;
		}
		StartCoroutine (BreakBullet());
		yield return null;
	}

	void OnTriggerEnter(Collider col){
		StartCoroutine (BreakBullet());
	}
	private IEnumerator BreakBullet(){
		Destroy (gameObject);
		yield return null;
	}
}

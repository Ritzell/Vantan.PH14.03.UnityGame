using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {
	private float speed = 800f;

	public IEnumerator shot(){
		transform.parent = null;
		StartCoroutine (timeLimit ());
		while (!GameManager.GameOver) {
		try{
			moveForward();
		}catch{
		}
			yield return null;
		}
	}
	public void moveForward(){
		transform.Translate(Vector3.forward * Time.deltaTime * speed);
	}

	public IEnumerator  timeLimit(){
		float time = 0f;
		while (time < 5.5f && !GameManager.GameOver) {
			time += Time.deltaTime;
			yield return null;
		}
		StartCoroutine (breakBullet());
		yield return null;
	}

	void OnTriggerEnter(Collider col){
		StartCoroutine (breakBullet());
	}
	private IEnumerator breakBullet(){
		Destroy (gameObject);
		yield return null;
	}
}

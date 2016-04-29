using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (shoot());
	}
	public IEnumerator shoot(){
		float timer = 0f;
		while (!GameManager.GameOver) {
			timer += Time.deltaTime;
			if (timer >= 1) {
				shootMissile ();
				timer = 0;
			}
			yield return null;
		}
	}
	public void shootMissile(){
		missileFactory.NewMissile.transform.position = transform.position;
		missileFactory.newMissile.layer = 12;
		StartCoroutine (missileFactory.newMissile.GetComponent<missile> ().straight (GameObject.Find("parts").transform));
	}
}

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
		missileFactory script = GameObject.Find ("GameManager").GetComponent<missileFactory> ();
		script.NewMissileE.transform.position = transform.position;
		script.newMissileE.layer = 12;
		script.newMissileE.transform.localScale = new Vector3 (100, 100, 100);
		StartCoroutine (script.newMissileE.GetComponent<missile> ().straight (GameObject.Find("eurofighter").transform));
	}
}

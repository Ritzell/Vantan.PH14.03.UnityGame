using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {
	public missileFactory script;
	// Use this for initialization
	void Start () {
		script = GameObject.Find ("GameManager").GetComponent<missileFactory> ();
		StartCoroutine (shoot());
	}
	public IEnumerator shoot(){
		float timer = 0f;
		while (!GameManager.GameOver) {
			timer += Time.deltaTime;
			if (timer >= 1) {
				choseAction ();
				timer = 0;
			}
			yield return null;
		}
	}

	public void choseAction(){
		if (Random.value > 0.5f) {
			shootMissile_T ();
		} else {
			shootMissile_T ();
		}
	}

	public void shootMissile_S(){
		missileConfig ();
		StartCoroutine (script.newMissileE.GetComponent<missile> ().straight (GameObject.Find("eurofighter").transform));
	}

	public void shootMissile_T(){
		missileConfig ();
		StartCoroutine (script.newMissileE.GetComponent<missile> ().Tracking_E (GameObject.Find("eurofighter").transform));
	}

	public void missileConfig(){
		script.NewMissileE.transform.position = transform.position;
		script.newMissileE.layer = 12;
		script.newMissileE.transform.localScale = new Vector3 (100, 100, 100);
	}
}

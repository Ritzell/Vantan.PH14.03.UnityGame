using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
	[SerializeField]
	private MissileFactory Factory;

	private Transform Player;

	void Awake ()
	{
		Player = GameObject.Find ("eurofighter").transform;
		Factory = GameObject.Find ("GameManager").GetComponent<MissileFactory> ();
	}

	void Start(){
		StartCoroutine (Shoot ());
	}

	public IEnumerator Shoot ()
	{
		float timer = 0f;
		const float delay = 2f;
		while (!GameManager.GameOver) {
			timer += Time.deltaTime;
			if (timer >= delay) {
				ChooseAction ();
				timer = 0;
			}
			yield return null;
		}
	}

	public void ChooseAction ()
	{
		if (Random.value > 0.5f) {
			TrackingMissile ();
		} else {
			TrackingMissile ();
		}
	}

	public void StraightMissile ()
	{
		StartCoroutine (Factory.NewMissileE (transform.position).GetComponent<Missile> ().StraightEnemy (Player,false));
	}

	public void TrackingMissile ()
	{
		StartCoroutine (Factory.NewMissileE (transform.position).GetComponent<Missile> ().TrackingEnemy (Player));
	}
}

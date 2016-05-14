using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
	[SerializeField]
	private MissileFactory Factory;

	private Transform Player;

	void Start ()
	{
		StartCoroutine (Shoot ());
		Player = GameObject.Find ("eurofighter").transform;
		Factory = GameObject.Find ("GameManager").GetComponent<MissileFactory> ();
	}

	public IEnumerator Shoot ()
	{
		float timer = 0f;
		while (!GameManager.GameOver) {
			timer += Time.deltaTime;
			if (timer >= 1) {
//				ChooseAction ();
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
		StartCoroutine (Factory.NewMissileE (transform.position).GetComponent<Missile> ().Straight (Player));
	}

	public void TrackingMissile ()
	{
		StartCoroutine (Factory.NewMissileE (transform.position).GetComponent<Missile> ().Tracking_E (Player));
	}
}

using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
	[SerializeField]
	private MissileFactory Factory;
	private IEnumerator Tracking;
	private IEnumerator Straight;
	private Transform Player;

	void Awake ()
	{
		Player = GameObject.Find ("eurofighter").transform;
		Factory = GameObject.Find ("GameManager").GetComponent<MissileFactory> ();
	}

	void Start(){
		StartCoroutine (Shoot ());
		StartCoroutine (SpecialAttack ());
	}

	private IEnumerator SpecialAttack(){
		yield return new WaitForSeconds (10);
		while(GameManager.RestTime.Seconds > 30){
			yield return null;
		}
		StartCoroutine (OmniDirectionAttack());
		yield return null;
	}

	private IEnumerator Shoot ()
	{
		float timer = 0f;
		const float delay = 1f;
//		StartCoroutine (OmniDirectionAttack());
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

	private void StraightMissile ()
	{
		StartCoroutine (Factory.NewMissileE (transform.position).GetComponent<Missile> ().StraightToTgt (Player,false));
	}

	private void StraightMissile (Vector3 Rot)
	{
		StartCoroutine (Factory.NewMissileE (transform.position,Rot).GetComponent<Missile> ().StraightToTgt (false));
	}

	private void TrackingMissile ()
	{
		StartCoroutine (Factory.NewMissileE (transform.position).GetComponent<Missile> ().TrackingEnemy (Player));
	}

	private IEnumerator OmniDirectionAttack(){
		float XAngle = 0f;
		float YAngle = 0f;

		while(XAngle < 91){
			while (YAngle < 361) {
				StraightMissile(new Vector3 (XAngle-45, YAngle, 0));
				YAngle += 30;
				yield return new WaitForSeconds(0.2f);
			}
			YAngle = 0;
			XAngle += 20;
			yield return null;
		}
		yield return null;
	}
}

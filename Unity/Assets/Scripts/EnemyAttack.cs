using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAttack : MonoBehaviour
{
	


	[SerializeField]
	private MissileFactory Factory;

	private IEnumerator Tracking;
	private IEnumerator Straight;
	private Transform Player;

	private Transform _target;
	public Transform Target {
		set {
			_target = value;
		}get {
			return _target;
		}
	}

	void Awake ()
	{
		Player = GameObject.Find ("AirPlain").transform;
		Factory = GameObject.Find ("GameManager").GetComponent<MissileFactory> ();
	}

	void Start(){
		StartCoroutine (Shoot ());
//		StartCoroutine (SpecialAttack ());
		Target = Player;
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
		const float delay = 1.5f;
//		StartCoroutine (OmniDirectionAttack());
		while (!GameManager.GameOver ) {
			timer += Time.deltaTime;
			if (isShoot(timer,delay)) {
				//Debug.Log (GameManager.EnemyMissiles);
				ChooseAction ();
				timer = 0;
			}
			yield return null;
		}
	}

	private bool isShoot(float timer, float delay){
		return timer >= delay && GameManager.EnemyMissiles <= 80;
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
		GameObject Missile = Factory.NewEnemyMissile (transform.position);
		Missile.GetComponent<MissileSystem>().StartCoroutine (Missile.GetComponent<MissileSystem> ().Straight (Target,false));
	}

	private void StraightMissile (Vector3 Rot)
	{
		GameObject Missile = Factory.NewEnemyMissile (transform.position,Rot);
		Missile.GetComponent<MissileSystem>().StartCoroutine (Missile.GetComponent<MissileSystem> ().Straight (Target));
	}

	private void TrackingMissile ()
	{
		GameObject Missile = Factory.NewEnemyMissile (transform.position);
		Missile.GetComponent<MissileSystem>().StartCoroutine (Missile.GetComponent<MissileSystem> ().TrackingForPlayer (Target));
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

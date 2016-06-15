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
		Target = GameObject.Find ("eurofighter").transform;
		Factory = GameObject.Find ("GameManager").GetComponent<MissileFactory> ();

	}

	public void Start(){
//		StartCoroutine (Attack ());
//		StartCoroutine (SpecialAttack ());
//		Target = Player;
	}


//	private IEnumerator SpecialAttack(){
//		yield return new WaitForSeconds (10);
//		while(GameManager.RestTime.Seconds > 30){
//			yield return null;
//		}
//		StartCoroutine (OmniDirectionAttack());
//		yield return null;
//	}

	public IEnumerator Attack ()
	{
		float delay = 3f;
		while (!GameManager.GameOver ) {
			yield return new WaitForSeconds (delay);
			while (!isShoot()) {
				yield return null;
			}
			ChooseAction ();
			yield return null;
		}
	}

	private bool isShoot(){
		return GameManager.EnemyMissiles <= 80;
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

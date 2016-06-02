using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissileSystem : MonoBehaviour
{
	[SerializeField]
	private AudioClip AudioClip1;
	[SerializeField]
	private AudioClip AudioClip2;

	private AudioSource AudioS;
	private float Speed;
	private float LifeTime = 40;
	private static Airframe AirFrame;
	private readonly Vector3 MissilePosA = new Vector3 (8.5f,-0.6f,-8);
	private readonly Vector3 MissilePosB = new Vector3 (5,-0.6f,-5.35f);
	private readonly Vector3 MissilePosC = new Vector3 (-5f,-0.6f,-5.35f);
	private readonly Vector3 MissilePosD = new Vector3 (-8.5f,-0.6f,-8);



	private Vector3 _startPos;
	public Vector3 StartPos{
		set{
			if (value == MissilePosA) {
				StartCoroutine (MissileUI.TurningOn (0));
			} else if(value == MissilePosB){
				StartCoroutine (MissileUI.TurningOn (1));
			}else if(value == MissilePosC){
				StartCoroutine (MissileUI.TurningOn (2));
			}else if(value == MissilePosD){
				StartCoroutine (MissileUI.TurningOn (3));
			}
			_startPos = value;
		}get{
			if (_startPos == MissilePosA) {
				StartCoroutine (MissileUI.TurningOff (0));
			} else if(_startPos == MissilePosB){
				StartCoroutine (MissileUI.TurningOff (1));
			}else if(_startPos == MissilePosC){
				StartCoroutine (MissileUI.TurningOff (2));
			}else if(_startPos == MissilePosD){
				StartCoroutine (MissileUI.TurningOff (3));
			}
			return _startPos;
		}
	}
	private Quaternion _startRot;
	public Quaternion StartRot{
		set{
			_startRot = value;
		}get{
			return _startRot;
		}
	}
	private static MissileFactory Factory;
	private static ReticleSystem Reticle;


	void Awake ()
	{
		AirFrame = GameObject.Find ("eurofighter").GetComponent<Airframe> ();
		AudioS = gameObject.GetComponent<AudioSource> ();
		GameManager.EnemyMissiles = 1;
		Factory = GameObject.Find ("GameManager").GetComponent<MissileFactory> ();
		Reticle = GameObject.Find ("ReticleImage").GetComponent<ReticleSystem> ();


	}

	void Start ()
	{
		if (gameObject.layer == 12) {
			
			ReticleSystem.AddMissiles.Add (gameObject);
			Speed = 700;//700
			MissileRader.AddOutRangeMissile.Add (gameObject.transform);
		} else {
			Speed = 850;
		}
		AudioS.clip = AudioClip1;
		StartPos = transform.localPosition;
		StartRot = transform.localRotation;
	}

	public IEnumerator StraightToTgt (Transform tgt, bool isPlayer)
	{
		if (isPlayer) {
			ShootReady (true);
		} else {
			StartCoroutine (SelfBreak ());
		}
		transform.LookAt (tgt);

		while (true) {
			StartCoroutine (MoveForward ());
			yield return null;
		}
	}

	public IEnumerator StraightToTgt (bool isPlayer)
	{
		if (isPlayer) {
			ShootReady (true);
		} else {
			StartCoroutine (SelfBreak ());
		}
		while (true) {
			yield return StartCoroutine (MoveForward ());
		}
	}

	public IEnumerator TrackingForEnemy (Transform tgt,bool isReload)
	{
		ShootReady (isReload);
		while (true) {
			yield return StartCoroutine (GetAimingPlayer (tgt));
			yield return StartCoroutine (MoveForward ());
//			yield return null;
		}
	}

	public IEnumerator TrackingForPlayer (Transform tgt)
	{
		StartCoroutine (SelfBreak ());
		while (true) {
			Vector3 RandomError = new Vector3 (Random.Range (-10, 10), Random.Range (-10, 10), Random.Range (-10, 10));
			while (Distance (tgt) >= 40) {
				yield return StartCoroutine (ErrorTracking (tgt, RandomError));
			}
			while (true) {
				yield return StartCoroutine (MoveForward ());
			}
		}
	}

	public IEnumerator MultipleMissileInterceptShoot ()
	{
		Speed = 850;
		StartCoroutine (TrackingForEnemy (ReticleSystem.MultiMissileLockOn.Dequeue ().transform,false));
		yield return null;
		if (ReticleSystem.MultiMissileLockOn.Count > 0) {
			StartCoroutine (MakeAvater ());
		} else {
			StartCoroutine (Reticle.ChangeMode (false));
		}
		yield return null;
	}

	private IEnumerator MakeAvater ()
	{
		GameObject NewMissile = Factory.NewPlayerMissile (transform.position, transform.rotation, false);
		var MissileScript = NewMissile.GetComponent<MissileSystem> ();
		MissileScript.StartCoroutine (MissileScript.MultipleMissileInterceptShoot ());
		yield return MissileScript.StartCoroutine (MissileScript.Leave ());
	}
	private IEnumerator Leave(){
		Vector2 Dis = new Vector2 (Random.value <= 0.5f ? -10 : 10, Random.value <= 0.5f ? -10 : 10);
		float time = 0f;

		while (time < 0.1f) {
			time += Time.deltaTime;
			transform.Translate (Dis.x*(Time.deltaTime*10),Dis.y*(Time.deltaTime*10),0);
			yield return null;
		}
		yield return null;
	}

	private IEnumerator ErrorTracking (Transform tgt, Vector3 Random3)
	{
		transform.LookAt (tgt.transform.position + Random3);
		yield return StartCoroutine (MoveForward ());
	}

	private void RefreshSelfBreak ()
	{
		StopCoroutine (SelfBreak ());
		LifeTime = 4;
		StartCoroutine (SelfBreak ());
	}

	private float Distance (Transform tgt)
	{
		return Mathf.Abs (Vector3.Distance (tgt.position, transform.position));
	}

	private void ShootReady (bool isReload)
	{
		if (isReload) {
			AirFrame.Reload (StartPos, StartRot);
		}
		transform.parent = null;
		AudioS.Play ();
		StartCoroutine (SelfBreak ());
		transform.FindChild ("Steam").gameObject.SetActive (true);
		transform.FindChild ("Afterburner").gameObject.SetActive (true);
	}

	private IEnumerator GetAimingPlayer (Transform tgt)
	{
		Vector3 TgtPos = new Vector3 (tgt.transform.position.x, tgt.transform.position.y, tgt.transform.position.z);
		transform.LookAt (TgtPos);
		yield return null;
	}

	//	private Vector3 GetAimingEnemy (Transform tgt)
	//	{
	//
	////		try{
	////			Vector3 Distance = new Vector3 ((tgt.transform.position.x+ Random.Range (-40, 40)) - transform.position.x,(tgt.transform.position.y+ Random.Range (-40, 40)) - transform.position.y, (tgt.transform.position.z+ Random.Range (-40, 40)) - transform.position.z);
	////			Vector3 TgtPos = new Vector3 (tgt.position.x - Distance.x/2,tgt.position.y - Distance.y/2,tgt.position.z - Distance.z/2);
	////		transform.LookAt (TgtPos);
	////			return TgtPos;
	////		}catch{
	////			return Vector3.zero;
	////		}
	//		Vector3 a;
	//		return Vector3;
	//	}
	//
	private IEnumerator MoveForward ()
	{
		transform.Translate (Vector3.forward * Time.deltaTime * Speed);
		yield return null;
	}

	void OnTriggerEnter (Collider col)
	{
		if (transform.parent != null) {
			return;
		}
		StartCoroutine (BreakMissile ());
	}

	private IEnumerator SelfBreak ()
	{
		float time = 0f;
		while (time < LifeTime) {
			time += Time.deltaTime;
			yield return null;
		}
		StartCoroutine (BreakMissile ());
		yield return null;
	}

	private IEnumerator BreakMissile ()
	{
		StopAllCoroutines ();
		Instantiate (Resources.Load ("prefabs/Explosion"), transform.position, Quaternion.identity);
		MissileRader.DestroyMissile (gameObject.transform);
		EstimationSystem.RemoveList (gameObject);
		GameManager.EnemyMissiles = -1;
		StartCoroutine (Deth ());
		yield return null;
	}

	private IEnumerator Deth ()
	{
		yield return new WaitForSeconds (0.5f);
		Destroy (gameObject);
		yield return null;
	}
}


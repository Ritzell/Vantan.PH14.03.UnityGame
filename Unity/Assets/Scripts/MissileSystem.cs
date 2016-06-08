using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissileSystem : MonoBehaviour
{
	[SerializeField]
	private AudioClip AudioClip1;
	[SerializeField]
	private AudioClip AudioClip2;
	[SerializeField]
	private AudioClip AudioClip3;

	private AudioSource AudioS;
	private float Speed;
	private float LifeTime = 40;
	private static Airframe AirFrame;

	private Vector3 _startPos;
	public Vector3 StartPos{
		set{
			_startPos = value;
		}get{
			StartCoroutine (LightingControlSystem.TurningOff (UIType.Missile));
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
			AudioS.volume = 0.65f;
			AudioS.spatialBlend = 1;
			AudioS.maxDistance = 300f;
			AudioS.clip = AudioClip3;
			AudioS.loop = true;
			AudioS.Play ();
			ReticleSystem.AddMissiles.Add (gameObject);
			Speed = 700;//700
			MissileRader.AddOutRangeMissile.Add (gameObject.transform);
		} else {
			Speed = 850;
			AudioS.clip = AudioClip1;
		}
		StartPos = transform.localPosition;
		StartRot = transform.localRotation;
	}

	public IEnumerator Straight (Transform tgt, bool isPlayer)
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

	public IEnumerator Straight (bool isPlayer)
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
		while (tgt != null) {
			try{
			StartCoroutine (GetAimingPlayer (tgt));
			}catch{
			}
			StartCoroutine (MoveForward());
			yield return null;
		}
		while (true) {
			StartCoroutine (MoveForward());
			yield return null;
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
		try{
		StartCoroutine (TrackingForEnemy (ReticleSystem.MultiMissileLockOn[0].transform,false));
		ReticleSystem.MultiMissileLockOn.RemoveAt (0);
		}catch{
			ReticleSystem.MultiMissileLockOn.RemoveAt (0);
			StartCoroutine (Straight (true));
		}
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
			StartCoroutine(AirFrame.Reload (StartPos, StartRot));
		}
		transform.parent = null;
		AudioS.Play ();
		StartCoroutine (SelfBreak ());
		transform.FindChild ("Steam").gameObject.SetActive (true);
		transform.FindChild ("Afterburner").gameObject.SetActive (true);
	}

	private IEnumerator GetAimingPlayer (Transform tgt)
	{
		try{
			Vector3 TgtPos = new Vector3 (tgt.transform.position.x, tgt.transform.position.y, tgt.transform.position.z);
			transform.LookAt (TgtPos);
		}catch{
		}
		yield return null;
	}

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
		GameManager.EnemyMissiles = -1;
		StartCoroutine (Deth ());
		yield return null;
	}

	private IEnumerator Deth ()
	{
		if (gameObject.layer == 12) {
			ReticleSystem.RemoveMissiles.Add (gameObject);
			try{
				ReticleSystem.MultiMissileLockOn.Remove (gameObject);
			}catch{
			}
		}
		yield return new WaitForSeconds (0.5f);
		StopAllCoroutines ();
		Destroy (gameObject);
		yield return null;
	}
}


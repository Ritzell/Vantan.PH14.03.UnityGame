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
	[SerializeField]
	private float ChangeModeDistance = 100f;
	[SerializeField]
	private Vector2 RandomRange = new Vector2 (-10, 10);

	private AudioSource AudioS;
	private float Speed;
	private float LifeTime = 40;
	private static Airframe AirFrame;
	private static LightingControlSystem LightingSystem;
	private static Attack PlayerAttackSystem;
	private Vector3 _startPos;

	public Vector3 StartPos {
		set {
			_startPos = value;
		}get {
			LightingSystem.TurningOff (UIType.Missile);
			return _startPos;
		}
	}

	private Quaternion _startRot;

	public Quaternion StartRot {
		set {
			_startRot = value;
		}get {
			return _startRot;
		}
	}

	private static MissileFactory Factory;
	private static ReticleSystem Reticle;
	private Coroutine selfBrake;

	void Awake()
	{
		
		AirFrame = GameObject.FindObjectOfType<Airframe> ();
		Factory = GameObject.FindObjectOfType<MissileFactory> ();
		Reticle = GameObject.FindObjectOfType<ReticleSystem> ();
		LightingSystem = FindObjectOfType<LightingControlSystem> ();
		PlayerAttackSystem = FindObjectOfType<Attack> ();
		AudioS = gameObject.GetComponent<AudioSource> ();
	}

	void Start ()
	{

		if (gameObject.layer == 12) {
			AudioS.volume = 0.5f;
			AudioS.spatialBlend = 1;
			AudioS.maxDistance = 250f;
			GameManager.EnemyMissiles = 1;
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
			selfBrake = StartCoroutine (SelfBreak ());
		}
		while (true) {
			yield return StartCoroutine (MoveForward ());
		}
	}

	public IEnumerator TrackingForEnemy (Transform tgt, bool isReload)
	{
		ShootReady (isReload);
		while (tgt != null) {
			try {
				StartCoroutine (GetAimingPlayer (tgt));
				StartCoroutine (MoveForward ());
			} catch {
			}
			yield return null;
		}
		while (gameObject != null) {
			StartCoroutine (MoveForward ());
			yield return null;
		}
	}

	public IEnumerator TrackingForPlayer (Transform tgt)
	{
		selfBrake = StartCoroutine (SelfBreak ());
		while (true) {
			Vector3 RandomError = new Vector3 (Random.Range (RandomRange.x, RandomRange.y), Random.Range (RandomRange.x, RandomRange.y), Random.Range (RandomRange.x, RandomRange.y));
			while (tgt != null && Distance (tgt) >= ChangeModeDistance) {
				yield return StartCoroutine (ErrorTracking (tgt, RandomError));
			}
			RefreshSelfBreak ();
			while (true) {
				yield return StartCoroutine (MoveForward ());
			}
		}
	}

	public IEnumerator MultipleMissileInterceptShoot ()
	{
		if (ReticleSystem.MultiMissileLockOn [0] != null) {
			StartCoroutine (TrackingForEnemy (ReticleSystem.MultiMissileLockOn [0].transform, false));
			ReticleSystem.MultiMissileLockOn.RemoveAt (0);
		} else {
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

	private IEnumerator Leave ()
	{
		Vector2 Dis = new Vector2 (Random.value <= 0.5f ? -10 : 10, Random.value <= 0.5f ? -10 : 10);
		for (float time = 0f; time < 0.1f; time += Time.deltaTime) {
			transform.Translate (Dis.x * (Time.deltaTime * 10), Dis.y * (Time.deltaTime * 10), 0);
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
		StopCoroutine (selfBrake);
		LifeTime = 10;
		StartCoroutine (SelfBreak ());
	}

	private float Distance (Transform tgt)
	{
		return Mathf.Abs (Vector3.Distance (tgt.position, transform.position));
	}

	private void ShootReady (bool isReload)
	{
		if (isReload) {
			PlayerAttackSystem.StartCoroutine (AirFrame.Reload (StartPos, StartRot));
		}
		transform.parent = null;
		AudioS.Play ();
		selfBrake = StartCoroutine (SelfBreak ());
		transform.FindChild ("Steam").gameObject.SetActive (true);
		transform.FindChild ("Afterburner").gameObject.SetActive (true);
	}

	private IEnumerator GetAimingPlayer (Transform tgt)
	{
		try {
			Vector3 TgtPos = new Vector3 (tgt.transform.position.x, tgt.transform.position.y, tgt.transform.position.z);
			transform.LookAt (TgtPos);
		} catch {
		}
		yield return null;
	}

	private IEnumerator MoveForward ()
	{
		transform.Translate (Vector3.forward * Time.deltaTime * Speed);
		yield return null;
	}

	private bool isDeth = false;
	void OnTriggerStay (Collider col)
	{
		if (transform.parent != null || isDeth) {
			return;
		}
		StartCoroutine (BreakMissile ());
		if (gameObject.layer == 8 && (col.gameObject.layer == 11 || col.gameObject.layer == 15)) {
			GameObject.Find("MissileNotification").GetComponent<NotificationSystem>().StartCoroutine(NotificationSystem.UpdateMissileNotification ());
		}
		isDeth = true;
	}

	private IEnumerator SelfBreak ()
	{
		yield return new WaitForSeconds (LifeTime);
		StartCoroutine (BreakMissile ());
		yield return null;
	}

	private IEnumerator BreakMissile ()
	{
		StopAllCoroutines ();
		if (gameObject.layer == 12) {
			ReticleSystem.RemoveMissiles.Add (gameObject);
			try {
				ReticleSystem.MultiMissileLockOn.Remove (gameObject);
			} catch {
			}
		}
		Instantiate (Resources.Load ("prefabs/Explosion"), transform.position, Quaternion.identity);
		MissileRader.DestroyMissile (gameObject.transform);
		GameManager.EnemyMissiles = -1;
		StartCoroutine (Deth ());
		yield return null;
	}

	private IEnumerator Deth ()
	{
		
		yield return new WaitForSeconds (0.5f);
		StopAllCoroutines ();
		Destroy (gameObject);
		yield return null;
	}
}


using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class ReticleSystem : MonoBehaviour
{
	//Corgi(8)レイヤーとだけ衝突する
	//int layerMask = 1 << 8;
	//Corgi(8)、Shiba(9)レイヤーとだけ衝突する
	//int layerMask = 1 << 8 | 1 << 9;
	//Corgi(8)レイヤーとだけ衝突しない
	//int layerMask = ~(1 << 8);
	private static AudioSource AudioBox;
	private static RectTransform UITransform;
	private static List<GameObject> Missiles = new List<GameObject> ();
	private static AudioClip LockOnSE;


	private static List<GameObject> _addMissiles = new List<GameObject> ();
	public static List<GameObject> AddMissiles {
		get {
			return _addMissiles;
		}
	}

	private static List<GameObject> _removeMissiles = new List<GameObject> ();
	public static List<GameObject> RemoveMissiles {
		get {
			return _removeMissiles;
		}
	}

	private static List<GameObject> _multiMissileLockOn = new List<GameObject> ();
	public static List<GameObject> MultiMissileLockOn {
		get {
			return _multiMissileLockOn;
		}
	}

	private static GameObject lockOnTgt = null;
	public static GameObject LockOnTgt {
		set {
			lockOnTgt = value;
			if (value == null) {
				UITransform.localPosition = new Vector3 (0, 0, 0);
				AudioBox.pitch = 0.75f;
				AudioBox.PlayOneShot (LockOnSE);
				UI.color = Color.green;
			} else {
				UI.color = Color.red;
				AudioBox.pitch = 1f;
				AudioBox.PlayOneShot (LockOnSE);
			}
			UI.GetComponent<ReticleSystem> ().ChangeCoroutine (value);
		}get {
			return lockOnTgt;
		}
	}

	private static Image UI;
	public static Image UIImage {
		get {
			return UI;
		}
	}

	[SerializeField]
	private Camera MainCamera;

	[SerializeField]
	private GameObject ReticleUI;

	[SerializeField]
	private GameObject MuzzleA;

	[SerializeField]
	private GameObject MuzzleB;

	[SerializeField]
	private GameObject MultipleReticleObject;

	void Awake ()
	{
		UI = gameObject.GetComponent<Image> ();
		AudioBox = GetComponent<AudioSource> ();
		UITransform = GetComponent<RectTransform> ();
		LockOnSE = (AudioClip)Instantiate (Resources.Load ("Sounds/LockOnSE"));
	}

	void Start ()
	{
		UI.color = Color.green;
	}

	public void EnableReticle(){
        if (VRMode.isVRMode)
        {
            return;
        }
	    StartCoroutine (SerchEnemy ());
		StartCoroutine (ReticleMoveInput ());
	}

	private IEnumerator ReticleMoveInput ()
	{
		while (!GameManager.IsGameOver) {
			ReticleController (new Vector3 (Input.GetAxis ("3thAxis"), Input.GetAxis ("4thAxis"), 0) == Vector3.zero ? new Vector3 (Input.GetAxis ("Key3thAxis"), Input.GetAxis ("Key4thAxis"), 0) : new Vector3 (Input.GetAxis ("3thAxis"), Input.GetAxis ("4thAxis"), 0));
			yield return null;
		}
		yield return null;
	}


	private IEnumerator SerchEnemy ()
	{
		
		RaycastHit Hit;
		const int LayerMask = 1 << (int)Layers.Enemy | 1 << (int)Layers.EnemyMissile | 1 << (int)Layers.EnemyArmor;
		 
		while (!GameManager.IsGameOver) {
			if (CameraSystem.FreeMove) {
				yield return null;
				continue;
			}
			var ray = Camera.main.ScreenPointToRay (new Vector3 (transform.position.x, transform.position.y, 0.0f));

			if (Physics.Raycast (ray, out Hit, 30000, LayerMask)) {
				StartCoroutine (Gun.MuzzuleLookTgt (Hit.transform.position));
				SelectTgt (Hit.transform.gameObject);
			} else {
				StartCoroutine (Gun.MuzzuleLookTgt (ray.GetPoint (4000)));/////////////////
				FadeCancel ();
			}
			yield return null;
		}
	}

	/// <summary>
	/// ロックを外す
	/// </summary>
	/// <returns>The lock input.</returns>
	private IEnumerator ReleaseLockInput ()
	{
		while (!GameManager.IsGameOver) {
			if (Attack.isCancel()) {
				LockOnTgt = null;
			}
			yield return null;
		}
	}

	private IEnumerator ReticleMoveToTgt ()
	{
		StartCoroutine (ForciblyRelaseLock ());
		while (!GameManager.IsGameOver) {
			var ray = Camera.main.ScreenPointToRay (new Vector3 (transform.position.x, transform.position.y, 0.0f));
			StartCoroutine (Gun.MuzzuleLookTgt (ray.GetPoint (4000)));
			UITransform.position = RectTransformUtility.WorldToScreenPoint (Camera.main, lockOnTgt.transform.position);
			yield return null;
		}
	}

	private IEnumerator ForciblyRelaseLock ()
	{
		while (lockOnTgt != null && !GameManager.IsGameOver) {
			if (ReticleIsOutOfScreen ()) {
				LockOnTgt = null;
				yield return null;
			}
			yield return null;
		}
		yield return null;
	}

	private bool ReticleIsOutOfScreen ()
	{
		if (UITransform.position.x < 0 || UITransform.position.x > Screen.width || UITransform.position.y < 0 || UITransform.position.y > Screen.height) {
			return true;
		}
		return false;
	}

	public void DestoroyLockOnTgt (GameObject Enemy)
	{
		if (LockOnTgt == Enemy) {
			AudioBox.pitch = 1.75f;
			AudioBox.PlayOneShot (LockOnSE);
			LockOnTgt = null;
		}
		return;
	}

	public void SelectTgt (GameObject TgtOb)
	{
		ReticleColorFade (Color.red);
		LockNow (TgtOb);
	}

	public void FadeCancel ()
	{
		if (UI.color.g < 1) {
			ReticleColorFade (Color.green);
		}
	}

	private void LockNow (GameObject Tgt)
	{
		if (UI.color.r >= 1) {
			LockOnTgt = Tgt;
			Debug.Log (LockOnTgt.name + " をロックオン!");
		}
	}

	private void ReticleController (Vector3 Dis)
	{
		if (ReticleErrorScan (Dis)) {
			ReticleMove (Dis);
		}
	}

	/// <summary>
	/// レティクルが一定範囲の外に向かうならFalse
	/// </summary>
	/// <param name="Dis">Dis.</param>
	private bool ReticleErrorScan (Vector3 Dis)
	{
		const float LimitDistance = 100;
		if (Mathf.Abs (ReticleDistance (Dis)) <= LimitDistance) {
			return true;
		} else {
			return false;
		}
	}


	/// <summary>
	/// レティクルの座標を調べる
	/// </summary>
	/// <returns>The distance.</returns>
	/// <param name="Dis">Dis.</param>
	private float ReticleDistance (Vector3 Dis)
	{
		return Vector2.Distance (new Vector2 (transform.position.x + Dis.x, transform.position.y - Dis.y),
			new Vector2 (Screen.width / 2, Screen.height / 2));
	}

	/// <summary>
	/// レティクルを動かす
	/// </summary>
	/// <param name="Dis">Dis.</param>
	private void ReticleMove (Vector3 Dis)
	{
		if (CameraSystem.FreeMove) {
			return;
		}
		transform.Translate (Dis.x, -Dis.y, 0);
		if (transform.position.x > Screen.width || transform.position.x < 0
		    || transform.position.y > Screen.height || transform.position.y < 0) {

		}

	}

	/// <summary>
	/// 使うコルーチンを変える
	/// </summary>
	/// <param name="LockOn">If set to <c>true</c> lock on.</param>
	private void ChangeCoroutine (bool LockOn)
	{
		if (LockOn) {
			StopAllCoroutines ();
			StartCoroutine (ReleaseLockInput ());
			StartCoroutine (ReticleMoveToTgt ());
		} else {
			StopAllCoroutines ();
			StartCoroutine (SerchEnemy ());
			StartCoroutine (ReticleMoveInput ());
		}
	}

	/// <summary>
	/// MMIモードとノーマルモードを切り替える
	/// </summary>
	/// <returns>The mode.</returns>
	/// <param name="isMMI">If set to <c>true</c> is MM.</param>
	public IEnumerator ChangeMode (bool isMMI)
	{
		UI.color = Color.green;
		if (isMMI) {
			StopAllCoroutines ();
			StartCoroutine (ReticleScaleUp ());
			StartCoroutine (MultipleLockOnSystem ());
		} else {
			StartCoroutine (MultipleReticle.AllReleaceReticle());
			yield return StartCoroutine (ReticleScaleDown ());
			ChangeCoroutine(false);

		}
		yield return null;
	}

	public void MMIReady ()
	{
		StopAllCoroutines ();
		Attack.MMIisReady = true;
		Attack.Reloading = 0;
		UI.color = Color.red;

	}

	/// <summary>
	/// 敵のミサイルの全てを対象にロックオン判定を行う
	/// </summary>
	/// <returns>The lock on system.</returns>
	private IEnumerator MultipleLockOnSystem ()
	{
		while (!GameManager.IsGameOver) {
			yield return StartCoroutine (RemoveMissile());
			foreach (GameObject Missile in Missiles) {
				Vector3 ScreenMissilePos;
				try{
					ScreenMissilePos = Camera.main.WorldToScreenPoint (Missile.transform.position);
				}catch{
					continue;
				}
				if (MissileInReticle (ScreenMissilePos)) {
					MultipleLockOnSetting (Missile);
					yield return null;
				}
				yield return null;
			}
			Missiles.AddRange (AddMissiles);
			yield return null;
		}
	}

	private IEnumerator RemoveMissile(){
		MultiMissileLockOn.ForEach (Missile => Missiles.Remove(Missile));
		RemoveMissiles.ForEach (Missile => Missiles.Remove (Missile));
		yield return null;
	}

	private void MultipleLockOnSetting(GameObject Missile){
		MultiMissileLockOn.Add (Missile);
		var newReticle = Instantiate (MultipleReticleObject);
		newReticle.GetComponent<MultipleReticle> ().LockOn = Missile;
		AudioBox.pitch = 1;
		AudioBox.PlayOneShot (LockOnSE);
	}

	private bool MissileInReticle (Vector3 MissilePos)
	{
		if (Vector2.Distance (new Vector2 (UITransform.position.x, UITransform.position.y), new Vector2 (MissilePos.x, MissilePos.y)) < (UITransform.lossyScale.x * 100) / 2) {
			return true;
		} else {
			return false;
		}
	}

	private IEnumerator ReticleScaleUp ()
	{
		while (!GameManager.IsGameOver) {
			float time = Time.deltaTime / 3;
			UITransform.localScale = new Vector3 (UITransform.localScale.x + time, UITransform.localScale.y + time, 0);
			yield return null;
		}
	}

	private IEnumerator ReticleScaleDown ()
	{
		while (!GameManager.IsGameOver && UITransform.localScale.x > 0.2f) {
			float time = Time.deltaTime*5;///1000;
			UITransform.localScale = new Vector3 (Mathf.Clamp (UITransform.localScale.x - time, 0.18f, 100), Mathf.Clamp (UITransform.localScale.y - time, 0.18f, 100), 0);
			yield return null;
		}
		UITransform.localScale = new Vector3 (0.2f, 0.2f, 0);
			yield return null;
	}



	private void ReticleColorFade (Color Color)
	{
		UI.color = new Vector4 (UI.color.r + (Color == Color.red ? (Time.deltaTime / 2) : (Time.deltaTime / -2)),
			UI.color.g + (Color == Color.red ? (Time.deltaTime / -2) : (Time.deltaTime / 2)), 0, 1);

	}
}


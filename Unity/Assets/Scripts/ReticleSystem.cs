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
	private GameObject MultipleReticle;

	void Awake ()
	{
		UI = gameObject.GetComponent<Image> ();
		AudioBox = GetComponent<AudioSource> ();
		UITransform = GetComponent<RectTransform> ();
		LockOnSE = (AudioClip)Instantiate (Resources.Load ("Sounds/LockOnSE"));
	}

	void Start ()
	{
		StartCoroutine (SerchEnemy ());
		StartCoroutine (ReticleMoveInput ());
		UI.color = Color.green;
	}

	private IEnumerator ReticleMoveInput ()
	{
		while (!GameManager.GameOver) {
			ReticleController (new Vector3 (Input.GetAxis ("3thAxis"), Input.GetAxis ("4thAxis"), 0));
			yield return null;
		}
		yield return null;
	}


	private IEnumerator SerchEnemy ()
	{
		
		RaycastHit Hit;
		const int LayerMask = 1 << 11 | 1 << 12;
		 
		while (!GameManager.GameOver) {
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

	private IEnumerator ReleaseLockInput ()
	{
		while (!GameManager.GameOver) {
			if ((Input.GetKeyDown (KeyCode.Alpha3) || Input.GetKeyDown (KeyCode.JoystickButton18))) {
				LockOnTgt = null;
			}
			yield return null;
		}
	}

	private IEnumerator ReticleMoveToTgt ()
	{
		StartCoroutine (ForciblyRelaseLock ());
		while (!GameManager.GameOver) {
			var ray = Camera.main.ScreenPointToRay (new Vector3 (transform.position.x, transform.position.y, 0.0f));
			StartCoroutine (Gun.MuzzuleLookTgt (ray.GetPoint (4000)));
			UITransform.position = RectTransformUtility.WorldToScreenPoint (Camera.main, lockOnTgt.transform.position);
			yield return null;
		}
	}

	private IEnumerator ForciblyRelaseLock ()
	{
		while (lockOnTgt != null && !GameManager.GameOver) {
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

	private void SelectTgt (GameObject TgtOb)
	{
		FadeToColor (Color.red);
		LockNow (TgtOb);
	}

	private void FadeCancel ()
	{
		if (UI.color.g < 1) {
			FadeToColor (Color.green);
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

	private float ReticleDistance (Vector3 Dis)
	{
		return Vector2.Distance (new Vector2 (transform.position.x + Dis.x, transform.position.y - Dis.y),
			new Vector2 (Screen.width / 2, Screen.height / 2));
	}

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

	public void ChangeMode (bool ToMMI)
	{
		StopAllCoroutines ();
		UI.color = Color.green;
		if (ToMMI) {
			StartCoroutine (ReticleScaleUp ());
			StartCoroutine (MultipleLockOnSystem ());
		} else {
			StartCoroutine (ReticleScaleDown ());
		}
	}

	public void MMIReady ()
	{
		StopAllCoroutines ();
		Attack.MMIReady = true;
		UI.color = Color.red;

	}

	private IEnumerator MultipleLockOnSystem ()
	{
		while (!GameManager.GameOver) {
			foreach (GameObject Missile in Missiles) {
				Vector3 ScreenMissilePos = Camera.main.WorldToScreenPoint (Missile.transform.position);
				if (MissileInReticle (ScreenMissilePos)) {
					MultipleLockOnSetting (Missile);
					yield return null;
				}
				yield return null;
			}
			Missiles.AddRange (AddMissiles);
			foreach (GameObject Missile in MultiMissileLockOn) {
				Missiles.Remove (Missile);
			}
			yield return null;
		}
		yield return null;
	}

	private void MultipleLockOnSetting(GameObject Missile){
		MultiMissileLockOn.Add (Missile);
		var newReticle = Instantiate (MultipleReticle);
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
		while (!GameManager.GameOver) {
			float time = Time.deltaTime / 3;
			UITransform.localScale = new Vector3 (UITransform.localScale.x + time, UITransform.localScale.y + time, 0);
			yield return null;
		}
	}

	private IEnumerator ReticleScaleDown ()
	{
		while (!GameManager.GameOver && UITransform.localScale.x != 0.2f) {
			float time = Time.deltaTime * 7f;
			UITransform.localScale = new Vector3 (Mathf.Clamp (UITransform.localScale.x - time, 0.2f, 100), Mathf.Clamp (UITransform.localScale.y - time, 0.2f, 100), 0);
			yield return null;
		}
		yield return null;
	}



	private void FadeToColor (Color Color)
	{
		UI.color = new Vector4 (UI.color.r + (Color == Color.red ? (1 * (Time.deltaTime / 2)) : (-1 * (Time.deltaTime / 2))),
			UI.color.g + (Color == Color.red ? (-1 * (Time.deltaTime / 2)) : (1 * (Time.deltaTime / 2))), 0, 1);

	}
}


using UnityEngine;
using System.Collections;



public class ReticleSystem : MonoBehaviour
{
	//Corgi(8)レイヤーとだけ衝突する
	//int layerMask = 1 << 8;
	//Corgi(8)、Shiba(9)レイヤーとだけ衝突する
	//int layerMask = 1 << 8 | 1 << 9;
	//Corgi(8)レイヤーとだけ衝突しない
	//int layerMask = ~(1 << 8);
	private static GameObject lockOnTgt = null;
	private static AudioSource AudioBox;
	[SerializeField]
	private AudioClip LockOnSE;
	private static GUITexture UI;

	public static GameObject LockOnTgt {
		set {
			lockOnTgt = value;
			if (value == null) {
				UI.color = Color.green;
			} else {
				UI.color = Color.red;
			}
		}get {
			return lockOnTgt;
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

	void Awake ()
	{
		UI = gameObject.GetComponent<GUITexture> ();
		UI.color = Color.green;
	}

	void Start ()
	{
		StartCoroutine (SerchEnemy ());
		StartCoroutine (ReleaseLock ());
		StartCoroutine (InputStick_R ());
		AudioBox = GetComponent<AudioSource> ();
	}

	private IEnumerator InputStick_R ()
	{
		while (!GameManager.GameOver) {
			ReticleController (new Vector3 (Input.GetAxis ("3thAxis"), Input.GetAxis ("4thAxis"), 0));
			yield return null;
		}
	}

	private IEnumerator SerchEnemy ()
	{
		
		RaycastHit Hit;
		int LayerMask = 1 << 11 | 1 << 12;

		while (!GameManager.GameOver) {
			if (CameraSystem.FreeMove) {
				yield return null;
				continue;
			}
			var ray = Camera.main.ScreenPointToRay (new Vector3 (((transform.position.x + (transform.localScale.x * 0.01f)) / 2) * Screen.width, ((transform.position.y + (transform.localScale.y * 0.05f)) / 2) * Screen.height, 0.0f));
			Debug.DrawRay (ray.origin, ray.direction * 30000, Color.red);
			if (Physics.Raycast (ray, out Hit, 30000, LayerMask)) {
				if (lockOnTgt == null) {
					SelectTgt (Hit.transform.gameObject);
					StartCoroutine (Gun.MuzzuleLookTgt (Hit.transform.position));
				}
			} else {
				StartCoroutine (Gun.MuzzuleLookTgt (ray.GetPoint (1000)));
				if (lockOnTgt == null && UI.color.g < 1) {
					SelectCancel ();
				}
			}
			yield return null;
		}
	}

	private void FadeToColor (Color Color)
	{
		UI.color = new Vector4 (UI.color.r + (Color == Color.red ? (1 * (Time.deltaTime / 2)) : (-1 * (Time.deltaTime / 2))),
			UI.color.g + (Color == Color.red ? (-1 * (Time.deltaTime / 2)) : (1 * (Time.deltaTime / 2))), 0, 1);

	}


	private void SelectTgt (GameObject TgtOb)
	{
		FadeToColor (Color.red);
		LockNow (TgtOb);
	}

	private void SelectCancel ()
	{
		FadeToColor (Color.green);
		if (UI.color.g >= 1) {
			LockOnTgt = null;
		}
	}

	//private float LockNow;

	private void LockNow (GameObject Tgt)
	{
		if (UI.color.r >= 1) {
			LockOnTgt = Tgt;
			AudioBox.pitch = 1;
			AudioBox.PlayOneShot (LockOnSE);
			Debug.Log (LockOnTgt.name + " をロックオン!");
		}
	}

	private IEnumerator ReleaseLock ()
	{
		while (!GameManager.GameOver) {
			if ((Input.GetKeyDown (KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.JoystickButton18)) && LockOnTgt != null) {
				AudioBox.pitch = 0.75f;
				AudioBox.PlayOneShot (LockOnSE);
				Debug.Log (lockOnTgt.name + " のロックを解除！");
				LockOnTgt = null;
			}
			yield return null;
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
		const float LimitDistance = 30;
		//if (transform.position.x + (Dis.x / Screen.width) >= 0.75f && transform.position.x + (Dis.x / Screen.width) <= 1.25f
		//   && transform.position.y + (-Dis.y / Screen.height) >= 0.75f && transform.position.y + (-Dis.y / Screen.height) <= 1.25f) 
		if (Mathf.Abs (ReticleDistance (Dis)) <= LimitDistance) {
			return true;
		} else {
			return false;
		}
	}

	private float ReticleDistance (Vector3 Dis)
	{
		return Vector2.Distance (new Vector2 ((((transform.position.x / 2) + (Dis.x / Screen.width)) * Screen.width), ((transform.position.y / 2) + (-Dis.y / Screen.height)) * Screen.height),
			new Vector2 (Screen.width / 2, Screen.height / 2));
	}

	private void ReticleMove (Vector3 Dis)
	{
		if (CameraSystem.FreeMove) {
			return;
		}
		transform.Translate (Dis.x / Screen.width, -Dis.y / Screen.height, 0);
	}
}


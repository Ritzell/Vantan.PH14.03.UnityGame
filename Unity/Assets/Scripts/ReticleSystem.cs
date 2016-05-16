using UnityEngine;
using System.Collections;
using UnityEngine.UI;



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
	private static GameObject lockOnTgt = null;
	public static GameObject LockOnTgt {
		set {
			lockOnTgt = value;
			if (value == null) {
				UITransform.localPosition = new Vector3 (0,0,0);
				UI.color = Color.green;
			} else {
				UI.color = Color.red;
			}
			UI.GetComponent<ReticleSystem> ().ChangeCoroutine(value);
		}get {
			return lockOnTgt;
		}
	}
	private static Image UI;
	public static Image UIImage{
		get{
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
	private AudioClip LockOnSE;

	void Awake ()
	{
		UI = gameObject.GetComponent<Image> ();
		AudioBox = GetComponent<AudioSource> ();
		UITransform = GetComponent<RectTransform> ();
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

	public void ReleaseLock(GameObject Enemy){
		if (LockOnTgt == Enemy) {
			AudioBox.pitch = 1.75f;
			AudioBox.PlayOneShot (LockOnSE);
			LockOnTgt = null;
		}
		return;
	}



	private IEnumerator ReleaseLockInput ()
	{
		while (!GameManager.GameOver) {
			if ((Input.GetKeyDown (KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.JoystickButton18))) {
				AudioBox.pitch = 0.75f;
				AudioBox.PlayOneShot (LockOnSE);
				Debug.Log (lockOnTgt.name + " のロックを解除！");
				LockOnTgt = null;
			}
			yield return null;
		}
	}

	private IEnumerator ReticleMoveToTgt(){
		while (!GameManager.GameOver) {
			var ray = Camera.main.ScreenPointToRay (new Vector3 (transform.position.x, transform.position.y, 0.0f));
			StartCoroutine (Gun.MuzzuleLookTgt (ray.GetPoint (4000)));
			UITransform.position = RectTransformUtility.WorldToScreenPoint (Camera.main,lockOnTgt.transform.position);
			yield return null;
		}
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
			AudioBox.pitch = 1;
			AudioBox.PlayOneShot (LockOnSE);
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
		return Vector2.Distance (new Vector2 (transform.position.x + Dis.x, transform.position.y -Dis.y),
						new Vector2 (Screen.width / 2, Screen.height / 2));
	}

	private void ReticleMove (Vector3 Dis)
	{
		if (CameraSystem.FreeMove) {
			return;
		}
		transform.Translate (Dis.x, -Dis.y, 0);

	}

	private void ChangeCoroutine(bool LockOn){
		if (LockOn) {
			StopAllCoroutines ();
			StartCoroutine (ReleaseLockInput());
			StartCoroutine (ReticleMoveToTgt());
		} else {
			StopAllCoroutines ();
			StartCoroutine (SerchEnemy ());
			StartCoroutine (ReticleMoveInput ());
		}
	}


	private void FadeToColor (Color Color)
	{
		UI.color = new Vector4 (UI.color.r + (Color == Color.red ? (1 * (Time.deltaTime / 2)) : (-1 * (Time.deltaTime / 2))),
			UI.color.g + (Color == Color.red ? (-1 * (Time.deltaTime / 2)) : (1 * (Time.deltaTime / 2))), 0, 1);

	}
}


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
	}

	private IEnumerator InputStick_R ()
	{
		while (!GameManager.GameOver) {
			ReticleMove = new Vector3 (Input.GetAxis ("3thAxis"), Input.GetAxis ("4thAxis"), 0);
			yield return null;
		}
	}

	private IEnumerator SerchEnemy ()
	{
		
		RaycastHit Hit;
		int LayerMask = 1 << 11 | 1 << 12;

		while (!GameManager.GameOver) {
			var ray = Camera.main.ScreenPointToRay (new Vector3 ((transform.position.x / 2) * Screen.width, (transform.position.y / 2) * Screen.height, 0.0f));
			if (Physics.Raycast (ray, out Hit, 30000, LayerMask)) {
				if (lockOnTgt == null) {
					SelectTgt (Hit.transform.gameObject);
					StartCoroutine (Gun.MuzzuleLookTgt (Hit.point));
				}
			} else {
				if (lockOnTgt != null || UI.color.g < 1) {
					SelectCancel ();
				}
				StartCoroutine (Gun.MuzzuleLookTgt (ray.GetPoint (3000)));
			}
			yield return null;
		}
	}

	private void FadeToColor(Color Color){
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
		if(UI.color.g >= 1){
			LockOnTgt = null;
		}
	}

	//private float LockNow;

	private void LockNow (GameObject Tgt)
	{
		if(UI.color.r >= 1){
			LockOnTgt = Tgt;
			Debug.Log (LockOnTgt + " をロックオン!");
		}
	}

	private IEnumerator ReleaseLock ()
	{
		while (!GameManager.GameOver) {
			if (Input.GetKeyDown (KeyCode.Alpha3) && LockOnTgt != null) {
				LockOnTgt = null;
				Debug.Log (lockOnTgt);
			}
			yield return null;
		}
	}

	private Vector3 ReticleMove {
		set {
			transform.Translate (value.x / Screen.width, -value.y / Screen.height, 0);
			MuzzleA.transform.Rotate (value.y * 1 / 4f, value.x / 4f, 0);
			MuzzleB.transform.Rotate (value.y * 1 / 4f, value.x / 4f, 0);
		}
	}
}


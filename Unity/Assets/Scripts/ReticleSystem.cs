using UnityEngine;
using System.Collections;



public class ReticleSystem : MonoBehaviour {
	////Corgi(8)レイヤーとだけ衝突する
	//int layerMask = 1 << 8;
	////Corgi(8)、Shiba(9)レイヤーとだけ衝突する
	//int layerMask = 1 << 8 | 1 << 9;
	////Corgi(8)レイヤーとだけ衝突しない
	//int layerMask = ~(1 << 8);
	public static GameObject LockOnTgt;

	[SerializeField]
	private Camera MainCamera;

	private float lockNow;

	[SerializeField]
	private GameObject ReticleUI;

	[SerializeField]
	private GameObject MuzzleA;

	[SerializeField]
	private GameObject MuzzleB;

	void Start () {
		StartCoroutine (SerchEnemy ());
		StartCoroutine (ReleaseLock ());
		StartCoroutine (InputStick_R());
	}

	private IEnumerator InputStick_R(){
		while (!GameManager.GameOver) {
			ReticleMove = new Vector3 (Input.GetAxis ("3thAxis"), Input.GetAxis ("4thAxis"), 0);
			//transform.Translate(Input.GetAxisRaw ("5thAxis"),Input.GetAxisRaw ("4thAxis"),0);
			yield return null;
		}
	}

	private IEnumerator SerchEnemy(){
		RaycastHit Hit;
		int LayerMask = 1 << 11 | 1 << 12;
		while (!GameManager.GameOver) {
			var ray = RectTransformUtility.ScreenPointToRay (Camera.main,new Vector3(294.0f, 157.5f, 0.0f));
			if(Physics.Raycast(ray,out Hit,30000,LayerMask) && LockOnTgt == null){
				LockNow = Hit.transform.gameObject;
			}
			yield return null;
		}
	}

	private GameObject LockNow{
		set{
			if (Time.deltaTime >= 0.4f) {
				Debug.Log (LockNow);
				lockNow = 0f;
			} else {
				lockNow += Time.deltaTime;
				LockOn (value);
			}
		}get{
			return null;
		}
	}
	private void LockOn(GameObject Tgt){
		if (lockNow >= 1.5f) {
			LockOnTgt = Tgt;
			Debug.Log (LockOnTgt + " をロックオン!");
			lockNow = 0;
		}
	}

	private IEnumerator ReleaseLock(){
		while (!GameManager.GameOver) {
			if (Input.GetKeyDown (KeyCode.Alpha1) && LockOnTgt != null) {
				LockOnTgt = null;
			}
			yield return null;
		}
	}
	private Vector3 ReticleMove {
		set {
			// 350 20 340 20
			transform.Translate (value.x, -value.y, 0);
//			if ((MainCamera.transform.localRotation.y <= 20 || MainCamera.transform.localRotation.y >= 340)  &&
//				(MainCamera.transform.localRotation.x <= 20|| MainCamera.transform.localRotation.x >= 340)) {
//				transform.Translate (value.x, value.y, 0);
//			}
			//MainCamera.transform.Rotate (value.y*-1, value.x,0);
			//MainCamera.transform.localRotation = new Quaternion (MainCamera.transform.rotation.x, MainCamera.transform.rotation.y, 0, MainCamera.transform.rotation.w);
			MuzzleA.transform.Rotate (value.y*1/4f, value.x/4f,0);
			MuzzleB.transform.Rotate (value.y*1/4f, value.x/4f,0);
			//MainCamera.transform.Rotate (value.y*-1, value.x,0);
		}
	}
}


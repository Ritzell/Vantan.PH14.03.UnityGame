using UnityEngine;
using System.Collections;

public class ArsenalCamera : MonoBehaviour {
	[SerializeField]
	private GameObject Fighter;
	// Use this for initialization
	void Start () {
		StartCoroutine (RotateAroundCamera());
		StartCoroutine (InputScroll());
	}

	private IEnumerator RotateAroundCamera(){
		while (true) {
			transform.RotateAround (Vector3.zero,Vector3.up,Time.deltaTime*10f);
			transform.LookAt (Vector3.zero);
			yield return null;
		}
	}
	private IEnumerator InputScroll(){
		while (true) {
			var scroll = Input.GetAxis("Mouse ScrollWheel");
			scroll = ScrollRegulation (scroll, 10, 1.7f);
			yield return StartCoroutine(TranslateCamera(Vector3.Distance (Vector3.zero, new Vector3 (transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + scroll)),scroll));
		}
	}

	private IEnumerator TranslateCamera(float distance,float scroll){
		if (distance > 15) {
			transform.Translate (scroll * Vector3.forward);
		}else if (distance > 14 && scroll <= 0) {
			transform.Translate (scroll * Vector3.forward);
		}
		yield return null;
	}

	/// <summary>
	/// スクロールの値をTranslate向けに調整
	/// </summary>
	/// <returns>The regulation.</returns>
	/// <param name="scroll">Scroll.</param>
	/// <param name="Speed">Speed.</param>
	/// <param name="SpeedSwiching">Speed swiching.</param>
	private float ScrollRegulation(float scroll,float Speed,float SpeedSwiching){
		if (scroll > 0 && scroll < SpeedSwiching) {
			return Speed * Time.deltaTime;
		} else if (scroll > SpeedSwiching) {
			return Speed * 2 * Time.deltaTime;
		} else if (scroll < 0 && scroll > -SpeedSwiching) {
			return -Speed * Time.deltaTime;
		} else if (scroll < -SpeedSwiching) {
			return -Speed * 2 * Time.deltaTime;
		}
		return 0;
	}
}


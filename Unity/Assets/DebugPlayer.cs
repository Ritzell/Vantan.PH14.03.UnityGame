using UnityEngine;
using System.Collections;

public class DebugPlayer : MonoBehaviour {
	private static float MoveSpeed = 200;
	// Use this for initialization
	void Start () {
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.None;
		StartCoroutine (DebugMove ());
		StartCoroutine (DebugRotate ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private IEnumerator DebugMove(){
		while (true) {
			if (Input.GetKey (KeyCode.Escape)) {
				Cursor.lockState = Cursor.lockState ==  CursorLockMode.None? CursorLockMode.Confined : CursorLockMode.None;
				Cursor.visible = Cursor.visible ? false : true ;
			}
			if (Input.GetKeyDown (KeyCode.LeftShift)) {
				MoveSpeed = MoveSpeed * 2f;
			} else if (Input.GetKeyUp (KeyCode.LeftShift)) {
				MoveSpeed = MoveSpeed / 2;
			}
			if (Input.GetKey (KeyCode.W)) {
				transform.Translate (0, 0, MoveSpeed * Time.deltaTime);
			}
			if (Input.GetKey (KeyCode.A)) {
				transform.Translate (-MoveSpeed * Time.deltaTime, 0, 0);
			}
			if (Input.GetKey (KeyCode.S)) {
				transform.Translate (0, 0, -MoveSpeed * Time.deltaTime);
			}
			if (Input.GetKey (KeyCode.D)) {
				transform.Translate (MoveSpeed * Time.deltaTime, 0, 0);
			}
			if (Input.GetKey (KeyCode.Space)) {
				transform.Translate (0, MoveSpeed * Time.deltaTime, 0);
			}
			if (Input.GetKey (KeyCode.LeftControl)) {
				transform.Translate (0, -MoveSpeed * Time.deltaTime, 0);
			}
			yield return null;
		}
	}

	private IEnumerator DebugRotate(){
		while (true) {
			transform.rotation = Quaternion.Euler(Input.mousePosition.y/2 * -1, Input.mousePosition.x/2, 0);
			yield return null;
		}
	}
}

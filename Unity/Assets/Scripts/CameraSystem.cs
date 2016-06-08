using UnityEngine;
using System.Collections;

public class CameraSystem : MonoBehaviour
{
	private static GameObject MyCamera;
	private static float MaxNormalZ_Error;
	private static float MinNormalZ_Error;
	private static Vector3 LookBehindPos;
	private static Vector3 LookFrontPos;
	private static GameObject AirPlain;

	private static bool _lookBehind = false;
	public  static bool LookBehind {
		set{
			if (value) {
//				MyCamera.GetComponent<CameraSystem>().StopAllCoroutines ();
//				Color color = ReticleSystem.UIImage.color;
//				Debug.Log ("a");
//				ReticleSystem.UIImage.color = new Vector4 (color.r, color.g, color.b, 1);
			} else {
			}
			_lookBehind = value;

		}get{
			return _lookBehind;
		}
	}


	private static bool freemove = false;
	public static bool FreeMove {
		set {
			if (value == false) {
				MyCamera.transform.localPosition = LookFrontPos;
				MyCamera.transform.localRotation = new Quaternion (0, 0, 0, MyCamera.transform.localRotation.w);
			}
			Color color = ReticleSystem.UIImage.color;
			ReticleSystem.UIImage.color = new Vector4 (color.r, color.g, color.b, value ? 0 : 1);
			//GameObject.Find ("ReticleImage").SetActive (!value);
			freemove = value;
		}get {
			return freemove;
		}
	}

	private static bool stopReset = false;
	public static bool StopReset {
		set {
			stopReset = value;
		}
		get {
			return stopReset;
		}
	}

	void Awake ()
	{
		MyCamera = GameObject.Find ("Main Camera");
		AirPlain = GameObject.Find ("eurofighter");
	}

	void Start ()
	{
		StartCoroutine (CameraChangePosition ());
		StartCoroutine (CameraModeChange ());
		LookFrontPos = MyCamera.transform.localPosition;
		LookBehindPos = new Vector3 (0,  LookFrontPos.y-9.2f, LookFrontPos.z+80.5f);//GameObject.Find ("CameraPos1").transform.localPosition;
		MaxNormalZ_Error = LookFrontPos.z + 0.1f;
		MinNormalZ_Error = LookFrontPos.z - 0.1f;
	}

	public IEnumerator LookTgt (GameObject Tgt)
	{
		Vector3 TgtPos = Tgt.transform.position;
		while (!GameManager.GameOver) {
			transform.LookAt (TgtPos);
			yield return null;
		}
		yield return null;
	}

	private IEnumerator CameraModeChange ()
	{
		while (!GameManager.GameOver) {
			if (Input.GetKeyDown (KeyCode.JoystickButton6)) {
				FreeMove = !FreeMove;
				StartCoroutine (CameraFreeMove ());
			}
			yield return null;
		}
	}

	private IEnumerator CameraFreeMove ()
	{
		while (!GameManager.GameOver && freemove) {
			CameraMove (InputController ());
			yield return null;
		}
	}

	private Vector2 InputController ()
	{
		return new Vector2 (Input.GetAxis ("3thAxis"), Input.GetAxis ("4thAxis"));
	}

	private void CameraMove (Vector2 Move)
	{
		transform.RotateAround (AirPlain.transform.position, Vector3.up, Move.x * 2);
		transform.RotateAround (AirPlain.transform.position, Vector3.left, Move.y * 2);
	}

	private static IEnumerator CameraChangePosition ()
	{
		while (!GameManager.GameOver) {
			if (isChange ()) {
				if (FreeMove) {
					FreeMove = false;
					yield return null;
					continue;
				}
				MyCamera.transform.localPosition = ChangePos (MyCamera.transform.localPosition);
				MyCamera.transform.Rotate (MyCamera.transform.rotation.x - 5, 180, 0);
			}
			yield return null;
		}
	}

	private static bool isChange ()
	{
		return Input.GetKeyDown (KeyCode.JoystickButton11) || Input.GetKeyDown (KeyCode.M);
	}

	private static Vector3 ChangePos (Vector3 NowPos)
	{
		if (!LookBehind) {
			LookBehind = true;
			return LookBehindPos;
		} else {
			LookBehind = false;
			return LookFrontPos;
		}
	}

	public static IEnumerator CameraPosReset ()
	{
		if (LookBehind) {
			yield break;
		}
		float dis = MyCamera.transform.localPosition.z - (LookFrontPos.z);
		while ((MyCamera.transform.localPosition.z >= MaxNormalZ_Error || MyCamera.transform.localPosition.z <= MinNormalZ_Error) && !stopReset) {
			MyCamera.transform.Translate (0, 0, -0.05f * System.Math.Sign (dis));
			yield return null;
		}
		if (MyCamera.transform.localPosition.z <= MaxNormalZ_Error && MyCamera.transform.localPosition.z >= MinNormalZ_Error) {
			MyCamera.transform.localPosition = new Vector3 (0, LookFrontPos.y, LookFrontPos.z);
		}
		yield return null;
	}

	public static IEnumerator SwayCamera ()
	{
		float SwayTime = 0;
		Vector3 NormalPos = new Vector3 (0, 15, -50);//MyCamera.transform.localPosition;

		while (SwayTime < 0.4f && !GameManager.GameOver) {
			SwayTime += Time.deltaTime;

			Vector3 Amplitude = new Vector3 (Random.Range (-5, 5), Random.Range (-5, 5), 0);
			MyCamera.transform.localPosition = new Vector3 (NormalPos.x + Amplitude.x, NormalPos.y + Amplitude.y,MyCamera.transform.localPosition.z);
			yield return new WaitForSeconds (0.01f);
			yield return null;
		}
		MyCamera.transform.localPosition = LookBehind ? LookBehindPos : NormalPos;
		yield return null;
	}

	public static void MoveCamera (float value)
	{
		if (freemove || LookBehind) {
			return;
		}
		Vector3 CameraPos = MyCamera.transform.localPosition;
		MyCamera.transform.localPosition = new Vector3 (CameraPos.x, CameraPos.y, Mathf.Clamp (CameraPos.z + -0.05f * (value / (value / value)), LookFrontPos.z - 7, LookFrontPos.z + 7));
	}

}

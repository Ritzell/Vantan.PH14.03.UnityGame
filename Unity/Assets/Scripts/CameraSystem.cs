using UnityEngine;
using System.Collections;

public class CameraSystem : MonoBehaviour
{
	private static GameObject MyCamera;

	private static Vector3 LookBehindPos;
	private static Vector3 LookFrontPos;
	private static bool stopReset = false;
	private static GameObject AirPlain;
	private static bool freemove = false;
	private static bool LookBehind = false;

	public static bool FreeMove{
		set{
			if (value == false) {
				MyCamera.transform.localPosition = LookFrontPos;//ChangePos (MyCamera.transform.localPosition);
				//MyCamera.transform.Rotate (0, 180, 0);
				MyCamera.transform.localRotation = new Quaternion(0,0,0,MyCamera.transform.localRotation.w);
			}
			freemove = value;
		}get{
			return freemove;
		}
	}

	public static bool StopReset {
		set {
			stopReset = value;
		}
		get {
			return stopReset;
		}
	}

	struct CameraLimitter
	{
		public const float NormalZ = -26f;
		public const float NormalY = 7.5f;
		public const float MaxNormalZ = NormalZ + 4;
		public const float MinNormalZ = NormalZ - 4;
		public const float MaxNormalZ_Error = NormalZ + 0.1f;
		public const float MinNormalZ_Error = NormalZ - 0.1f;
	}

	void Start ()
	{
		MyCamera = GameObject.Find ("Main Camera");
		AirPlain = GameObject.Find ("eurofighter");
		StartCoroutine (CameraSwitching ());
		StartCoroutine (CameraModeChange ());
		LookBehindPos = new Vector3 (0, 5.8f, 30.5f);//GameObject.Find ("CameraPos1").transform.localPosition;
		LookFrontPos = MyCamera.transform.localPosition;
	}

	private IEnumerator CameraModeChange ()
	{
		while (!GameManager.GameOver) {
			if (Input.GetKeyDown (KeyCode.JoystickButton6)) {
				FreeMove = !FreeMove;
				StartCoroutine (CameraFreeMove());
			}
			yield return null;
		}
	}

	private IEnumerator CameraFreeMove(){
		while (!GameManager.GameOver && freemove) {
			CameraMove (Input.GetAxis("3thAxis"),Input.GetAxis("4thAxis"));
			yield return null;
		}
	}

	private void CameraMove(float MoveX,float MoveY){
		transform.RotateAround (AirPlain.transform.position, Vector3.up, MoveX*2);
		transform.RotateAround (AirPlain.transform.position, Vector3.left, MoveY*2);
	}

	private static IEnumerator CameraSwitching ()
	{
		while (!GameManager.GameOver) {
			if (Input.GetKeyDown (KeyCode.JoystickButton11)) {
				if(FreeMove){
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

	private static Vector3 ChangePos (Vector3 NowPos)
	{
		if (NowPos.z <= -22f && NowPos.x >= -30f) {
			LookBehind = true;
			return LookBehindPos;
		} else {
			LookBehind = false;
			return LookFrontPos;
		}
	}

	public static IEnumerator CameraPosReset ()
	{
		if(LookBehind){
			yield break;
		}
		float dis = MyCamera.transform.localPosition.z - (CameraLimitter.NormalZ);
		while ((MyCamera.transform.localPosition.z >= CameraLimitter.MaxNormalZ_Error || MyCamera.transform.localPosition.z <= CameraLimitter.MinNormalZ_Error) && !stopReset) {
			MyCamera.transform.Translate (0, 0, -0.05f * System.Math.Sign (dis));
			yield return null;
		}
		if (MyCamera.transform.localPosition.z <= CameraLimitter.MaxNormalZ_Error && MyCamera.transform.localPosition.z >= CameraLimitter.MinNormalZ_Error) {
			MyCamera.transform.localPosition = new Vector3 (0, CameraLimitter.NormalY, CameraLimitter.NormalZ);
		}
		yield return null;
	}

	public static void MoveCamera (float value)
	{
		if (freemove || LookBehind) {
			return;
		}
		Vector3 CameraPos = MyCamera.transform.localPosition;
		Vector3 BehindPos = GameObject.Find ("CameraPos1").transform.localPosition;

//		if (LookBehind) {
//			MyCamera.transform.localPosition = new Vector3(CameraPos.x,CameraPos.y,Mathf.Clamp(CameraPos.z + 0.025f * (value / (value / value)),34.5f,26f));
//			return;
//		}

		MyCamera.transform.localPosition = new Vector3(CameraPos.x,CameraPos.y,Mathf.Clamp(CameraPos.z + -0.025f * (value / (value / value)),-30f,-22f));
			//GameObject.Find ("CameraPos1").transform.localPosition = new Vector3(BehindPos.x,BehindPos.y,Mathf.Clamp(BehindPos.z + 0.025f * (value / (value / value)),-30f,-22f));
	}

}

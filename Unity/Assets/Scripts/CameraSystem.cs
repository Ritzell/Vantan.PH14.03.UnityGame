using UnityEngine;
using System.Collections;

public class CameraSystem : MonoBehaviour
{
	private static GameObject MyCamera;

	private static Vector3 LookBehindPos;
	private static Vector3 LookFrontPos;
	private static bool stopReset = false;

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
		StartCoroutine (CameraWork ());
		LookBehindPos = GameObject.Find ("CameraPos1").transform.localPosition;
		LookFrontPos = MyCamera.transform.localPosition;
	}


	private static IEnumerator CameraWork ()
	{
		while (!GameManager.GameOver) {
			if (Input.GetKeyDown (KeyCode.JoystickButton11)) {
				MyCamera.transform.localPosition = ChangePos (MyCamera.transform.localPosition);
				MyCamera.transform.Rotate (MyCamera.transform.rotation.x - 5, 180, 0);
			}
			yield return null;
		}
	}

	private static Vector3 ChangePos (Vector3 NowPos)
	{
		if (NowPos == LookFrontPos) {
			return LookBehindPos;
		} else {
			return LookFrontPos;
		}
	}

	public static IEnumerator CameraPosReset ()
	{
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

	public static float MoveCamera {
		set {
			if (PlayerMove.SpeedConfig.Speed < PlayerMove.SpeedConfig.MaxSpeed && PlayerMove.SpeedConfig.Speed > PlayerMove.SpeedConfig.MinSpeed) {
				MyCamera.transform.Translate (0, 0, -0.025f * (value / (value / value)));
			}

			if (MyCamera.transform.localPosition.z < CameraLimitter.MinNormalZ) {
				MyCamera.transform.localPosition = new Vector3 (0, CameraLimitter.NormalY, CameraLimitter.MinNormalZ);
			} else if (MyCamera.transform.localPosition.z > CameraLimitter.MaxNormalZ) {
				MyCamera.transform.localPosition = new Vector3 (0, CameraLimitter.NormalY, CameraLimitter.MaxNormalZ);
			}
		}
	}

}

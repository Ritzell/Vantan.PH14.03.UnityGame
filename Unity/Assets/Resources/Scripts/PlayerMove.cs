using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{



	public struct SpeedConfig
	{
		public static float Speed = 300f;
		public const short Accele = +1;
		public const short decele = -1;
		public const float MinSpeed = 200f;
		public const float MaxSpeed = 690f;
	}

	private const float Keep = 0;
	public	GameObject MyCamera;
	private static engineSound EngineS;

	void Start ()
	{
		EngineS = GameObject.Find ("engine").GetComponent<engineSound> ();
		StartCoroutine (Move ());
		StartCoroutine (ChangeSpeed ());
	}

	public IEnumerator Move ()
	{
		while (!GameManager.GameOver) {
			Rotation = new Vector3 (Input.GetAxis ("Vertical") * 3, 0, Input.GetAxis ("Horizontal") * 2);
			MoveForward ();
			yield return null;
		}
	}

	public void MoveForward ()
	{
		transform.Translate (Vector3.forward * Time.deltaTime * SpeedConfig.Speed);
	}

	/// <summary>
	/// 機体の速度を加減する。
	/// </summary>
	/// <returns>The speed.</returns>
	public IEnumerator ChangeSpeed ()
	{
		while (!GameManager.GameOver) {
			if (Input.GetKey (KeyCode.JoystickButton13) || Input.GetKey (KeyCode.JoystickButton14) || Input.GetKey (KeyCode.Alpha1) || Input.GetKey (KeyCode.Alpha2)) {
				CameraSystem.StopReset = true;
				if (Input.GetKey (KeyCode.JoystickButton13) || Input.GetKey (KeyCode.Alpha1)) {
					FuelTank = SpeedConfig.decele;
				} else if (Input.GetKey (KeyCode.JoystickButton14) || Input.GetKey (KeyCode.Alpha2)) {//Joystick1Button5 or Joystick8Button12 or JoystickButton14
					FuelTank = SpeedConfig.Accele;
				}
			} else if (CameraSystem.StopReset) {
				CameraSystem.StopReset = false;
				AfterBurner (Keep);
				StartCoroutine (CameraSystem.CameraPosReset ());
			}
			yield return null;
		}
	}

	/// <summary>
	/// 機体の速度に制限。
	/// 巡航速度1000Km 最高速度2484Km (speed*60*60=時速とする)
	/// </summary>
	/// <value>The speed.</value>
	public float FuelTank {
		set{
		if (SpeedConfig.Speed >= SpeedConfig.MinSpeed && SpeedConfig.Speed <= SpeedConfig.MaxSpeed) {
				CameraSystem.MoveCamera = value;
				SpeedConfig.Speed += value;
				AfterBurner (value);
		}
		if (SpeedConfig.Speed < SpeedConfig.MinSpeed) {
			SpeedConfig.Speed = SpeedConfig.MinSpeed;
		} else if (SpeedConfig.Speed > SpeedConfig.MaxSpeed) {
			SpeedConfig.Speed = SpeedConfig.MaxSpeed;
		}
		EngineS.Pitch = SpeedConfig.Speed;
		}
	}

	public void AfterBurner (float fuel)
	{
		ParticleSystem burner = GameObject.Find ("Afterburner").GetComponent<ParticleSystem> ();
		ParticleSystem glow = GameObject.Find ("Glow").GetComponent<ParticleSystem> ();
		var em = glow.emission;
		var rate = glow.emission.rate;

		if (fuel > Keep) {
			burner.startSpeed = 25;
			glow.startSpeed = 25;
			rate.constantMax = 450f;
			em.rate = rate;

		} else if (fuel == Keep) {
			burner.startSpeed = 4;
			glow.startSpeed = 4;
			rate.constantMax = 100f;
			em.rate = rate;

		} else {
			Debug.Log ("減速時のアフターバーナーの火力は如何致しましょうか？");
		}
	}

	private Vector3 Rotation {
		set {
			transform.Rotate (value.x / 1.5f, 0f, value.z * 2.5f);
			//myCamera.transform.Rotate (0, 0, -value.z * 2.5f);//カメラ回転無効  
			//myCamera.transform.localRotation = new Quaternion(myCamera.transform.rotation.x,myCamera.transform.rotation.y,0,myCamera.transform.rotation.w);
		}
	}
}

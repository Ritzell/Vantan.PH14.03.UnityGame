using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{



	public struct speedConfig
	{
		public static float speed = 300f;
		public const short accele = +1;
		public const short decele = -1;
		public const float minSpeed = 200f;
		public const float maxSpeed = 690f;
	}

	private const float keep = 0;
	public	GameObject myCamera;
	private static engineSound engineS;

	void Start ()
	{
		engineS = GameObject.Find ("engine").GetComponent<engineSound> ();
		StartCoroutine (move ());
		StartCoroutine (changeSpeed ());
	}

	public IEnumerator move ()
	{
		while (!GameManager.GameOver) {
			Rotation = new Vector3 (Input.GetAxis ("Vertical") * 3, 0, Input.GetAxis ("Horizontal") * 2);
			moveForward ();
			yield return null;
		}
	}

	public void moveForward ()
	{
		transform.Translate (Vector3.forward * Time.deltaTime * speedConfig.speed);
	}

	/// <summary>
	/// 機体の速度を加減する。
	/// </summary>
	/// <returns>The speed.</returns>
	public IEnumerator changeSpeed ()
	{
		while (!GameManager.GameOver) {
			if (Input.GetKey (KeyCode.JoystickButton13) || Input.GetKey (KeyCode.JoystickButton14) || Input.GetKey (KeyCode.Alpha1) || Input.GetKey (KeyCode.Alpha2)) {
				CameraSystem.stopReset = true;
				if (Input.GetKey (KeyCode.JoystickButton13) || Input.GetKey (KeyCode.Alpha1)) {
					fuelTank = speedConfig.decele;
				} else if (Input.GetKey (KeyCode.JoystickButton14) || Input.GetKey (KeyCode.Alpha2)) {//Joystick1Button5 or Joystick8Button12 or JoystickButton14
					fuelTank = speedConfig.accele;
				}
			} else if (CameraSystem.stopReset) {
				CameraSystem.stopReset = false;
				afterBurner (keep);
				StartCoroutine (CameraSystem.cameraPosReset ());
			}
			yield return null;
		}
	}

	/// <summary>
	/// 機体の速度に制限。
	/// 巡航速度1000Km 最高速度2484Km (speed*60*60=時速とする)
	/// </summary>
	/// <value>The speed.</value>
	public float fuelTank {
		set{
		if (speedConfig.speed >= speedConfig.minSpeed && speedConfig.speed <= speedConfig.maxSpeed) {
				CameraSystem.moveCamera = value;
				speedConfig.speed += value;
				afterBurner (value);
		}
		if (speedConfig.speed < speedConfig.minSpeed) {
			speedConfig.speed = speedConfig.minSpeed;
		} else if (speedConfig.speed > speedConfig.maxSpeed) {
			speedConfig.speed = speedConfig.maxSpeed;
		}
		engineS.Pitch = speedConfig.speed;
		}
	}

	public void afterBurner (float fuel)
	{
		ParticleSystem burner = GameObject.Find ("Afterburner").GetComponent<ParticleSystem> ();
		ParticleSystem glow = GameObject.Find ("Glow").GetComponent<ParticleSystem> ();
		var em = glow.emission;
		var rate = glow.emission.rate;

		if (fuel > keep) {
			burner.startSpeed = 25;
			glow.startSpeed = 25;
			rate.constantMax = 450f;
			em.rate = rate;

		} else if (fuel == keep) {
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

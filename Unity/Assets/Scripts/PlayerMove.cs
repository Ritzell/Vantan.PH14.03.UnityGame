using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
	public static float Speed = 300f;
	public const short Accele = +1;
	public const short Decele = -1;
	public const float MinSpeed = 200f;
	public const float MaxSpeed = 690f;
	private const float Keep = 0;

	private static EngineSound EngineS;

	void Awake(){
		EngineS = GameObject.Find ("engine").GetComponent<EngineSound> ();
	}

	void Start ()
	{
		StartCoroutine (Move ());
		StartCoroutine (ChangeSpeed ());
	}

	private IEnumerator Move ()
	{
		while (!GameManager.GameOver) {
			Rotation(new Vector3 (Input.GetAxis ("Vertical") * 3, 0, Input.GetAxis ("Horizontal") * 2));
			MoveForward ();
			yield return null;
		}
	}

	private void MoveForward ()
	{
		transform.Translate (Vector3.forward * Time.deltaTime * Speed);
	}

	/// <summary>
	/// 機体の速度を加減する。
	/// </summary>
	/// <returns>The speed.</returns>
	private IEnumerator ChangeSpeed ()
	{
		while (!GameManager.GameOver) {
			if (Input.GetKey (KeyCode.JoystickButton13) || Input.GetKey (KeyCode.JoystickButton14) || Input.GetKey (KeyCode.Alpha1) || Input.GetKey (KeyCode.Alpha2)) {
				if (Input.GetKey (KeyCode.JoystickButton13) || Input.GetKey (KeyCode.Alpha1)) {
					FuelInjector(Decele);
				} else if (Input.GetKey (KeyCode.JoystickButton14) || Input.GetKey (KeyCode.Alpha2)) {//Joystick1Button5 or Joystick8Button12 or JoystickButton14
					FuelInjector(Accele);
				}
			} else if (Input.GetKeyUp (KeyCode.JoystickButton13) || Input.GetKeyUp (KeyCode.JoystickButton14) || Input.GetKeyUp (KeyCode.Alpha1) || Input.GetKeyUp (KeyCode.Alpha2)) {
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
	private void FuelInjector (float Signal){
			Speed = Mathf.Clamp(Speed + Signal,MinSpeed,MaxSpeed);
			if (Speed > MinSpeed && Speed < MaxSpeed) {
				AfterBurner (Signal);
				CameraSystem.MoveCamera (Signal);
			}
			EngineS.Pitch = Speed;
	}

	private void AfterBurner (float Fuel)
	{
		ParticleSystem Burner = GameObject.Find ("Afterburner").GetComponent<ParticleSystem> ();
		ParticleSystem Glow = GameObject.Find ("Glow").GetComponent<ParticleSystem> ();
		var em = Glow.emission;
		var rate = Glow.emission.rate;

		if (Fuel > Keep) {
			HighPower (Burner, Glow, em, rate);
		} else if (Fuel == Keep) {
			LowPower (Burner, Glow, em, rate);
		} else {
		}
	}

	//下記二つのプログラムが嫌い
	private void HighPower (ParticleSystem Burner, ParticleSystem Glow, ParticleSystem.EmissionModule em, ParticleSystem.MinMaxCurve rate)
	{
		Burner.startSpeed = 25;
		Glow.startSpeed = 25;
		rate.constantMax = 450f;
		em.rate = rate;
	}

	private void LowPower (ParticleSystem Burner, ParticleSystem Glow, ParticleSystem.EmissionModule em, ParticleSystem.MinMaxCurve rate)
	{
		Burner.startSpeed = 4;
		Glow.startSpeed = 4;
		rate.constantMax = 100f;
		em.rate = rate;
	}

	private void Rotation(Vector3 AddRot) {
		transform.Rotate (AddRot.x / 1.5f, 0f, AddRot.z * 2f);
	}
}

using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
	
	public	GameObject 	myCamera;
	private static engineSound engineS;

	public struct speedConfig{
		public static float speed = 300f;
		public const short accele = +1;
		public const short decele = -1;
		public const float cruisingSpeed = 200f;
		public const float maxSpeed = 690f;
	}

	void Start () {
		engineS = GameObject.Find("engine").GetComponent<engineSound> ();
        StartCoroutine(move());
		StartCoroutine(changeSpeed());
	}
    public IEnumerator move()
    {
        while (!GameManager.GameOver)
        {
            Rotation = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
			moveForward ();
            yield return null;
        }
    }

	public void moveForward(){
		transform.Translate(Vector3.forward * Time.deltaTime * speedConfig.speed);
	}

	public IEnumerator changeSpeed(){
		while (!GameManager.GameOver) {
			if (Input.GetKey (KeyCode.JoystickButton13) || Input.GetKey (KeyCode.JoystickButton14)) {
				CameraSystem.stopReset = true;
				if (Input.GetKey (KeyCode.JoystickButton13)) {
					Speed = speedConfig.decele;
				} else if (Input.GetKey (KeyCode.JoystickButton14)) {//Joystick1Button5 or Joystick8Button12 or JoystickButton14
					Speed = speedConfig.accele;
				}
			} else if (CameraSystem.stopReset) {
				CameraSystem.stopReset = false;
				StartCoroutine (CameraSystem.cameraPosReset ());
			}
			yield return null;
		}
	}

	/// <summary>
	/// 機体の速度を変更する。
	/// 巡航速度1000Km 最高速度2484Km (speed*60*60=時速とする)
	/// </summary>
	/// <value>The speed.</value>
	public static float Speed {
		set{
			if (speedConfig.speed >= speedConfig.cruisingSpeed && speedConfig.speed <= speedConfig.maxSpeed) {
				CameraSystem.moveCamera = value;
				speedConfig.speed += value;
			}
			if (speedConfig.speed < speedConfig.cruisingSpeed) {
				speedConfig.speed = speedConfig.cruisingSpeed;
			} else if (speedConfig.speed > speedConfig.maxSpeed) {
				speedConfig.speed = speedConfig.maxSpeed;
			}
			engineS.Pitch = speedConfig.speed;
		}get{
			return speedConfig.speed;
		}
	}
    private Vector3 Rotation
    {
        set
        {
			transform.Rotate (value.x / 1.5f, 0f, value.z * 2.5f);
			myCamera.transform.Rotate (0, 0, -value.z * 2.5f);//カメラ回転無効  
        }
    }
}

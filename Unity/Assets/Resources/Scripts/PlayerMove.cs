using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
	
	public static float speed = 300;
	public	GameObject 	myCamera;
	private static engineSound engineS;

	void Start () {
		engineS = GameObject.Find("engine").GetComponent<engineSound> ();
        StartCoroutine(move());
		StartCoroutine(changeSpeed());
	}
    public IEnumerator move()
    {
        while (!GameManager.GameOver)
        {
            Rm = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
			transform.Translate(Vector3.forward * Time.deltaTime * speed);
            yield return null;
        }
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
			if (speed >= speedConfig.cruisingSpeed && speed <= speedConfig.maxSpeed) {
				CameraSystem.moveCamera = value;
                speed += value;
			}
			if (speed < speedConfig.cruisingSpeed) {
				speed = speedConfig.cruisingSpeed;
			} else if (speed > speedConfig.maxSpeed) {
				speed = speedConfig.maxSpeed;
			}
			engineS.Pitch = speed;
		}get{
			return speed;
		}
	}
    private Vector3 Rm
    {
        set
        {
			transform.Rotate (value.x / 1.5f, 0f, value.z * 2.5f);
			myCamera.transform.Rotate (0, 0, -value.z * 2.5f);//カメラ回転無効  
        }
    }
}

class speedConfig{
	public const short accele = +1;
	public const short decele = -1;
	public const float cruisingSpeed = 300f;
	public const float maxSpeed = 690f;
}

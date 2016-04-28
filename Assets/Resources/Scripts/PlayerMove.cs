<<<<<<< HEAD
﻿using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
	public static float speed = 80.0f;
	public	GameObject	body;
	public	GameObject 	myCamera;
	public	GameObject	look;
	engineSound engineS;
	// Use this for initialization
	void Start () {
		engineS = GameObject.Find("engine").GetComponent<engineSound> ();
        StartCoroutine(move());
	}
    public IEnumerator move()
    {
        while (!GameManager.GameOver)
        {
            Rm = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
            //myCamera.transform.LookAt(look.transform);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);

            if (Input.GetKey(KeyCode.Joystick1Button4) || Input.GetKey(KeyCode.Joystick1Button5))
            {
                if (Input.GetKey(KeyCode.Joystick1Button4))
                {
                    Speed = -1f;
                }else if (Input.GetKey(KeyCode.Joystick1Button5))// or 12
                {
                    Speed = +1f;
                }
            }else if (myCamera.transform.localPosition.z >= -14.45f || myCamera.transform.localPosition.z <= -14.55f)
            {
                float dis = myCamera.transform.localPosition.z - (-14.5f);
                myCamera.transform.Translate(0,0,-0.05f* System.Math.Sign(dis));
                //myCamera.transform.localPosition = new Vector3(myCamera.transform.localPosition.x, myCamera.transform.localPosition.y, -14.5f);
            }else
            {
                myCamera.transform.localPosition = new Vector3(-0.85f, 12.7f,-14.5f);
            }
            yield return null;
        }
        yield return null;
    }
    private float moveCamera
    {
        set
        {
            if (speed < 280 && speed > 60)
            {
                myCamera.transform.Translate(0, 0, -0.025f * (value / (value / value)));
            }

            if (myCamera.transform.localPosition.z < -16.5f)
            {
                myCamera.transform.localPosition = new Vector3(-0.85f, 12.7f, -16.5f);
            }
            else if (myCamera.transform.localPosition.z > -12)
            {
                myCamera.transform.localPosition = new Vector3(-0.85f, 12.7f, -12);
            }
        }
    }

	public float Speed {
		set{
			if (speed >= 60f && speed <= 280f) {
                moveCamera = value;
                speed += value;
			}
			if (speed < 60) {
				speed = 60f;
			} else if (speed > 280) {
				speed = 280f;
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
            transform.Rotate(value.x / 1.5f, 0f, value.z * 2.5f);
            myCamera.transform.Rotate(0, 0, -(value.z * 2.5f));//カメラ回転無効  
        }
    }
}
=======
﻿using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
	private float speed = 80.0f;
	public	GameObject	body;
	public	GameObject 	myCamera;
	public	GameObject	look;
	engineSound engineS;
	// Use this for initialization
	void Start () {
		engineS = GameObject.Find("engine").GetComponent<engineSound> ();
	}
	
	// Update is called once per frame
	void Update () {
		float mY =  Input.GetAxis ("Horizontal");
		float mX = Input.GetAxis ("Vertical");

		//myCamera.transform.LookAt(look.transform);
		transform.Translate (Vector3.forward * Time.deltaTime * speed);
		transform.Rotate (mX/1.5f, 0f, mY*2.5f);

		if (Input.GetKey(KeyCode.Joystick1Button13)) {
			Speed= -1f;
		}

		if (Input.GetKey(KeyCode.Joystick1Button14)) {
			Debug.Log ("speedUP");
			Speed=+1f;
		}
	}
	public float Speed {
		set{
			if (this.speed >= 60f && this.speed <= 280f) {
				this.speed += value;
			}
			if (this.speed < 60) {
				this.speed = 60f;
			} else if (this.speed > 280) {
				this.speed = 280f;
			}
			engineS.Pitch = this.speed;
		}get{
			if (this.speed < 60) {
				return 60f;
			}
			return this.speed;
		}
	}
}
>>>>>>> 37ffaef15fce39cf566d16ffa856979c0f36a069

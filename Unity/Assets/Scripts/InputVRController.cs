using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputVRController : MonoBehaviour {
    [SerializeField]
    private List<GameObject> Controllers = new List<GameObject>();
    static SteamVR_TrackedObject[] trackedObject = new SteamVR_TrackedObject[2];
    // Use this for initialization

    public enum InputPress
    {
        PressTrigger,
        PressPad,
        PressGrip,
        UpPad,
        UpGrip,
        TouchPadPosition
    }

    void Start()
    {
        for(int i = 0; i < trackedObject.Length; i++)
        {
            trackedObject[i] = Controllers[i].GetComponent<SteamVR_TrackedObject>();
        }
    }

    void Update()
    {
        Debug.Log(GetPressStay(InputPress.PressGrip));
    }

    public static bool GetPress(InputPress input,bool isRight)
    {
        var device = SteamVR_Controller.Input((int)trackedObject[isRight ? 0 : 1].index);
        switch (input)
        {
            case InputPress.PressTrigger:
                return device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger);
            case InputPress.PressPad:
                return device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad);
            case InputPress.PressGrip:
                return device.GetPressDown(SteamVR_Controller.ButtonMask.Grip);
            case InputPress.UpPad:
                return device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad);
            case InputPress.UpGrip:
                return device.GetPressUp(SteamVR_Controller.ButtonMask.Grip);
            default:
                return false;
        }
    }

    public static bool GetPressStay(InputPress input, bool isRight)
    {
        var device = SteamVR_Controller.Input((int)trackedObject[isRight ? 0 : 1].index);
        switch (input)
        {
            case InputPress.PressTrigger:
                return device.GetPress(SteamVR_Controller.ButtonMask.Trigger);
            case InputPress.PressPad:
                return device.GetPress(SteamVR_Controller.ButtonMask.Touchpad);
            case InputPress.PressGrip:
                return device.GetPress(SteamVR_Controller.ButtonMask.Grip);
            default:
                return false;
        }
    }

    public static bool GetPressStay(InputPress input)
    {
        SteamVR_Controller.Device device, device2;
        try
        {
            device = SteamVR_Controller.Input((int)trackedObject[0].index);
            device2 = SteamVR_Controller.Input((int)trackedObject[1].index);
        }catch
        {
            Debug.Log("コントローラーが認識されていません。");
            return false;
        }

        if (input == InputPress.PressGrip)
            {
                return device.GetPress(SteamVR_Controller.ButtonMask.Grip) || device2.GetPress(SteamVR_Controller.ButtonMask.Grip);
            }
        return false;
    }

    public static bool GetPressUp(InputPress input)
    {
        var device = SteamVR_Controller.Input((int)trackedObject[0].index);
        var device2 = SteamVR_Controller.Input((int)trackedObject[1].index);

        switch (input)
        {
            case InputPress.UpGrip:
                return device.GetPressUp(SteamVR_Controller.ButtonMask.Grip) || device2.GetPressUp(SteamVR_Controller.ButtonMask.Grip);
        }
        return false;
    }

    public static Vector2 GetAxis(bool isRight)
    {
        var device = SteamVR_Controller.Input((int)trackedObject[isRight ? 0 : 1].index);
        return device.GetAxis();
    }
}

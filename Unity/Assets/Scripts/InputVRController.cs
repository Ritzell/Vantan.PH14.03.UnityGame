using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputVRController : MonoBehaviour {
    [SerializeField]
    public List<GameObject> Controllers = new List<GameObject>();
    [SerializeField]
    private GameObject Reticle;
    public static SteamVR_TrackedObject[] trackedObject = new SteamVR_TrackedObject[2];

    public enum InputPress
    {
        PressTrigger,
        PressPad,
        PressGrip,
        PressMenu,
        UpMenu,
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
        //Debug.Log(GetPressStay(InputPress.PressGrip));
    }

    public static bool GetPress(InputPress input,HandType type)
    {
        SteamVR_Controller.Device device;
        try
        {
            device = SteamVR_Controller.Input((int)trackedObject[(int)type].index);
        }catch
        {
            return false;
        }
        switch (input)
        {
            case InputPress.PressTrigger:
                return device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger);
            case InputPress.PressPad:
                return device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad);
            case InputPress.PressGrip:
                return device.GetPressDown(SteamVR_Controller.ButtonMask.Grip);
            case InputPress.PressMenu:
                return device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu);
            case InputPress.UpPad:
                return device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad);
            case InputPress.UpGrip:
                return device.GetPressUp(SteamVR_Controller.ButtonMask.Grip);
            default:
                return false;
        }
    }

    public static bool GetPressStay(InputPress input, HandType type)
    {
        SteamVR_Controller.Device device;
        try
        {
            device = SteamVR_Controller.Input((int)trackedObject[(int)type].index);
        }catch
        {
            return false;
        }
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

    public static bool GetUp(InputPress input, HandType type)
    {
        SteamVR_Controller.Device device;
        try
        {
            device = SteamVR_Controller.Input((int)trackedObject[(int)type].index);
        }
        catch
        {
            return false;
        }
        switch (input)
        {
            case InputPress.UpMenu:
                return device.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu);
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
            //Debug.Log("コントローラーが認識されていません。");
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
        SteamVR_Controller.Device device, device2;
        try
        {
            device = SteamVR_Controller.Input((int)trackedObject[0].index);
            device2 = SteamVR_Controller.Input((int)trackedObject[1].index);
        }catch
        {
            return false;
        }

        switch (input)
        {
            case InputPress.UpGrip:
                return device.GetPressUp(SteamVR_Controller.ButtonMask.Grip) || device2.GetPressUp(SteamVR_Controller.ButtonMask.Grip);
        }
        return false;
    }

    public static Vector2 GetAxis(HandType type)
    {
        var device = SteamVR_Controller.Input((int)trackedObject[(int) type].index);
        return device.GetAxis();
    }

    /// <summary>
    /// コントローラーを振動させます。パワーは100～2000迄です。
    /// </summary>
    /// <param name="Power"></param>
    /// <param name="type"></param>
    public static IEnumerator ControllerPulse(ushort Power,HandType type)
    {
        if(!(Power >= 100 && Power <= 2000))
        {
            Debug.Log("ControllerPulseのPowerは100~2000までの間で指定してください");
            yield break;
            yield return null;
        }
        if (!(type == HandType.Both))
        {
            SteamVR_Controller.Device device;
            try
            {
                device = SteamVR_Controller.Input((int)trackedObject[(int)type].index);
            }
            catch
            {
                Debug.Log("Cant Pulse");
                yield break;
            }
            for (float time = 0; time < 1; time += Time.deltaTime)
            {
                device.TriggerHapticPulse(Power);
                yield return null;
            }
        }
        else
        {
            SteamVR_Controller.Device device,device2;
            try
            {
                device = SteamVR_Controller.Input((int)trackedObject[0].index);
                device2 = SteamVR_Controller.Input((int)trackedObject[1].index);
            }
            catch
            {
                Debug.Log("Cant Pulse");
                yield break;
            }
            for (float time = 0; time < 1; time += Time.deltaTime)
            {
                device.TriggerHapticPulse(Power);
                device2.TriggerHapticPulse(Power);
                yield return null;
            }
        }
        
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSDK : MonoBehaviour {

    private static AndroidJavaObject _activeContext;

    private static AndroidJavaObject  _pluginClass;

    private static AndroidJavaClass _activeClass;

    private static float[] coordinats = new float[3];

    private static float[] coordinatsQuater = new float[4];

    private static Boolean isclicked;

    public static Quaternion _quatReset;

    private static float [] f = new float[4];

    [SerializeField] private static GameObject _controller;

    private static int i = 0;

    private static int k = 1;

    void Awake()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            _activeClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            _activeContext = _activeClass.GetStatic<AndroidJavaObject>("currentActivity");
            _pluginClass = new AndroidJavaObject("rclip.lib.RClip");
            StartCoroutine(ConnectToClipse());
            /*do
            {
                i = _pluginClass.Call<int>("AutoConnect");
            } while (i != 1);
            if (_pluginClass == null)
            {
                Application.Quit();
            }*/
        }
    }
	
	void Update () {
        if (i == 1)
        {
            coordinats = _pluginClass.Call<float[]>("GetAxel");
            //coordinatsQuater = _pluginClass.Call<float[]>("GetDevQuat");
            coordinatsQuater = _pluginClass.Call<float[]>("GetQuat");
            //coordinatsQuater = _pluginClass.Call<float[]>("GetChangedAxes");
        }
    }

    IEnumerator ConnectToClipse()
    {
        yield return new WaitForSeconds(0.3f);
        i = _pluginClass.Call<int>("AutoConnect");
        if (i != 1)
        {
            Reconnect();
        }
    }

    private void Reconnect()
    {
        StartCoroutine(ConnectToClipse());
    }

    public static float[] GetAxel()
    {
        return coordinats;
    }

    public static Quaternion GetQuatApparat()
    {
        f = _pluginClass.Call<float[]>("GetDevQuat");
        return Quaternion.Inverse(new Quaternion(f[1],f[2],f[3],f[0]));
    }

    public static void ResetQuat()
    {
        if (Input.acceleration.y <= 0f)
        {
            k = -1;
        }
        else
        {
            k = 1;
        }
        _pluginClass.Call("ResetQuat");
    }

    public static Quaternion GetQuaternion()
    {
       // GameObject.Find("Text1").GetComponent<UnityEngine.UI.Text>().text = coordinatsQuater[0].ToString() + " " + coordinatsQuater[1].ToString() + " " + coordinatsQuater[2].ToString() + " ";
        //return new Quaternion(coordinatsQuater[2],coordinatsQuater[3],coordinatsQuater[1],coordinatsQuater[0]);
        return new Quaternion(k*coordinatsQuater[2], k*coordinatsQuater[3], coordinatsQuater[1], coordinatsQuater[0]);
        // GameObject.Find("Text1").GetComponent<UnityEngine.UI.Text>().text = new Quaternion(coordinatsQuater[1], coordinatsQuater[3], coordinatsQuater[2], coordinatsQuater[0]).ToString();
        //return new Quaternion(coordinatsQuater[3], coordinatsQuater[1], coordinatsQuater[2], coordinatsQuater[0]);
        // return new Quaternion(coordinatsQuater[2], coordinatsQuater[1], coordinatsQuater[3], coordinatsQuater[0]);
    }

    public static bool GetButton(int _i)
    {
        if (i == 1)
        {
            return _pluginClass.Call<bool>("GetBtnState", _i);
        }
        else
        {
            return false;
        }
    }

    /*public static bool GetButtonMenu()
    {

    }*/

    public static void Vibro()
    {
        _pluginClass.Call("makeVibration", 700);
    }

    public static void ResetAxes()
    {
        //_quatReset = new Quaternion(coordinats[1],coordinats[2],coordinats[3],coordinats[0]);
        _pluginClass.Call("ResetDevice");
    }

    public static void StopGame()
    {

    }
}

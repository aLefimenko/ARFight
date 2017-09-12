
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

    [SerializeField] private static GameObject _controller;

    private static int i = 0;

    void Awake()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            _activeClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            _activeContext = _activeClass.GetStatic<AndroidJavaObject>("currentActivity");
            _pluginClass = new AndroidJavaObject("rclip.lib.RClip");
           /* while (i != 1)
            {
                i = _pluginClass.Call<int>("AutoConnect");
            }*/
            do
            {
                i = _pluginClass.Call<int>("AutoConnect");
            } while (i != 1);
            if (_pluginClass == null)
            {
                Application.Quit();
            }
        }
        //_controller = GameObject.FindGameObjectWithTag("control");
    }
	
	void Update () {
        if (i == 1)
        {
            coordinats = _pluginClass.Call<float[]>("GetAxel");
            coordinatsQuater = _pluginClass.Call<float[]>("GetQuat");

           // GameObject.Find("Text1").GetComponent<UnityEngine.UI.Text>().text = coordinatsQuater[0].ToString()+" "+ coordinatsQuater[1].ToString()+" "+ coordinatsQuater[2].ToString() + " ";
        }
    }

    public static float[] GetAxel()
    {
        return coordinats;
    }

    public static void ResetQuat()
    {
        _pluginClass.Call("ResetQuat");
    }

    public static Quaternion GetQuaternion()
    {
       // GameObject.Find("Text1").GetComponent<UnityEngine.UI.Text>().text = coordinatsQuater[0].ToString() + " " + coordinatsQuater[1].ToString() + " " + coordinatsQuater[2].ToString() + " ";
        //return new Quaternion(coordinatsQuater[2],coordinatsQuater[3],coordinatsQuater[1],coordinatsQuater[0]);
        //return new Quaternion(coordinatsQuater[1], coordinatsQuater[2], coordinatsQuater[3], coordinatsQuater[0]);
        // GameObject.Find("Text1").GetComponent<UnityEngine.UI.Text>().text = new Quaternion(coordinatsQuater[1], coordinatsQuater[3], coordinatsQuater[2], coordinatsQuater[0]).ToString();
        return new Quaternion(coordinatsQuater[1], coordinatsQuater[3], coordinatsQuater[2], coordinatsQuater[0]);
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

    public static void StopGame()
    {

    }
}

using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 30), "Add int to prefabs"))
        {
            PlayerPrefs.SetInt("TestInt", 3);
        }
        if (PlayerPrefs.HasKey("TestInt"))
        {
            GUI.Label(new Rect(165, 10, 300, 30), "Int value added in Player Prefs! = " + PlayerPrefs.GetInt("TestInt"));
        }

        if (GUI.Button(new Rect(10, 45, 150, 30), "Add string to prefabs"))
        {
            PlayerPrefs.SetString("TestString", "test string");
        }
        if (PlayerPrefs.HasKey("TestString"))
        {
            GUI.Label(new Rect(165, 45, 300, 30), "String value added in Player Prefs! = " + PlayerPrefs.GetString("TestString"));
        }

        if (GUI.Button(new Rect(10, 80, 150, 30), "Add float to prefabs"))
        {
            PlayerPrefs.SetFloat("TestFloat", 2f);
        }
        if (PlayerPrefs.HasKey("TestFloat"))
        {
            GUI.Label(new Rect(165, 80, 300, 30), "Float value added in Player Prefs! = " + PlayerPrefs.GetFloat("TestFloat"));
        }
    }
}

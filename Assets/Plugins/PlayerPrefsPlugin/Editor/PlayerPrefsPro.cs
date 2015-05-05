/**
 * @author Nechifor Andrei Marian
 * @email neomarian@gmail.com
 * @version 1.0
 * @date 24-01-2013
 * This class extends PlayerPrefs functionality.
 * With this class you can set and get types: string, float, int, bool, string array, float array, int array, bool array, Vector2, Vector3, Rect.
 * Also you can get an array with all keys saved with this class and an array with types for keys saved in preferences.
 * You can add keys and keyTypes but I recommend to don't do that because these two functions is used inside all `set` functions from this class.
 */

using Microsoft.Win32;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using PlistCS;

[System.Serializable]
public static class PlayerPrefsPro
{
    #region Keys
    /**
	 * Return a string array with all preferences keys saved with this class
	 * @return string[]
	 */
    public static string[] GetAllKeys()
    {
        return GetStringArray("keys");
    }

    /**
     * Return a string array with value types for preferences saved with this class
     * Value type for key[i] is keyType[i]
     * @return string[]
     */
    public static string[] GetAllKeyTypes()
    {
        return GetStringArray("keyTypes");
    }

    public static void AddExistingKeys()
    {
        string[] rKeys = GetStringArray("keys");
        string[] ignoreKeys = new string[3] { "UnityGraphicsQuality", "keys", "keyTypes" };
#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            string windowsRegPath = @"Software\" + PlayerSettings.companyName + @"\" + PlayerSettings.productName;
            int lastUnderscor;
            string newKeyName;
            int pos;

            using (RegistryKey keys = Registry.CurrentUser.OpenSubKey(windowsRegPath))
            {
                if (keys != null)
                {
                    foreach (string keyName in keys.GetValueNames())
                    {
                        lastUnderscor = keyName.LastIndexOf("_");
                        newKeyName = keyName.Substring(0, lastUnderscor);

                        pos = Array.IndexOf(rKeys, newKeyName);
                        if (pos < 0)
                        {
                            pos = Array.IndexOf(ignoreKeys, newKeyName);
                            if (pos < 0)
                            {
                                if (PlayerPrefs.GetInt(newKeyName) != 0)
                                {
                                    AddKey(newKeyName, "Int");
                                }
                                else if (PlayerPrefs.GetFloat(newKeyName) != 0)
                                {
                                    AddKey(newKeyName, "Float");
                                }
                                else if (PlayerPrefs.GetString(newKeyName) != "")
                                {
                                    AddKey(newKeyName, "String");
                                }
                            }
                        }
                    }
                }
            }
        }
        else if(Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
        {
            string macRegPath = "/Users/" + Environment.UserName + "/Library/Preferences/unity." + PlayerSettings.companyName + "." + PlayerSettings.productName + ".plist";
		    Dictionary<string, object> dict = (Dictionary<string, object>)Plist.readPlist(macRegPath);
		    float floatVal = 0;
		    int intVal = 0;
		    string stringVal = "";
            if (dict.Count > 0)
            {
                foreach (KeyValuePair<string, object> pair in dict)
                {
                    if (Array.IndexOf(ignoreKeys, pair.Key) < 0 && Array.IndexOf(rKeys, pair.Key) < 0)
                    {
                        if (int.TryParse(pair.Value.ToString(), out intVal))
                        {
                            AddKey(pair.Key, "Int");
                        }
                        else if (float.TryParse(pair.Value.ToString(), out floatVal))
                        {
                            AddKey(pair.Key, "Float");
                        }
                        else
                        {
                            AddKey(pair.Key, "String");
                        }
                    }
                }
            }
        }
#endif
    }

    /**
     * Used by inside function to add new key when new preference is saved
     * @param string key
     * @param string type
     */
    public static void AddKey(string key, string type)
    {
        string[] keys;
        string[] keyTypes;
        if (PlayerPrefs.HasKey("keys"))
        {
            keys = GetStringArray("keys");
            keyTypes = GetStringArray("keyTypes");
        }
        else
        {
            keys = new string[0];
            keyTypes = new string[0];
        }
        if (Array.IndexOf(keys, key) < 0)
        {
            Array.Resize(ref keys, keys.Length + 1);
            Array.Resize(ref keyTypes, keyTypes.Length + 1);
            keys[keys.Length - 1] = key;
            keyTypes[keyTypes.Length - 1] = type;
            PlayerPrefs.SetString("keys", String.Join("\n", keys));
            PlayerPrefs.SetString("keyTypes", String.Join("\n", keyTypes));
        }
    }

    /**
     * Used to delete a key from preferences, from `keys` array and from `keyTypes` array
     * @param string key
     * @return bool
     */
    public static bool DeleteKey(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string[] keys = GetStringArray("keys");
            string[] keyTypes = GetStringArray("keyTypes");
            string[] keysCache = new string[keys.Length - 1];
            string[] keyTypesCache = new string[keyTypes.Length - 1];
            int k = 0;
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i] != key)
                {
                    keysCache[k] = keys[i];
                    keyTypesCache[k] = keyTypes[i];
                    k += 1;
                }
            }
            if (keysCache.Length > 0)
            {
                PlayerPrefs.SetString("keys", String.Join("\n", keysCache));
                PlayerPrefs.SetString("keyTypes", String.Join("\n", keyTypesCache));
            }
            else
            {
                PlayerPrefs.DeleteKey("keys");
                PlayerPrefs.DeleteKey("keyTypes");
            }
            PlayerPrefs.DeleteKey(key);
            return true;
        }
        return false;
    }

    /**
     * Check if key exist in preferences
     * @param string key
     * @return bool
     */
    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }
    #endregion

    #region String
    /**
	 * Save string to preferences
	 * @param string key
	 * @param string value
	 * @return bool
	 */
    public static bool SetString(string key, string value)
    {
        if (key == "keys" || key == "keyTypes")
        {
            Debug.Log("Key `keys` and `keyTypes` are used to store all keys from PlayerPrefs. Please use another key!");
            return false;
        }
        else
        {
            PlayerPrefs.SetString(key, value);
            AddKey(key, "String");
            PlayerPrefs.Save();
            return true;
        }
    }

    /**
     * Return string value saved in preferences for key `key`
     * @param string key
     * @return string
     */
    public static string GetString(string key)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetString(key);
        return "";
    }
    #endregion

    #region Float
    /**
	 * Save float to preferences
	 * @param string key
	 * @param float value
	 * @return bool
	 */
    public static bool SetFloat(string key, float value)
    {
        if (key == "keys" || key == "keyTypes")
        {
            Debug.Log("Key `keys` and `keyTypes` are used to store all keys from PlayerPrefs. Please use another key!");
            return false;
        }
        else
        {
            PlayerPrefs.SetFloat(key, value);
            AddKey(key, "Float");
            PlayerPrefs.Save();
            return true;
        }
    }

    /**
     * Return float value saved in preferences for key `key`
     * @param string key
     * @return float
     */
    public static float GetFloat(string key)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetFloat(key);
        return 0;
    }
    #endregion

    #region Int
    /**
	 * Save int to preferences
	 * @param string key
	 * @param int value
	 * @return bool
	 */
    public static bool SetInt(string key, int value)
    {
        if (key == "keys" || key == "keyTypes")
        {
            Debug.Log("Keys `keys` and `keyTypes` are used to store all keys from PlayerPrefs. Please use another key!");
            return false;
        }
        else
        {
            PlayerPrefs.SetInt(key, value);
            AddKey(key, "Int");
            PlayerPrefs.Save();
            return true;
        }
    }

    /**
     * Return int value saved in preferences for key `key`
     * @param string key
     * @return int
     */
    public static int GetInt(string key)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetInt(key);
        return 0;
    }
    #endregion

    #region Bool
    /**
	 * Save bool to preferences
	 * @param string key
	 * @param bool value
	 * @return bool
	 */
    public static bool SetBool(string key, bool value)
    {
        if (key == "keys" || key == "keyTypes")
        {
            Debug.Log("Key `keys` and `keyTypes` are used to store all keys from PlayerPrefs. Please use another key!");
            return false;
        }
        else
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
            AddKey(key, "Bool");
            PlayerPrefs.Save();
            return true;
        }
    }

    /**
     * Return bool value saved in preferences for key `key`
     * @param string key
     * @return bool
     */
    public static bool GetBool(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            if (PlayerPrefs.GetInt(key) == 1)
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region String Array
    /**
	 * Save string array to preferences
	 * @param string key
	 * @param string[] values
	 * @return bool
	 */
    public static bool SetStringArray(string key, params string[] values)
    {
        if (key == "keys" || key == "keyTypes")
        {
            Debug.Log("Key `keys` and `keyTypes` are used to store all keys from PlayerPrefs. Please use another key!");
            return false;
        }
        else
        {
            if (values.Length == 0)
            {
                Debug.Log("Values array empty.");
                return false;
            }
            PlayerPrefs.SetString(key, String.Join("\n", values));
            AddKey(key, "String Array");
            PlayerPrefs.Save();
            return true;
        }
    }

    /**
     * Return string array values saved in preferences for key `key`
     * @param string key
     * @return string[]
     */
    public static string[] GetStringArray(string key)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetString(key).Split("\n"[0]);
        return new string[0];
    }
    #endregion

    #region Float Array
    /**
	 * Save float array to preferences
	 * @param string key
	 * @param float[] values
	 * @return bool
	 */
    public static bool SetFloatArray(string key, params float[] values)
    {
        if (key == "keys" || key == "keyTypes")
        {
            Debug.Log("Key `keys` and `keyTypes` are used to store all keys from PlayerPrefs. Please use another key!");
            return false;
        }
        else
        {
            if (values.Length == 0)
            {
                Debug.Log("Values array empty.");
                return false;
            }
            string[] saveValues = new string[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                saveValues[i] = values[i].ToString();
            }
            PlayerPrefs.SetString(key, String.Join("\n", saveValues));
            AddKey(key, "Float Array");
            PlayerPrefs.Save();
            return true;
        }
    }

    /**
     * Return float array values saved in preferences for key `key`
     * @param string key
     * @return float[]
     */
    public static float[] GetFloatArray(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string[] newValues = PlayerPrefs.GetString(key).Split("\n"[0]);
            float[] returnValues = new float[newValues.Length];
            for (int i = 0; i < newValues.Length; i++)
            {
                returnValues[i] = float.Parse(newValues[i]);
            }
            return returnValues;
        }
        return new float[0];
    }
    #endregion

    #region Int Array
    /**
	 * Save int array to preferences
	 * @param string key
	 * @param int[] values
	 * @return bool
	 */
    public static bool SetIntArray(string key, params int[] values)
    {
        if (key == "keys" || key == "keyTypes")
        {
            Debug.Log("Key `keys` and `keyTypes` are used to store all keys from PlayerPrefs. Please use another key!");
            return false;
        }
        else
        {
            if (values.Length == 0)
            {
                Debug.Log("Values array empty.");
                return false;
            }
            string[] saveValues = new string[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                saveValues[i] = values[i].ToString();
            }
            PlayerPrefs.SetString(key, String.Join("\n", saveValues));
            AddKey(key, "Int Array");
            PlayerPrefs.Save();
            return true;
        }
    }

    /**
     * Return int array values saved in preferences for key `key`
     * @param string key
     * @return int[]
     */
    public static int[] GetIntArray(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string[] newValues = PlayerPrefs.GetString(key).Split("\n"[0]);
            int[] returnValues = new int[newValues.Length];
            for (int i = 0; i < newValues.Length; i++)
            {
                returnValues[i] = int.Parse(newValues[i]);
            }
            return returnValues;
        }
        return new int[0];
    }
    #endregion

    #region Bool Array
    /**
	 * Save bool array to preferences
	 * @param string key
	 * @param bool[] values
	 * @return bool
	 */
    public static bool SetBoolArray(string key, params bool[] values)
    {
        if (key == "keys" || key == "keyTypes")
        {
            Debug.Log("Key `keys` and `keyTypes` are used to store all keys from PlayerPrefs. Please use another key!");
            return false;
        }
        else
        {
            if (values.Length == 0)
            {
                Debug.Log("Values array empty.");
                return false;
            }
            string[] saveValues = new string[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                saveValues[i] = values[i] ? "1" : "0";
            }
            PlayerPrefs.SetString(key, String.Join("\n", saveValues));
            AddKey(key, "Bool Array");
            PlayerPrefs.Save();
            return true;
        }
    }

    /**
     * Return bool array values saved in preferences for key `key`
     * @param string key
     * @return bool[]
     */
    public static bool[] GetBoolArray(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string[] newValues = PlayerPrefs.GetString(key).Split("\n"[0]);
            bool[] returnValues = new bool[newValues.Length];
            for (int i = 0; i < newValues.Length; i++)
            {
                returnValues[i] = (newValues[i] == "1") ? true : false;
            }
            return returnValues;
        }
        return new bool[0];
    }
    #endregion

    #region Vector2
    /**
	 * Save Vector2 to preferences
	 * @param string key
	 * @param Vector2 values
	 * @return bool
	 */
    public static bool SetVector2(string key, Vector2 value)
    {
        if (key == "keys" || key == "keyTypes")
        {
            Debug.Log("Key `keys` and `keyTypes` are used to store all keys from PlayerPrefs. Please use another key!");
            return false;
        }
        else
        {
            string[] saveValues = new string[2] { value.x.ToString(), value.y.ToString() };
            PlayerPrefs.SetString(key, String.Join("\n", saveValues));
            AddKey(key, "Vector2");
            PlayerPrefs.Save();
            return true;
        }
    }

    /**
     * Return Vector2 value saved in preferences for key `key`
     * @param string key
     * @return Vector2
     */
    public static Vector2 GetVector2(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string[] newValues = PlayerPrefs.GetString(key).Split("\n"[0]);
            if (newValues.Length == 2)
            {
                return new Vector2(float.Parse(newValues[0]), float.Parse(newValues[1]));
            }
        }
        return Vector2.zero;
    }
    #endregion

    #region Vector3
    /**
	 * Save Vector3 to preferences
	 * @param string key
	 * @param Vector3 values
	 * @return bool
	 */
    public static bool SetVector3(string key, Vector3 value)
    {
        if (key == "keys" || key == "keyTypes")
        {
            Debug.Log("Key `keys` and `keyTypes` are used to store all keys from PlayerPrefs. Please use another key!");
            return false;
        }
        else
        {
            string[] saveValues = new string[3] { value.x.ToString(), value.y.ToString(), value.z.ToString() };
            PlayerPrefs.SetString(key, String.Join("\n", saveValues));
            AddKey(key, "Vector3");
            PlayerPrefs.Save();
            return true;
        }
    }

    /**
     * Return Vector3 value saved in preferences for key `key`
     * @param string key
     * @return Vector3
     */
    public static Vector3 GetVector3(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string[] newValues = PlayerPrefs.GetString(key).Split("\n"[0]);
            if (newValues.Length == 3)
            {
                return new Vector3(float.Parse(newValues[0]), float.Parse(newValues[1]), float.Parse(newValues[2]));
            }
        }
        return Vector3.zero;
    }
    #endregion

    #region Rect
    /**
	 * Save Rect to preferences
	 * @param string key
	 * @param Rect values
	 * @return bool
	 */
    public static bool SetRect(string key, Rect value)
    {
        if (key == "keys" || key == "keyTypes")
        {
            Debug.Log("Key `keys` and `keyTypes` are used to store all keys from PlayerPrefs. Please use another key!");
            return false;
        }
        else
        {
            string[] saveValues = new string[4] { value.x.ToString(), value.y.ToString(), value.width.ToString(), value.height.ToString() };
            PlayerPrefs.SetString(key, String.Join("\n", saveValues));
            AddKey(key, "Rect");
            PlayerPrefs.Save();
            return true;
        }
    }

    /**
     * Return Rect value saved in preferences for key `key`
     * @param string key
     * @return Rect
     */
    public static Rect GetRect(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string[] newValues = PlayerPrefs.GetString(key).Split("\n"[0]);
            if (newValues.Length == 4)
            {
                return new Rect(float.Parse(newValues[0]), float.Parse(newValues[1]), float.Parse(newValues[2]), float.Parse(newValues[3]));
            }
        }
        return new Rect(0, 0, 0, 0);
    }
    #endregion
}
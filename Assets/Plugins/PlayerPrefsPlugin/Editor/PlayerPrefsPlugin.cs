/**
 * @author Nechifor Andrei Marian
 * @email neomarian@gmail.com
 * @version 1.0
 * @date 24-01-2013
 * Class used for Player Preferences Plugin.
 * This class and `PlayerPrefsPro` class need to be inside `Editor` directory from `Assets`.
 * You will find this plugin in `Window/Player Prefs Plugin` on Unity Editor.
 * Use this plugin to save new preferences.
 * You cand also edit preferences saved with this plugin or saved with `PlayerPrefsPro` class
 */

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerPrefsPlugin : EditorWindow {
	
	private int labelWidth = 70;
	
	/**
	 * Init Player Prefs Plugin
	 */
	[MenuItem ("Window/Player Prefs Plugin")]
	static void Init() {
		PlayerPrefsPlugin window = (PlayerPrefsPlugin)EditorWindow.GetWindow(typeof(PlayerPrefsPlugin));
		window.position = new Rect(100, 100, 250, 200);
        window.Show();
	}
 
	private int lastAction = 0;
	private int action = 0;
	private string[] actions = new string[3]{"New", "Edit", "Clear"};
	private int lastPrefTypeIndex = 0;
	private int newPrefTypeIndex = 0;
	private string[] types = new string[11]{"String", "Float", "Int", "Bool", "String Array", "Float Array", "Int Array", "Bool Array", "Vector2", "Vector3", "Rect"};
	
	/**
	 * Called when the user interface is created and every frame
	 * Display new preference options and edit preferences options
	 */
	void OnGUI() {
		if(lastAction != action) {		//if action is changed
			if(action == 0) {			//if action is new
				newPrefName = "";
				newPrefTypeIndex = 0;
				newPrefValSize = 1;
				newPrefVal = new string[1]{""};
			}
			else if(action == 1) {		//if action is edit
				GetPrefData();
			}
            else if (action == 2)
            {
                PlayerPrefs.DeleteAll();
                action = 0;
            }
			lastAction = action;
		}
		
		if(action == 1) {				//if action is edit
			if(lastSelectedPrefKey != selectedPrefKey) {		//if preference displayed is changed
				newPrefTypeIndex = Array.IndexOf(types, keyTypes[selectedPrefKey]);
				lastSelectedPrefKey = selectedPrefKey;
				ChangePref();
			}
		}
		
		//scroll rect
		scrollInside.Set(position.width, Mathf.Max(position.height, 160 + 25 * newPrefVal.Length));
		if(scrollInside.y > position.height) {
			scrollInside.Set(scrollInside.x - 15, scrollInside.y);
		}
		
		scroll = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), scroll, new Rect(0, 0, scrollInside.x, scrollInside.y));
			action = GUI.SelectionGrid(new Rect(5, 5, scrollInside.x - 10, 20), action, actions, 3);
			DisplayPrefGUI();
		GUI.EndScrollView();
	}
	
	//new preference vars
	private string newPrefName = "";
	private int newPrefValSize = 1;
	private string[] newPrefVal = new string[1]{""};
	private Vector2 v2Cache = Vector2.zero;
	private Vector3 v3Cache = Vector3.zero;
	private Rect rectCache = new Rect(0, 0, 0, 0);
	private int intVal = 0;
	private float floatVal = 0;
	private int i = 0;
	private Vector2 scroll = Vector2.zero;
	private Vector2 scrollInside = new Vector2(250, 200);
	
	//edit preferences vars
	private int lastSelectedPrefKey = 0;
	private int selectedPrefKey = 0;
	private string[] keys = new string[0];
	private string[] keyTypes = new string[0];
	
	/**
	 * Used for gui and functionality
	 * Options: name, type, value or size , values
	 * Buttons: sort for arrays, save and delete for edit preferences
	 */
	void DisplayPrefGUI() {
		//preference name
		GUI.SetNextControlName("PrefName");
		if(action == 0) {			//if new preference
			GUI.Label(new Rect(5, 40, labelWidth, 20), "Name");
			newPrefName = GUI.TextField(new Rect(labelWidth + 10, 40, scrollInside.x - labelWidth - 15, 16), newPrefName);
		}
		else if(action == 1) {		//if edit preferences
			GUI.Label(new Rect(5, 40, labelWidth, 20), "Name");
			selectedPrefKey = EditorGUI.Popup(new Rect(labelWidth + 10, 40, scrollInside.x - labelWidth - 15, 20), selectedPrefKey, keys);
		}
		
		//preference type
		if(action == 0) {			//if new preference
			GUI.Label(new Rect(5, 65, labelWidth, 20), "Type");
			newPrefTypeIndex = EditorGUI.Popup(new Rect(labelWidth + 10, 65, scrollInside.x - labelWidth - 15, 20), newPrefTypeIndex, types);
		}
		else if(action == 1) {		//if edit preferences
			GUI.Label(new Rect(5, 65, labelWidth, 20), "Type");
			GUI.Label(new Rect(labelWidth + 10, 65, 100, 20), types[newPrefTypeIndex]);
		}
		//in preference type is changed
		if(lastPrefTypeIndex != newPrefTypeIndex) {												
			if(types[newPrefTypeIndex] == "String" || types[newPrefTypeIndex] == "Float" || types[newPrefTypeIndex] == "Int" || types[newPrefTypeIndex] == "Bool") {
				Array.Resize(ref newPrefVal, 1);
				if(newPrefVal[0] == null) {
					if(types[newPrefTypeIndex] == "Float" || types[newPrefTypeIndex] == "Int" || types[newPrefTypeIndex] == "Bool") {
						newPrefVal[0] = "0";
					}
					else {
						newPrefVal[0] = "";
					}
				}
				
				if(types[newPrefTypeIndex] == "Float") {					//change type to float from string
					if(!float.TryParse(newPrefVal[0], out floatVal)) {
						newPrefVal[0] = "0";
					}
					else {
						newPrefVal[0] = floatVal.ToString();
					}
				}
				else if(types[newPrefTypeIndex] == "Int") {					//change type to int from string
					if(!int.TryParse(newPrefVal[0], out intVal)) {
						newPrefVal[0] = "0";
					}
					else {
						newPrefVal[0] = intVal.ToString();
					}
				}
				else if(types[newPrefTypeIndex] == "Bool") {				//change type to bool from string
					if(newPrefVal[0] != "0" && newPrefVal[0] != "1") {
						newPrefVal[0] = "0";
					}
				}
			}
			else if(types[newPrefTypeIndex] == "String Array" || types[newPrefTypeIndex] == "Float Array" || types[newPrefTypeIndex] == "Int Array" || types[newPrefTypeIndex] == "Bool Array") {
				ResizeArrayValues();
				
				if(types[newPrefTypeIndex] == "Float Array") {					//change type to float from string
					for(i=0; i<newPrefVal.Length; i++) {
						if(!float.TryParse(newPrefVal[i], out floatVal)) {
							newPrefVal[i] = "0";
						}
						else {
							newPrefVal[i] = floatVal.ToString();
						}
					}
				}
				else if(types[newPrefTypeIndex] == "Int Array") {				//change type to int from string
					for(i=0; i<newPrefVal.Length; i++) {
						if(!int.TryParse(newPrefVal[i], out intVal)) {
							newPrefVal[i] = "0";
						}
						else {
							newPrefVal[i] = intVal.ToString();
						}
					}
				}
				else if(types[newPrefTypeIndex] == "Bool Array") {				//change type to bool from string
					for(i=0; i<newPrefVal.Length; i++) {
						if(newPrefVal[i] != "0" && newPrefVal[i] != "1") {
							newPrefVal[i] = "0";
						}
					}
				}
			}
			else if(types[newPrefTypeIndex] == "Vector2" || types[newPrefTypeIndex] == "Vector3" || types[newPrefTypeIndex] == "Rect") {
				if(types[newPrefTypeIndex] == "Vector2") {
					Array.Resize(ref newPrefVal, 2);
				}
				else if(types[newPrefTypeIndex] == "Vector3") {
					Array.Resize(ref newPrefVal, 3);
				}
				else if(types[newPrefTypeIndex] == "Rect") {
					Array.Resize(ref newPrefVal, 4);
				}
				
				for(i=0; i<newPrefVal.Length; i++) {
					if(newPrefVal[i] == null) {
						newPrefVal[i] = "0";
					}
					if(!float.TryParse(newPrefVal[i], out floatVal)) {
						newPrefVal[i] = "0";
					}
					else {
						newPrefVal[i] = floatVal.ToString();
					}
				}
			}
			lastPrefTypeIndex = newPrefTypeIndex;
			GUI.FocusControl("PrefName");
		}
		
		//display value/values input/s
		switch(types[newPrefTypeIndex]) {
			case "String":						//type String
				GUI.Label(new Rect(5, 90, labelWidth, 20), "Value");
				newPrefVal[0] = GUI.TextField(new Rect(labelWidth + 10, 90, scrollInside.x - labelWidth - 15, 16), newPrefVal[0]);
				break;
			
			case "Float":						//type Float
				GUI.Label(new Rect(5, 90, labelWidth, 20), "Value");
				newPrefVal[0] = EditorGUI.FloatField(new Rect(labelWidth + 10, 90, scrollInside.x - labelWidth - 15, 16), float.Parse(newPrefVal[0])).ToString();
				break;
			
			case "Int":							//type Int
				GUI.Label(new Rect(5, 90, labelWidth, 20), "Value");
				newPrefVal[0] = EditorGUI.IntField(new Rect(labelWidth + 10, 90, scrollInside.x - labelWidth - 15, 16), int.Parse(newPrefVal[0])).ToString();
				break;
			
			case "Bool":						//type Bool
				GUI.Label(new Rect(5, 90, labelWidth, 20), "Value");
				newPrefVal[0] = EditorGUI.Toggle(new Rect(labelWidth + 10, 90, 16, 16), (newPrefVal[0] == "1") ? true : false) ? "1" : "0";
				break;
			
			case "String Array":				//type String Array
				GUI.Label(new Rect(5, 90, labelWidth, 20), "Size");
				newPrefValSize = EditorGUI.IntField(new Rect(labelWidth + 10, 90, scrollInside.x - labelWidth - 15, 16), newPrefValSize);
				ResizeArrayValues();
				
				for(i=0; i<newPrefValSize; i++) {
					GUI.Label(new Rect(15, 115 + 25 * i, labelWidth, 20), "Value " + (i + 1));
					newPrefVal[i] = GUI.TextField(new Rect(labelWidth + 20, 115 + 25 * i, scrollInside.x - labelWidth - 25, 16), newPrefVal[i]);
				}
				break;
			
			case "Float Array":					//type Float Array
				GUI.Label(new Rect(5, 90, labelWidth, 20), "Size");
				newPrefValSize = EditorGUI.IntField(new Rect(labelWidth + 10, 90, scrollInside.x - labelWidth - 15, 16), newPrefValSize);
				ResizeArrayValues();
			
				for(i=0; i<newPrefValSize; i++) {
					GUI.Label(new Rect(15, 115 + 25 * i, labelWidth, 20), "Value " + (i + 1));
					newPrefVal[i] = EditorGUI.FloatField(new Rect(labelWidth + 20, 115 + 25 * i, scrollInside.x - labelWidth - 25, 16), float.Parse(newPrefVal[i])).ToString();
				}
				break;
			
			case "Int Array":					//type Int Array
				GUI.Label(new Rect(5, 90, labelWidth, 20), "Size");
				newPrefValSize = EditorGUI.IntField(new Rect(labelWidth + 10, 90, scrollInside.x - labelWidth - 15, 16), newPrefValSize);
				ResizeArrayValues();
			
				for(i=0; i<newPrefValSize; i++) {
					GUI.Label(new Rect(15, 115 + 25 * i, labelWidth, 20), "Value " + (i + 1));
					newPrefVal[i] = EditorGUI.IntField(new Rect(labelWidth + 20, 115 + 25 * i, scrollInside.x - labelWidth - 25, 16), int.Parse(newPrefVal[i])).ToString();
				}
				break;
			
			case "Bool Array":					//type Bool Array
				GUI.Label(new Rect(5, 90, labelWidth, 20), "Size");
				newPrefValSize = EditorGUI.IntField(new Rect(labelWidth + 10, 90, scrollInside.x - labelWidth - 15, 16), newPrefValSize);
				ResizeArrayValues();
			
				for(i=0; i<newPrefValSize; i++) {
					GUI.Label(new Rect(15, 115 + 25 * i, labelWidth, 20), "Value " + (i + 1));
					newPrefVal[i] = EditorGUI.Toggle(new Rect(labelWidth + 20, 115 + 25 * i, 16, 16), (newPrefVal[i] == "1") ? true : false) ? "1" : "0";
				}
				break;
			
			case "Vector2":						//type Vector2
				v2Cache.Set(float.Parse(newPrefVal[0]), float.Parse(newPrefVal[1]));
				v2Cache = EditorGUI.Vector2Field(new Rect(5, 90, scrollInside.x - 10, 20), "Value", v2Cache);
				newPrefVal[0] = v2Cache.x.ToString();
				newPrefVal[1] = v2Cache.y.ToString();
				break;
			
			case "Vector3":						//type Vector3
				v3Cache.Set(float.Parse(newPrefVal[0]), float.Parse(newPrefVal[1]), float.Parse(newPrefVal[2]));
				v3Cache = EditorGUI.Vector3Field(new Rect(5, 90, scrollInside.x - 10, 20), "Value", v3Cache);
				newPrefVal[0] = v3Cache.x.ToString();
				newPrefVal[1] = v3Cache.y.ToString();
				newPrefVal[2] = v3Cache.z.ToString();
				break;
			
			case "Rect":						//type Rect
				rectCache.Set(float.Parse(newPrefVal[0]), float.Parse(newPrefVal[1]), float.Parse(newPrefVal[2]), float.Parse(newPrefVal[3]));
				rectCache = EditorGUI.RectField(new Rect(5, 90, scrollInside.x - 10, 20), "Value", rectCache);
				newPrefVal[0] = rectCache.x.ToString();
				newPrefVal[1] = rectCache.y.ToString();
				newPrefVal[2] = rectCache.width.ToString();
				newPrefVal[3] = rectCache.height.ToString();
				break;
		}
		
		//Sort array buttons
		if(newPrefVal.Length <= 1) {	//if type selected for preference is not an array
			GUI.enabled = false;
		}
		if(GUI.Button(new Rect(5, scrollInside.y - 50, scrollInside.x / 2 - 7.5f, 20), "Sort Asc")) {
			Array.Sort(newPrefVal);		//sort array ascendent
		}
		if(GUI.Button(new Rect(scrollInside.x / 2 + 2.5f, scrollInside.y - 50, scrollInside.x / 2 - 7.5f, 20), "Sort Desc")) {
			Array.Reverse(newPrefVal);	//sort array descendent
		}
		GUI.enabled = true;
		
		//Save new preference button
		if(newPrefName == "") {			//if preference don't have a name
			GUI.enabled = false;
		}
		if(GUI.Button(new Rect(5, scrollInside.y - 25, scrollInside.x / 2 - 7.5f, 20), "Save")) {
			SavePref();					//save preference
		}
		GUI.enabled = true;
		
		//Delete preferences button
		if(action == 0) {				//if new preference action
			GUI.enabled = false;
		}
		if(GUI.Button(new Rect(scrollInside.x / 2 + 2.5f, scrollInside.y - 25, scrollInside.x / 2 - 7.5f, 20), "Delete")) {
			DeletePref();				//delete preference
		}
		GUI.enabled = true;
	}
	
	/**
	 * save preference
	 */
	private void SavePref() {
		bool saved = false;
		
		switch (types[newPrefTypeIndex]) {
			case "String" :
				if(newPrefVal.Length > 0) {
					PlayerPrefsPro.SetString(newPrefName, newPrefVal[0]);
					saved = true;
				}
				else {
					Debug.Log("Values empty!");
				}
				break;
			case "Float" :
				if(newPrefVal.Length > 0) {
					PlayerPrefsPro.SetFloat(newPrefName, float.Parse(newPrefVal[0]));
					saved = true;
				}
				else {
					Debug.Log("Values empty!");
				}
				break;
			case "Int" :
				if(newPrefVal.Length > 0) {
					PlayerPrefsPro.SetInt(newPrefName, int.Parse(newPrefVal[0]));
					saved = true;
				}
				else {
					Debug.Log("Values empty!");
				}
				break;
			case "Bool":
				if(newPrefVal.Length > 0) {
					PlayerPrefsPro.SetBool(newPrefName, (newPrefVal[0] == "1") ? true : false);
					saved = true;
				}
				else {
					Debug.Log("Values empty!");
				}
				break;
			case "String Array" :
				if(newPrefVal.Length > 0) {
					PlayerPrefsPro.SetStringArray(newPrefName, newPrefVal);
					saved = true;
				}
				else {
					Debug.Log("Values empty!");
				}
				break;
			case "Float Array" :
				if(newPrefVal.Length > 0) {
					float[] valuesToSave = new float[newPrefVal.Length];
					for(i=0; i<newPrefVal.Length; i++) {
						valuesToSave[i] = float.Parse(newPrefVal[i]);
					}
					PlayerPrefsPro.SetFloatArray(newPrefName, valuesToSave);
					saved = true;
				}
				else {
					Debug.Log("Values empty!");
				}
				break;
			case "Int Array" :
				if(newPrefVal.Length > 0) {
					int[] valuesToSave = new int[newPrefVal.Length];
					for(i=0; i<newPrefVal.Length; i++) {
						valuesToSave[i] = int.Parse(newPrefVal[i]);
					}
					PlayerPrefsPro.SetIntArray(newPrefName, valuesToSave);
					saved = true;
				}
				else {
					Debug.Log("Values empty!");
				}
				break;
			case "Bool Array":
				if(newPrefVal.Length > 0) {
					bool[] valuesToSave = new bool[newPrefVal.Length];
					for(i=0; i<newPrefVal.Length; i++) {
						valuesToSave[i] = (newPrefVal[i] == "1") ? true : false;
					}
					PlayerPrefsPro.SetBoolArray(newPrefName, valuesToSave);
					saved = true;
				}
				else {
					Debug.Log("Values empty!");
				}
				break;
			case "Vector2":
				if(newPrefVal.Length > 0) {
					PlayerPrefsPro.SetVector2(newPrefName, v2Cache);
					saved = true;
				}
				else {
					Debug.Log("Values empty!");
				}
				break;
			case "Vector3":
				if(newPrefVal.Length > 0) {
					PlayerPrefsPro.SetVector3(newPrefName, v3Cache);
					saved = true;
				}
				else {
					Debug.Log("Values empty!");
				}
				break;
			case "Rect":
				if(newPrefVal.Length > 0) {
					PlayerPrefsPro.SetRect(newPrefName, rectCache);
					saved = true;
				}
				else {
					Debug.Log("Values empty!");
				}
				break;
		}
		
		if(saved) {
			action = 1;
		}
	}
	
	/**
	 * Get all keys saved in preferences with `PlayerPrefsPro` class with their types and treat exceptions
	 */
	private void GetPrefData() {
        PlayerPrefsPro.AddExistingKeys();
		keys = PlayerPrefsPro.GetAllKeys();
		keyTypes = PlayerPrefsPro.GetAllKeyTypes();
		if(keys.Length == 0) {
			Debug.Log("You don't have preferences saved to can edit!");
			action = 0;
		}
		else {
			selectedPrefKey = 0;
			newPrefTypeIndex = Array.IndexOf(types, keyTypes[selectedPrefKey]);
			ChangePref();
			action = 1;
		}
	}
	
	/**
	 * Change preference key
	 * Used only for edit preferences
	 */
	private void ChangePref() {
		newPrefName = keys[selectedPrefKey];
		if(types[newPrefTypeIndex] == "String") {
			newPrefVal = new string[1]{PlayerPrefsPro.GetString(keys[selectedPrefKey])};
		}
		else if(types[newPrefTypeIndex] == "Float") {
			newPrefVal = new string[1]{PlayerPrefsPro.GetFloat(keys[selectedPrefKey]).ToString()};
		}
		else if(types[newPrefTypeIndex] == "Int") {
			newPrefVal = new string[1]{PlayerPrefsPro.GetInt(keys[selectedPrefKey]).ToString()};
		}
		else if(types[newPrefTypeIndex] == "Bool") {
			newPrefVal = new string[1]{PlayerPrefsPro.GetBool(keys[selectedPrefKey]) ? "1" : "0"};
		}
		else {
			newPrefVal = PlayerPrefsPro.GetStringArray(keys[selectedPrefKey]);
		}
		newPrefValSize = newPrefVal.Length;
	}
	
	/**
	 * Delete preference selected
	 */
	private void DeletePref() {
		if(PlayerPrefsPro.DeleteKey(newPrefName)) {
			GetPrefData();
		}
	}
	
	/**
	 * Resize array which stove values for new preference
	 */
	private void ResizeArrayValues() {
		if(newPrefValSize != newPrefVal.Length) {		//if array size is changed
			Array.Resize(ref newPrefVal, Mathf.Max(newPrefValSize, 0));		//resize array
			for(int k=0; k<newPrefValSize; k++) {
				if(newPrefVal[k] == null) {
					if(types[newPrefTypeIndex] == "Float Array" || types[newPrefTypeIndex] == "Int Array" || types[newPrefTypeIndex] == "Bool Array") {
						newPrefVal[k] = "0";
					}
					else {
						newPrefVal[k] = "";
					}
				}
			}
		}
	}
}

#endif
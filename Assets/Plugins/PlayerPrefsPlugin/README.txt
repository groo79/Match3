Contact me at:
name: Nechifor Andrei Marian
email: neomarian@gmail.com
email: neom_2005@yahoo.com
facebook: https://www.facebook.com/andreimarian.nechifor

Plugin Name: PlayerPrefsPlugin
Version: 1.0
Release Date: 24 January 2013

DESCRIPTION
This is a plugin for Unity Editor to help developers who use `PlayerPrefs` class easely in their projects.
Very easy to integrate and use. Tested in Unity 3.5 and in Unity 4.0.
With this plugin you can test your project Player Preferences without writing any code for saving/editing/deleting prefs.

FEATURES
 * Player Prefs in Editor (Plugin)
 * Read/Edit/Delete Player Prefs saved with default PlayerPrefs class
 * Clear Player Prefs
 * Can save/edit/delete String, Float, Int, Bool, String Array, Floar Array, Int Array, Bool Array, Vector2, Vector3 and Rect types
 * Can sort arrays in ascending and descending order
 * Work on any version of Unity

HOW TO DO
1. Download Unity package from Unity3D Asset Store
2. Open this package in your project
3. Put `PlayerPrefsPlugin.cs` and `PlayerPrefsPro.cs` files in any `Editor` folder from `Assets` folder in your project (or leave them as they are)
4. In `Unity Editor` go on `Window` Menu and click on `Player Prefs Plugin`
5. When you click on `Edit` tab preferences saved with default PlayerPrefs class will be read
6. When you click on `Clear` tab all Player Prefs will be deleted
7. On `New` tab you need set a name for new preference, select type and fill values acording to type selected then hit `Save` and preference will be saved and you will be redirected to `Edit` tab
8. If you want to save arrays you can sort them ascendent or descendent
9. In edit tab you can change values and you can delete preferences saved with this plugin

PlayerPrefsPro class
 * This class extends PlayerPrefs functionality.
 * With this class you can set and get types: string, float, int, bool, string array, float array, int array, bool array, Vector2, Vector3, Rect.
 * Also you can get an array with all keys saved with this class and an array with types for keys saved in preferences.
 * You can add keys and keyTypes but I recommend to don't do that because these two functions is used inside all `set` functions from this class.
 
Plist class
 * This class is used for pulling Player Prefs from Mac OS
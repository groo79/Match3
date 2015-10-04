using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace UnfinityGames.Common.EditorEvents
{
	[InitializeOnLoad]
	public static class EditorEvents
	{
		static EditorEvents()
		{
			//remove the method from the event, just in case this gets called more than once
			//(it shouldn't, but better safe than sorry!)  If it's not already assigned, the removal is ignored
			EditorApplication.playmodeStateChanged -= SaveOnBeginPlay;
			EditorApplication.playmodeStateChanged += SaveOnBeginPlay;

			//Debug.Log("Added PlayMode events...");
		}

		static void SaveOnBeginPlay()
		{
			if (EditorApplication.isPlayingOrWillChangePlaymode)
			{
				SaveOnPlayEvent.Invoke();
				//Debug.Log("Saved due to Play Mode starting.");
			}
		}
	}

	//A delegate for detecting when the Unity Editor is exiting
	public delegate void EditorSavingHandler();

	public static class EditorSavingEvent
	{
		/// <summary>
		/// A public event for when the Unity Editor closes (used for saving stuff right before a close)
		/// Probably shouldn't attach any slow/resource-intensive methods to this event, since the methods
		/// may not complete before the editor is closed.
		/// </summary>
		public static event EditorSavingHandler OnEditorSave;

		public static void Invoke()
		{
			if (OnEditorSave != null)
			{
				//If the event isn't null, then invoke it here and call everything that's hooked into OnEditorSave.
				OnEditorSave();
				//Debug.Log("Finished running all methods attached to OnEditorSave");
			}
			else
			{
				//Debug.Log("OnEditorSave was Invoked, but no methods were subscribed to the event");
			}
		}
	}

	//A delegate for detecting when the Unity Editor is exiting
	public delegate void SaveOnPlayHandler();

	public static class SaveOnPlayEvent
	{
		/// <summary>
		/// A public event for when the Unity Editor switches to Play Mode (used for saving stuff right before the switch)
		/// Probably shouldn't attach any slow/resource-intensive methods to this event, since the methods
		/// may not complete before the editor switches modes.
		/// </summary>
		public static event SaveOnPlayHandler OnPlayModeStarted;

		public static void Invoke()
		{
			if (OnPlayModeStarted != null)
			{
				//If the event isn't null, then invoke it here and call everything that's hooked into OnEditorSave.
				OnPlayModeStarted();
				//Debug.Log("Finished running all methods attached to OnPlayModeStarted");
			}
			else
			{
				//Debug.Log("OnPlayModeStarted was Invoked, but no methods were subscribed to the event");
			}
		}
	}

	public class OnWillSaveHelper : UnityEditor.AssetModificationProcessor
	{
		//this is called every time the Unity Editor attempts to serialize something (scene object, etc.)
		public static string[] OnWillSaveAssets(string[] paths)
		{
			// Get the name of the scene to save.
			//string scenePath = string.Empty;//not really needed...
			string sceneName = string.Empty;

			//check all of our paths (if we even have more than one...)
			foreach (string path in paths)
			{
				//if the path contains our .unity (scene) file, set the path strings to the current path.
				if (path.Contains(".unity"))
				{
					//scenePath = Path.GetDirectoryName(path);//not really needed...
					//getting the name is as simple as removing the extension (.unity)
					sceneName = Path.GetFileNameWithoutExtension(path);
				}
			}

			//if the scene length is zero, it means we didn't get a valid path.  In this case, just exit out of
			//this method. (return our empty path string)
			if (sceneName.Length == 0)
			{
				return paths;
			}

			//Do any custom saving here, before we return our (not empty) scene paths.

			//trigger any methods that are attached to EditorSavingEvent.OnEditorSave
			EditorSavingEvent.Invoke();

			return paths;
		}
	}
}

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

namespace u2dex
{

#if !UNITY_3_5
    public class OnWillSaveHelper : UnityEditor.AssetModificationProcessor
#else //if we're unity 3.5, use the now-depreciated AssetModificationProcessor
    public class OnWillSaveHelper : AssetModificationProcessor
#endif
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


using UnityEngine;
using UnityEditor;
using System.Collections;

namespace u2dex
{
    [InitializeOnLoad]
    public static class OnPlayModeHelper
    {
        static OnPlayModeHelper()
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
}

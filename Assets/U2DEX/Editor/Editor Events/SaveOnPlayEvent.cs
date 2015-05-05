using UnityEngine;
using System.Collections;

namespace u2dex
{
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
}

using UnityEngine;
using System.Collections;

namespace u2dex
{
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
}

using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Reflection;

#if UNITY_3_5
using u2dex;
#endif

#if !UNITY_3_5
namespace u2dex
{
#endif
    /// <summary>
    /// The Transform Inspector that handles switching between our pre-made 2D Inspectors.
    /// Overrides the default Inspector, and selects the best Inspector to use for the currently selected sprite(s).
    /// </summary>

    //CanEditMultipleObjects allows multiple transforms in Selection.transforms, which
    //we'll use to detect if multiple objects are selected and react accordingly.
    [CustomEditor(typeof(Transform)), CanEditMultipleObjects]
    public class TransformInspector : Editor
    {
        Type ObjectType = null;
        tk2dTransformInspector tk2dInspector = new tk2dTransformInspector();
        orthelloTransformInspector orthelloInspector = new orthelloTransformInspector();
        unitySpriteTransformInspector unitySpriteInspector = new unitySpriteTransformInspector();

        GenericTransformInspector genericInspector = new GenericTransformInspector();

        public override void OnInspectorGUI()
        {
            Transform t = (Transform)this.target;
            
            ObjectType = TransformInspectorUtility.GetObjectType(t);

            if (!FoundNGUI() || !GlobalSnappingData.UseNGUIInspector)
            {
                //if we don't get any supported 2D object types (or we're disabled)...
                if (ObjectType == null || !GlobalSnappingData.TransformInspectorEnabled)
                {
                    //This is not an applicable object.
                    GlobalSnappingData.ApplicableObjectSelected = false;

                    //Draw the default inspector
                    DefaultTransformInspector.Draw(t);
                }
                else //we got at least one supported 2D object type, find out which one it is.
                {
                    //This *is* an applicable object.
                    GlobalSnappingData.ApplicableObjectSelected = true;

                    if (ObjectType == TransformInspectorUtility.GetType("tk2dBaseSprite"))
                    {
                        //We got 2D Toolkit, show its inspector.
                        tk2dInspector.DrawInspector(t);
                    }
                    else
                    {
                        //We got Orthello, show its inspector
                        if (ObjectType == TransformInspectorUtility.GetType("OTSprite"))
                        {
                            orthelloInspector.DrawInspector(t);
                        }
                        else
                        {
                            //We got a Unity sprite, show its inspector
                            if (ObjectType == TransformInspectorUtility.GetType("SpriteRenderer"))
                            {
                                unitySpriteInspector.DrawInspector(t);
                            }
                            else
                            {
                                //add more supported 2D stuff here...

                                //The foreach should be in the LAST else bracket, since we want to check for officially supported
                                //types BEFORE types the user entered manually...
                                foreach (string Name in GlobalSnappingData.ApplicableClasses)
                                {
                                    //Debug.Log(ObjectType.Name + " == " + Name);
                                    if (ObjectType.Name == Name)
                                    {
                                        genericInspector.DrawInspector(t);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                DrawNGUI();
            }
        }

        /// <summary>
        /// A method that returns whether the user has NGUI installed.
        /// </summary>
        /// <returns></returns>
        bool FoundNGUI()
        {
            var NGUI = TransformInspectorUtility.GetType("NGUITransformInspector");
            if (NGUI != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// A method that allows us to draw our modified NGUITransformInspector, if the user wants to.
        /// </summary>
        void DrawNGUI()
        {
            //Get the type via reflection
            var NGUI = TransformInspectorUtility.GetType("NGUITransformInspector");

            //Get the DrawInspector method
            MethodInfo NGUI_DrawInspector = NGUI.GetMethod("DrawInspector");

            //Create an instance of the NGUITransformInspector class
            var NGUIObject = Activator.CreateInstance(NGUI);

            //If we found the method (not null)...
            if (NGUI_DrawInspector != null)
            {
                NGUI_DrawInspector.Invoke(NGUIObject, new object[] { (Transform)this.target });
            }
            else //Otherwise, something horrible has gone wrong, so put an error in the console.
            {
                Debug.LogError("Catastrophic reflection failure!  Contact U2DEX support!");
            }
        }
    }
#if !UNITY_3_5
}
#endif

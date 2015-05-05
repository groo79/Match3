using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

namespace u2dex
{
    public static class GlobalSnappingData
    {
        static SerializableGlobalSnappingData PrivateSnapData;
        const string SettingsPath = "Assets/";
        const string SettingsName = u2dexInfo.AbbreviatedName + " Settings.asset";

        /// <summary>
        /// Whether or not the U2DEX 2D Transform Inspector should be enabled or not.
        /// Defaults to true (or on).
        /// </summary>
        public static bool TransformInspectorEnabled
        {
            get
            {
                //if we have some data loaded, then go ahead and set this to the loaded data
                if (PrivateSnapData != null)
                {
                    return PrivateSnapData.transformInspectorEnabled;
                }
                else //otherwise, if we don't have any data loaded, set it to the default
                {
                    return true;
                }
            }
            set
            {
                //if we're not null, set the value.  Otherwise...do nothing, I guess?
                //When we're drawing the inspector in paused play mode, the SnapData is null, so we've got to check it.
                if (PrivateSnapData != null)
                {
                    PrivateSnapData.transformInspectorEnabled = value;
                }
            }
        }

        /// <summary>
        /// Sets whether 2D snapping should be enabled on all applicable object types
        /// The default is false (or off).
        /// </summary>    
        public static bool EnableSnapping
        {
            get
            {
                //if we have some data loaded, then go ahead and set this to the loaded data
                if (PrivateSnapData != null)
                {
                    return PrivateSnapData.enableSnapping;
                }
                else //otherwise, if we don't have any data loaded, set it to the default
                {
                    return false;
                }
            }
            set
            {
                //if we're not null, set the value.  Otherwise...do nothing, I guess?
                //When we're drawing the inspector in paused play mode, the SnapData is null, so we've got to check it.
                if (PrivateSnapData != null)
                {
                    PrivateSnapData.enableSnapping = value;
                }
            }
        }

        /// <summary>
        /// Whether or not the current object that's selected has the 2D Transform Inspector applied to it.
        /// </summary>
        public static bool ApplicableObjectSelected = false;

        /// <summary>
        /// Sets whether the 2D snapping foldout should be open on all applicable object types
        /// The default is false (or off).
        /// </summary>    
        public static bool SnappingFoldoutOpen
        {
            get
            {
                //if we have some data loaded, then go ahead and set this to the loaded data
                if (PrivateSnapData != null)
                {
                    return PrivateSnapData.foldoutOpen;
                }
                else //otherwise, if we don't have any data loaded, set it to the default
                {
                    return false;
                }
            }
            set
            {
                //if we're not null, set the value.  Otherwise...do nothing, I guess?
                //When we're drawing the inspector in paused play mode, the SnapData is null, so we've got to check it.
                if (PrivateSnapData != null)
                {
                    PrivateSnapData.foldoutOpen = value;
                }
            }
        }

        /// <summary>
        /// The amount that applicable objects will snap to by default (can be a float, or an integer value)
        /// The default is 1, so it should probably be changed to better fit your project!
        /// </summary>
        public static float AmountToSnapTo
        {
            get
            {
                //if we have some data loaded, then go ahead and set this to the loaded data
                if (PrivateSnapData != null)
                {
                    return PrivateSnapData.amountToSnapTo;
                }
                else //otherwise, if we don't have any data loaded, set it to the default
                {
                    return 1;
                }
            }
            set
            {
                //if we're not null, set the value.  Otherwise...do nothing, I guess?
                //When we're drawing the inspector in paused play mode, the SnapData is null, so we've got to check it.
                if (PrivateSnapData != null)
                {
                    PrivateSnapData.amountToSnapTo = value;
                }
            }
        }

        /// <summary>
        /// Whether or not the snapping tool should automatically snap to the grid.
        /// Defaults to false (or off)
        /// </summary>
        public static bool SnapToGrid
        {
            get
            {
                //if we have some data loaded, then go ahead and set this to the loaded data
                if (PrivateSnapData != null)
                {
                    return PrivateSnapData.snapToGrid;
                }
                else //otherwise, if we don't have any data loaded, set it to the default
                {
                    return false;
                }
            }
            set
            {
                //if we're not null, set the value.  Otherwise...do nothing, I guess?
                //When we're drawing the inspector in paused play mode, the SnapData is null, so we've got to check it.
                if (PrivateSnapData != null)
                {
                    PrivateSnapData.snapToGrid = value;
                }
            }
        }

        /// <summary>
        /// The extra (unofficially supported!) classes that will have the 2D Inspector applied to them.
        /// </summary>
        //Note: Contains the 2D Toolkit, Orthello and Unity 4.3 sprites by default.
        public static List<string> ApplicableClasses = new List<string>(new[] { "tk2dBaseSprite", "OTSprite",
                                                                                "SpriteRenderer"});

        /// <summary>
        /// The readonly list containing all of the names of the classes we currently support (officially, anyway).
        /// </summary>
        public static readonly string[] SupportedClasses = new string[3] { "tk2dBaseSprite", "OTSprite",
                                                                                "SpriteRenderer"};

        /// <summary>
        /// The size of the grid areas that are displayed if the grid component is attached to a camera
        /// (or some other applicable camera type, such as a 2D Toolkit Camera)
        /// </summary>
        public static Vector2 GridSize
        {
            get
            {
                //if we're not null, set the value.  Otherwise return the default.
                //When we're drawing the inspector in paused play mode, the SnapData is null, so we've got to check it.
                if (PrivateSnapData != null)
                {
                    return PrivateSnapData.gridSize;
                }
                else
                {
                    return new Vector2(64,64);
                }
            }
            set
            {
                //if we're not null, set the value.  Otherwise...do nothing, I guess?
                //When we're drawing the inspector in paused play mode, the SnapData is null, so we've got to check it.
                if (PrivateSnapData != null)
                {
                    PrivateSnapData.gridSize = value;
                }
            }
        }

        /// <summary>
        /// The color of the grid areas that are displayed if the grid component is attached to a camera
        /// (or some other applicable camera type, such as a 2D Toolkit Camera)
        /// </summary>
        public static Color GridColor
        {
            get 
            {
                //if we're not null, set the value.  Otherwise return the default.
                //When we're drawing the inspector in paused play mode, the SnapData is null, so we've got to check it.
                if (PrivateSnapData != null)
                {
                    return PrivateSnapData.gridColor;
                }
                else
                {
                    return Color.white;
                }
            }
            set
            {
                //if we're not null, set the value.  Otherwise...do nothing, I guess?
                //When we're drawing the inspector in paused play mode, the SnapData is null, so we've got to check it.
                if (PrivateSnapData != null)
                {
                    PrivateSnapData.gridColor = value;
                }
            }
        }

        /// <summary>
        /// Whether we use Pixels Per Meter with the Grid component.
        /// Defaults to false.
        /// </summary>
        public static bool UsePixelsPerMeter
        {
            get
            {
                //if we're not null, set the value.  Otherwise return the default.
                //When we're drawing the inspector in paused play mode, the SnapData is null, so we've got to check it.
                if (PrivateSnapData != null)
                {
                    return PrivateSnapData.usePixelsPerMeter;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                //if we're not null, set the value.  Otherwise...do nothing, I guess?
                //When we're drawing the inspector in paused play mode, the SnapData is null, so we've got to check it.
                if (PrivateSnapData != null)
                {
                    PrivateSnapData.usePixelsPerMeter = value;
                }
            }
        }

        /// <summary>
        /// The number of Pixels Per Meter that will be used by the Grid component to divide the grid's size by.
        /// Defaults to 1, but feel free to set it to whatever you use in your project.
        /// </summary>
        public static float PixelsPerMeter
        {
            get
            {
                //if we're not null, set the value.  Otherwise return the default.
                //When we're drawing the inspector in paused play mode, the SnapData is null, so we've got to check it.
                if (PrivateSnapData != null)
                {
                    return PrivateSnapData.pixelsPerMeter;
                }
                else
                {
                    return 1;
                }
            }
            set
            {
                //if we're not null, set the value.  Otherwise...do nothing, I guess?
                //When we're drawing the inspector in paused play mode, the SnapData is null, so we've got to check it.
                if (PrivateSnapData != null)
                {
                    PrivateSnapData.pixelsPerMeter = value;
                }
            }
        }

        /// <summary>
        /// Whether we should use the NGUI Inspector instead of the U2DEX Inspector.
        /// This has no effect unless NGUI is installed, and the NGUISupport.unitypackage found in the U2DEX folder
        /// has been extracted.
        /// Defaults to false.
        /// </summary>
        public static bool UseNGUIInspector
        {
            get
            {
                //if we're not null, set the value.  Otherwise return the default.
                //When we're drawing the inspector in paused play mode, the SnapData is null, so we've got to check it.
                if (PrivateSnapData != null)
                {
                    return PrivateSnapData.useNGUIInspector;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                //if we're not null, set the value.  Otherwise...do nothing, I guess?
                //When we're drawing the inspector in paused play mode, the SnapData is null, so we've got to check it.
                if (PrivateSnapData != null)
                {
                    PrivateSnapData.useNGUIInspector = value;
                }
            }
        }

        /// <summary>
        /// Whether or not we have at least one grid component active.
        /// </summary>
        public static bool HaveAtLeastOneGrid = false;      

        /// <summary>
        /// Called when this addon needs to save its data.
        /// </summary>
        public static void Save()
        {
            //if the data is not null.
            if (PrivateSnapData != null)
            {
                //Set our classes...
                PrivateSnapData.SetClasses(ApplicableClasses);

                //And then save our data.
                PrivateSnapData.Save();
            }
            else //the data was null, which is...not good.  Output an error message, and try to
                //load the EditorPrefs keys, or initialize with the default values.
            {
                Debug.LogError(u2dexInfo.AbbreviatedName + " could not save it's settings, due to them being null." +  
                "  Attempting to recover by loading any keys we have, or initializing them to default values.");
                
                //Initialize our PrivateSnapData
                PrivateSnapData = new SerializableGlobalSnappingData();

                //Attempt to load our data, or initialize them to the default values.
                PrivateSnapData.Load(SupportedClasses);
            }
        }

        /// <summary>
        /// Called when this addon needs to load its data.
        /// </summary>
        public static void Load()
        {
            //Create a new PrivateSnapData if our current one is null.
            if(PrivateSnapData == null)
            PrivateSnapData = new SerializableGlobalSnappingData();
            
            //Load our data.
            PrivateSnapData.Load(SupportedClasses);

            //Set our values.
            AmountToSnapTo = PrivateSnapData.amountToSnapTo;
            EnableSnapping = PrivateSnapData.enableSnapping;
            GridSize = PrivateSnapData.gridSize;
            GridColor = PrivateSnapData.gridColor;
            PixelsPerMeter = PrivateSnapData.pixelsPerMeter;
            UsePixelsPerMeter = PrivateSnapData.usePixelsPerMeter;
            UseNGUIInspector = PrivateSnapData.useNGUIInspector;

            ApplicableClasses = PrivateSnapData.GetClasses();
        }        
    }
}

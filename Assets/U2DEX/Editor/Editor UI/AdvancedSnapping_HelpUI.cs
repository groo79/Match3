using UnityEngine;
using UnityEditor;
using System.Collections;

//if we're using unity 3.5, we need this reference.
#if UNITY_3_5
using u2dex;
#endif

//only use namespaces if we aren't using unity 3.5 (only 4.0 or greater)
#if !UNITY_3_5
namespace u2dex
{
#endif

    public class AdvancedSnapping_HelpUI : EditorWindow
    {
        static GUIStyle headingStyle;
        static GUIStyle textStyle;

        static Texture2D addClass;
        static Texture2D cameraClass;

        static Vector2 scrollPosition;

        /// <summary>
        /// Called to initialize our window after we've created it.
        /// </summary>
        public void Init()
        {
            headingStyle = new GUIStyle(EditorStyles.label);
            headingStyle.fontSize = 16;
            headingStyle.fontStyle = FontStyle.BoldAndItalic;

            textStyle = new GUIStyle(GUI.skin.box);
            textStyle.wordWrap = true;
            textStyle.alignment = TextAnchor.UpperLeft;

            addClass = AssetDatabase.LoadAssetAtPath("Assets/U2DEX/Editor/Images/AddClass.png", typeof(Texture2D)) as Texture2D;
            cameraClass = AssetDatabase.LoadAssetAtPath("Assets/U2DEX/Editor/Images/CameraClass.png", typeof(Texture2D)) as Texture2D;
        }

        /// <summary>
        /// The ONGUI method.  This handles all the drawing for this editor window.
        /// </summary>
        public void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            EditorGUILayout.LabelField("Understanding the 'Applicable Classes'", headingStyle, GUILayout.Height(25));
            GUILayout.Label("Applicable classes are a way for you (yes you!) to apply the "
                + u2dexInfo.AbbreviatedName + " 2D Transform Inspector to additional classes that are not already" +
                " officially-supported by " + u2dexInfo.CompanyName + ".\n\n" +
                "To further understand the practical use of this feature, let's have a diagram:",
                textStyle, GUILayout.ExpandWidth(true));

            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();
            GUILayout.Label(new GUIContent(addClass, "A diagram representing an added class."));
            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(15);

            GUILayout.Label("• Verification Status", headingStyle);
            GUILayout.Label("'Verification status' reflects whether the class name you entered is valid.  In our example," +
                            " the entered name is invalid, as shown by the scary, red " + u2dexGUIUtil.XMark + "." +
                            "  If the symbol had been a green " + u2dexGUIUtil.CheckMark + " instead, then that would've meant" +
                            " the class had been found, and the " + u2dexInfo.AbbreviatedName + " 2D Transform Inspector had" +
                            " successfully been applied to it.", textStyle);

            GUILayout.Space(15);

            GUILayout.Label("• Class Name (or Type)", headingStyle);
            GUILayout.Label("'Class name' represents exactly what it says on the tin.  You enter the name" +
                            " of the class or Type you would like to apply the " + u2dexInfo.AbbreviatedName +
                            " 2D Transform Inspector to and then check if it passes verification. (for an explanation" +
                            " on verification, see the above bulletpoint)\n\nFor example, if you wanted" +
                            " to add the Unity Camera to the list of classes/types that use the " +
                            u2dexInfo.AbbreviatedName + " 2D Transform Inspector, you would simply enter" +
                            " 'Camera' (without the quotes) into the text box.  It should end up looking like this:", textStyle);

            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();
            GUILayout.Label(new GUIContent(cameraClass, "A diagram representing an added Camera class."), textStyle);
            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(15);

            GUILayout.Label("• Remove Class from List", headingStyle);
            GUILayout.Label("'Remove Class from list' also does exactly what it advertises." +
                            " When the button is clicked, the class defined on that line is removed from" +
                            " the 'Applicable Classes' list and goes off to... wherever retired data goes.\n\n" +
                            "Some retirement community in Florida, I guess?", textStyle);

            GUILayout.Space(25);

            GUILayout.Label("Still need some assistance?", headingStyle);
            GUILayout.Label("No problem.  Just click the button below to visit our forums.  Feel free to" +
                            " post your question on there, and we'll see what we can do!", textStyle);

            //Forum button
            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            if (GUILayout.Button(new GUIContent("Visit our Forums!", "Click to visit our forums")))
            {
                Application.OpenURL(u2dexInfo.ForumURL);
            }
            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);

            //Close button
            EditorGUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent("Close", "Click to close this window")))
            {
                //this.Close();
            }
            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndScrollView();
        }
    }

    //only use namespaces if we aren't using unity 3.5 (only 4.0 or greater)
#if !UNITY_3_5
}
#endif

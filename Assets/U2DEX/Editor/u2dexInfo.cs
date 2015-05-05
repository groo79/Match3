using UnityEngine;
using System.Collections;
using UnityEditor;

namespace u2dex
{
    /// <summary>
    /// Contains all of the info (metadata, really) for this plugin/addon.
    /// </summary>
    public static class u2dexInfo
    {
        public static double version = 1.20;
        const string releaseType = "Release";
        const string releaseDate = "August 2014";

        public const string FullName = "Unity 2D Editor Extensions";
        public const string AbbreviatedName = "U2DEX";

        public const string ForumURL = "http://www.unfinitygames.com/interact";

        public const string CompanyName = "Unfinity Games";

        public static string ReleaseStringIdentifier(double _version)
        {
            string id = _version.ToString("0.00");
            id += " " + releaseType;
            return id;
        }

        //[MenuItem(u2dexMenu.Root + "About", false, 10253)]
        public static void AboutU2DEX()
        {
            EditorUtility.DisplayDialog("About " + FullName,
                                        "Version: "
                                        + ReleaseStringIdentifier(version) + "\n" +
                                        "Released: " + releaseDate + "\n\n" +
                                        "Copyright © 2013-2014 " + CompanyName,
                                        "Close");
        }

        //[MenuItem(u2dexMenu.Root + "Forum", false, 10252)]
        public static void OpenForum()
        {
            Application.OpenURL(ForumURL);
        }
    }
}

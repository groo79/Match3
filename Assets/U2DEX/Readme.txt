-------------------------------------------------------------------------
U2DEX: Unity 2D Editor Extensions
Copyright © 2013-2014 Unfinity Games

Last updated August 8, 2014
-------------------------------------------------------------------------

Welcome to U2DEX, and thank you for purchasing!  
This readme is split into 5 sections, Installation, Updating, Settings, Help, and the Changelog.

-------------------------------------------------------------------------

1. INSTALLATION FROM UNITY ASSET STORE

Simply download and import the package through Unity, and everything should be good-to-go.  
U2DEX will automatically detect 2D Toolkit, Orthello or Unity 4.3 Sprites if you have one (or all!) of them
installed, so there is no additional required setup work besides the initial import.

If everything imported correctly, you should be ready to use U2DEX!

1a. SPECIAL NOTE FOR NGUI USERS

NGUI also overrides the default Transform Inspector, and because of this is not compatible with U2DEX out-of-the-box.
To enable compatibility, please extract the NGUISupport.unitypackage found in the U2DEX folder in your project.
After doing so, a new option will be present on the Edit->Preferences...->U2DEX menu that will allow you to use the
NGUI Transform Inspector in place of the U2DEX Transform Inspector, if you wish to do so.

-------------------------------------------------------------------------

2. UPDATING FROM AN OLDER VERSION

There are some breaking changes from v1.10 Beta to v 1.20 Beta.  
If you're upgrading, there are some steps you need to follow.

A. Delete the U2DEX folder.  You will unfortunately lose any preferences and settings, but from here on out
   settings are not stored in a file.  (Sorry about that!)
B. Download and import the new version from the Unity Asset store (using the instructions above, if needed).
C. Replace any Grid components you had with the new u2dexGrid component (found in the same place: Component->U2DEX->Camera->Grid).
D. Configure your settings (if needed) under the new menu found at Edit->Preferences...->U2DEX.

That's it!  You should be good to go!

-------------------------------------------------------------------------

3. SETTINGS

The U2DEX Settings are located under Edit->Preferences...->U2DEX.  Additionally, there are clickable menu
buttons for our forums (great for getting help!), an "About" popup that contains version info, etc.
and an updater utility (to see if a new version is available, and to read it's changelog if it is).
Additionally, the Grid component can be found under Component->U2DEX->Camera->Grid.

There are many options that you can configure, and all of them should be pretty self-explanatory.
The only exception is "Applicable Classes".  If you're confusedabout their usage,
there is a built-in help file that you can access by clicking the "?" button on the 'Applicable Classes' toolbar.

-------------------------------------------------------------------------

4. HELP

Help is available either via email, or by the use of the Unfinity Games Forums.
When seeking help, we prefer the forums, as any questions posted there will help others with the 
same questions, but ultimately, if you need help, either avenue is absolutely fine.

Forums: http://www.unfinitygames.com/interact
Email: http://www.unfinitygames.com/contact-us/

-------------------------------------------------------------------------

5. CHANGELOg

August 2014: v1.20 Release
- Renamed the Grid component to u2dexGrid for better name continuity.  See the Updating section in the Readme to find out how this affects you.
- Renamed the GridInspector to u2dexGridInspector for better name continuity.
- Moved everything U2DEX-related to the Unity Preferences area, under U2DEX.
- Added additional settings to the new Preference menu.
- Settings and Preferences are now saved to EditorPrefs, not a settings file.  The old settings file can be deleted.
- Preferences will now follow you between projects, so you don't have to configure settings for every project.
- Separated out some Editor UI stuff so it's not tied down to an EditorWindow.  
- Fixed some typos in variable declarations.
- Rewrote the grid component so it draws a grid over the entire screen, even while zoomed out.
- Added Pixel Per Meter support to the Grid Component.
- Added a fix for an incompatibility with NGUI (unpack the NGUI Support .unitypackage).
- Added an option to use the NGUI Transform Inspector instead of the U2DEX Transform Inspector.
- Reworked the U2DEX Grid Inspector a bit.

December 2013: v1.10 Beta
- Added full support for Unity 3.x versions, along with the already-supported 4.x versions.
- Added full support for Unity's 2D Sprites! (Requires Unity 4.3)
- Added support for play-mode editing.  Everything now works if you pause and edit while a game is running.
- Fixed a rare error that could occur when using MonoDevelop on versions greater than 4.2.1.
- Bulletproofed some code that could, in theory, throw some non-descriptive errors.
- Added a utility to check if new updates are available.
- Added a utility to view the changelog for the newest update, if there is one.
- Incremented version number of the save file format (format changed slightly, all old save files will be upgraded...)
- Re-labeled some controls so they're easier to understand at a glance.
- Changed some of the Inspectors and Editor Windows to better match Unity 4.3 (Only applies if using 4.3 or greater)
- Removed the need for Orthello sprites to have a valid material before the Transform Inspector would apply.
- Added a note on Orthello sprite Inspectors if the material is null.

October 2013: v1.00 Beta
- Initial Release

-------------------------------------------------------------------------
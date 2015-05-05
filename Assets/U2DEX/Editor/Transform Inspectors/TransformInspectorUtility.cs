using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

namespace u2dex
{
    /// <summary>
    /// A utility class that houses methods for getting object type, among other things.
    /// Used with 2D Transform Inspectors.
    /// </summary>
    public static class TransformInspectorUtility
    {
        static Type ObjectType = null;

        /// <summary>
        /// Returns the object type from a given Transform.  Only returns an officially-supported (or applicable)
        /// class type.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Type GetObjectType(Transform target)
        {
            Transform t = (Transform)target;

            //check if we even have the type available before checking if it's on the object
            if (GetType("tk2dBaseSprite") != null)
            {
                if (t.gameObject.GetComponent(GetType("tk2dBaseSprite")))
                {
                    ObjectType = GetType("tk2dBaseSprite");
                    return GetType("tk2dBaseSprite");
                }
            }

              //check if we even have the type available before checking if it's on the object
            if (GetType("OTSprite") != null)
            {
                if (t.gameObject.GetComponent(GetType("OTSprite")))
                {
                    ObjectType = GetType("OTSprite");
                    return GetType("OTSprite");
                }
            }

            //check all of our applicable class types.
            foreach (string TypeName in GlobalSnappingData.ApplicableClasses)
            {
                //If the name is not null or empty...
                if (!string.IsNullOrEmpty(TypeName))
                {
                    var type = GetType(TypeName);

                    //Check if the type is null...
                    if (type != null)
                    {
                        if (t.gameObject.GetComponent(type))
                        {
                            return type;
                        }
                    }
                }
            }

            //if we make it to the bottom and we don't have a supported 2D Type, return null
            ObjectType = null;
            return null;
        }

        /// <summary>
        /// Returns the object scale, from a given Transform.  Only returns the scale for officially-supported objects.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 GetObjectScale(Transform t)
        {
            //if we don't get any supported 2D object types...
            if (ObjectType == null)
            {
                //return zero
                return Vector3.zero;
            }
            else //we got at least one supported 2D object type, find out which one it is.
            {
                if (ObjectType == GetType("tk2dBaseSprite"))
                {
                    //We got 2D Toolkit, try to use its scale.
                    return GetScaleFromClassName("tk2dBaseSprite", "scale", t);
                    //return (t.gameObject.GetComponent(typeof(tk2dBaseSprite)) as tk2dBaseSprite).scale;
                }
                else
                {
                    //We got Orthello, try to use its size.
                    if (ObjectType == GetType("OTSprite"))
                    {
                        return GetScaleFromClassName("OTSprite", "size", t);
                        //return (t.gameObject.GetComponent(typeof(OTSprite)) as OTSprite).size;
                    }
                    else
                    {
                        //add more supported 2D stuff here, but for now, return zero
                        return Vector3.zero;
                    }
                }
            }
        }

        //Adapted from http://answers.unity3d.com/questions/206665/typegettypestring-does-not-work-in-unity.html
        /// <summary>
        /// Retrieves a Type from a given string.  This works around Unity not being able to do this for some objects.
        /// </summary>
        /// <param name="TypeName"></param>
        /// <returns></returns>
        public static Type GetType(string TypeName)
        {
            //Make sure that we're not going to be checking with an empty or null string...
            if (!string.IsNullOrEmpty(TypeName))
            {
                // Try Type.GetType() first. This will work with types defined
                // by the Mono runtime, in the same assembly as the caller, etc.
                var type = Type.GetType(TypeName);

                // If it worked, then we're done here
                if (type != null)
                    return type;

                // If the TypeName is a full name, then we can try loading the defining assembly directly
                if (TypeName.Contains("."))
                {

                    // Get the name of the assembly (Assumption is that we are using
                    // fully-qualified type names)
                    var assemblyName = TypeName.Substring(0, TypeName.IndexOf('.'));

                    // Attempt to load the indicated Assembly
                    var assembly = System.Reflection.Assembly.Load(assemblyName);
                    if (assembly == null)
                        return null;

                    // Ask that assembly to return the proper Type
                    type = assembly.GetType(TypeName);
                    if (type != null)
                        return type;

                }

                // If we still haven't found the proper type, we can enumerate all of the
                // loaded assemblies and see if any of them define the type
                var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();
                var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
                foreach (var assemblyName in referencedAssemblies)
                {
                    // Load the referenced assembly
                    var assembly = System.Reflection.Assembly.Load(assemblyName);
                    if (assembly != null)
                    {
                        // See if that assembly defines the named type
                        type = assembly.GetType(TypeName);
                        if (type != null)
                            return type;
                    }
                }

                //Check if this type is in the UnityEngine namespace, since nothing explicity checked for that.
                //Only run this check if we don't have UnityEngine. in the string already...
                if (!TypeName.Contains("UnityEngine."))
                {
                    var unityName = "UnityEngine." + TypeName;

                    var unityType = GetType(unityName);

                    //if we got something, return it!
                    if (unityType != null)
                        return unityType;
                }

                // The type just couldn't be found...
                return null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Uses reflection to grab a scale (size) property from a class that may or may not exist in the user's project.
        /// </summary>
        /// <param name="ClassName"></param>
        /// <param name="ScaleName"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Vector3 GetScaleFromClassName(string ClassName, string ScaleName, Transform target)
        {
            //Reflect all the public fields in this object to search for the "scale" field
            System.Reflection.PropertyInfo[] properties = GetType(ClassName).GetProperties();

            var Scale = Vector3.one;
            // If there is a field with the name 'name' return its value
            foreach (System.Reflection.PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.Name.ToLower() == ScaleName)
                {
                    //Try it as a Vector 3...
                    try
                    {
                        Scale = (Vector3)propertyInfo.GetValue(target.gameObject.GetComponent(ClassName), null);
                    }
                    catch //Catch the (probable) InvalidCastException.  This should be Vector2.
                    {
                        Vector2 Scale2D = (Vector2)propertyInfo.GetValue(target.gameObject.GetComponent(ClassName), null);
                        Scale = new Vector3(Scale2D.x, Scale2D.y, 0);
                    }
                }
            }

            return Scale;
        }

        /// <summary>
        /// Uses reflection to grab a scale (size) property from a class that may or may not exist in the user's project,
        /// and then set a value to it.
        /// </summary>
        /// <param name="ClassName"></param>
        /// <param name="ScaleName"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static void SetScaleFromClassName(string ClassName, string ScaleName, Transform target, Vector3 Scale)
        {
            //Reflect all the public fields in this object to search for the "scale" field
            System.Reflection.PropertyInfo[] properties = GetType(ClassName).GetProperties();

            // If there is a field with the name 'name' return its value
            foreach (System.Reflection.PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.Name.ToLower() == ScaleName)
                {
                    //Try it as a Vector3...
                    try
                    {
                        propertyInfo.SetValue(target.gameObject.GetComponent(ClassName), Scale, null);
                    }
                    catch //Catch the (probable) ArgumentException.  We need to pass it a Vector2.
                    {
                        propertyInfo.SetValue(target.gameObject.GetComponent(ClassName), new Vector2(Scale.x, Scale.y), null);
                    }
                }
            }
        }
    }
}

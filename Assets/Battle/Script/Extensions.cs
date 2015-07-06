using UnityEngine;
using System;
using System.Collections.Generic;

namespace Memoria.Battle
{
    public static class Extensions
    {
        public static T1 ToEnum<T1, T2>(this T2 sender)
        {
            string s = sender.ToString().ToUpper();
            return (T1)Enum.Parse(typeof(T1), s);
        }

        public static GameObject LoadComponentsFromList(this GameObject gameObject, IList<Type> components)
        {
            for(int i = 0; i < components.Count; i++)
            {
                gameObject.AddComponent(components[i]);
            }
            return gameObject;
        }
    }
}
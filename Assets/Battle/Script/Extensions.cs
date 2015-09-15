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

        public static int[] ToArray(this int number)
        {
            int[] result = new int[GetDigitArrayLength(number)];

            for(int i = 0; i < result.Length; i++)
            {
                result[result.Length - i -1] = number % 10;
                number /= 10;
            }
            return result;
        }

        public static int GetDigitArrayLength(int number)
        {
            if(number == 0)
            {
                return 1;
            }
            return (int)Math.Ceiling(Math.Log10(number));
        }

        public static bool IsBetween<T>(this T obj, T start, T end) where T : IComparable
        {
            return obj.CompareTo(start) >= 0 && obj.CompareTo(end) <= 0;
        }
    }
}
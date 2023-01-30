﻿namespace Elite.Engine
{
    using System.Numerics;

    internal static class Extensions
    {
        internal static Vector3 Cloner(this Vector3 vec)
        {
            return new Vector3(vec.X, vec.Y, vec.Z);
        }

        internal static Vector3[] Cloner(this Vector3[] vecs)
        {
            Vector3[] newVecs = new Vector3[vecs.Length];
            for (int i = 0; i < vecs.Length; i++)
            {
                newVecs[i] = vecs[i].Cloner();
            }

            return newVecs;
        }

        internal static Vector2 ToVector2(this Vector3 vector)
        {
            return new(vector.X, vector.Y);
        }

        public static bool IsOdd(this int value)
        {
            return value % 2 != 0;
        }

        public static bool IsOdd(this float value)
        {
            return ((int)value).IsOdd();
        }
    }
}
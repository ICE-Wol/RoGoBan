using System.Runtime.InteropServices;
using UnityEngine;

namespace _Scripts.Tools {
    public static class Calc {
        //if too big, the following traces will be a rectangle.
        private const float Epsilon = 0.00001f;

        public static Quaternion EulerZ(this float z) => Quaternion.Euler(0, 0, z);
        
        public static bool Equal(this float argument1, float argument2) {
            return Mathf.Abs(argument1 - argument2) <= Epsilon;
        }
        public static bool Equal(this float argument1, float argument2, float epsilon) {
            return Mathf.Abs(argument1 - argument2) <= epsilon;
        }
        
        public static bool Equal(this Vector3 argument1, Vector3 argument2) {
            return Equal(argument1.x, argument2.x) &&
                   Equal(argument1.y, argument2.y) &&
                   Equal(argument1.z, argument2.z);
        }
        public static bool Equal(this Vector3 argument1, Vector3 argument2, float epsilon) {
            return Equal(argument1.x, argument2.x, epsilon) &&
                   Equal(argument1.y, argument2.y, epsilon) &&
                   Equal(argument1.z, argument2.z, epsilon);
        }

        public static Vector2 Deg2Dir(this float degree) {
            return new Vector2(Mathf.Cos(Mathf.Deg2Rad * degree), Mathf.Sin(Mathf.Deg2Rad * degree));
        }
        
        public static Vector3 Deg2Dir3(this float degree) {
            return new Vector3(Mathf.Cos(Mathf.Deg2Rad * degree), 
                               Mathf.Sin(Mathf.Deg2Rad * degree),
                               0f);
        }
        
        
        public static Vector2 Deg2Dir(this int degree) {
            return new Vector2(Mathf.Cos(Mathf.Deg2Rad * degree), Mathf.Sin(Mathf.Deg2Rad * degree));
        }
        
        public static Vector3 Deg2Dir3(this int degree) {
            return new Vector3(Mathf.Cos(Mathf.Deg2Rad * degree), 
                Mathf.Sin(Mathf.Deg2Rad * degree),
                0f);
        }
        
        public static Vector3 GetRandomVectorCircle(float start, float end, float rad) {
            var randDegree = Random.Range(start, end);
            var randRadius = Random.Range(0, rad);
            var offset = randDegree.Deg2Dir3() * randRadius;
            return offset;
        }

        public static Vector2 GetRandomVectorNormalized(float start, float end) {
            return GetRandomVectorCircle(start, end, 1).normalized;
        }
        
        public static Vector3 GetRandomVectorRing(float start, float end, float innerRad, float outerRad) {
            var randDegree = Random.Range(start, end);
            var randRadius = Random.Range(innerRad, outerRad);
            var offset = randDegree.Deg2Dir3() * randRadius;
            return offset;
        }
        
        /// <summary>
        /// A function which approach the current value to the target value, the closer the slower.
        /// </summary>
        /// <param name="current">Value type, the current value which is approaching the target value.</param>
        /// <param name="target">the final destination of the approach process.</param>
        /// <param name="rate">the rate of approach process, the bigger the slower.</param>
        /// <returns></returns>
        public static float ApproachValue(this float current, float target, float rate) {
            if (Mathf.Abs(current - target) >= Epsilon) {
                current -= (current - target) / rate;
                //current = (target - current) * Mathf.Exp(Time.deltaTime) + current;
            }
            else {
                current = target;
            }

            return current;
        }
        
        public static float ApproachValue(this float current, float target, float rate, float epsilon) {
            if (Mathf.Abs(current - target) >= epsilon) {
                current -= (current - target) / rate;
            }
            else {
                current = target;
            }

            return current;
        }
        
        public static Color ApproachValue(this Color current, Color target, float rate)
        {
            float r = ApproachValue(current.r, target.r, rate);
            float g = ApproachValue(current.g, target.g, rate);
            float b = ApproachValue(current.b, target.b, rate);
            float a = ApproachValue(current.a, target.a, rate);

            return new Color(r, g, b, a);
        }
        
        
        public static Vector3 ApproachValue(this Vector3 current, Vector3 target, Vector3 rate) {
            current.x = ApproachValue(current.x, target.x, rate.x);
            current.y = ApproachValue(current.y, target.y, rate.y);
            current.z = ApproachValue(current.z, target.z, rate.z);
            return current;
        }
        
        public static Vector3 ApproachValue(this Vector3 current, Vector3 target, float rate) {
            current.x = ApproachValue(current.x, target.x, rate);
            current.y = ApproachValue(current.y, target.y, rate);
            current.z = ApproachValue(current.z, target.z, rate);
            return current;
        }

        public static Vector3 ApproachValue(this Vector3 current, Vector3 target, Vector3 rate, float epsilon) {
            current.x = ApproachValue(current.x, target.x, rate.x, epsilon);
            current.y = ApproachValue(current.y, target.y, rate.y, epsilon);
            current.z = ApproachValue(current.z, target.z, rate.z, epsilon);
            return current;
        }
        
        public static Vector2 ApproachValue(this Vector2 current, Vector2 target, Vector2 rate) {
            current.x = ApproachValue(current.x, target.x, rate.x);
            current.y = ApproachValue(current.y, target.y, rate.y);
            return current;
        }
        public static Vector2 ApproachValue(this Vector2 current, Vector2 target, float rate) {
            current.x = ApproachValue(current.x, target.x, rate);
            current.y = ApproachValue(current.y, target.y, rate);
            return current;
        }
        
        public static float ApproachRef(this ref float current, float target, float rate) {
            return current = ApproachValue(current, target, rate);
        }
        
        public static float ApproachRef(this ref float current, float target, float rate, float epsilon) {
            return current = ApproachValue(current, target, rate, epsilon);
        }
        
        public static Vector3 ApproachRef(this ref Vector3 current, Vector3 target, Vector3 rate) {
            current.x = ApproachValue(current.x, target.x, rate.x);
            current.y = ApproachValue(current.y, target.y, rate.y);
            current.z = ApproachValue(current.z, target.z, rate.z);
            return current;
        }
        
        public static Vector3 ApproachRef(this ref Vector3 current, Vector3 target, float rate) {
            current.x = ApproachValue(current.x, target.x, rate);
            current.y = ApproachValue(current.y, target.y, rate);
            current.z = ApproachValue(current.z, target.z, rate);
            return current;
        }

        public static Vector3 ApproachRef(this ref Vector3 current, Vector3 target, Vector3 rate, float epsilon) {
            current.x = ApproachValue(current.x, target.x, rate.x, epsilon);
            current.y = ApproachValue(current.y, target.y, rate.y, epsilon);
            current.z = ApproachValue(current.z, target.z, rate.z, epsilon);
            return current;
        }
        
        public static Color Fade(this Color current, float rate) {
            current.a = ApproachValue(current.a, 0, rate);
            return current;
        }

        public static Color Appear(this Color current, float rate) {
            current.a = ApproachValue(current.a, 1, rate);
            return current;
        }

        public static Color SetAlpha(this Color current, float alpha) {
            if (alpha > 1f) alpha = 1f;
            if (alpha < 0f) alpha = 0f;
            current.a = alpha;
            return current;
        }
        
        #region SetVector
        
        public static Vector3 SetX(this Vector3 v3, float x) {
            return new Vector3(x, v3.y, v3.z);
        }
        public static Vector3 SetY(this Vector3 v3, float y) {
            return new Vector3(v3.x, y, v3.z);
        }
        public static Vector3 SetZ(this Vector3 v3, float z) {
            return new Vector3(v3.x, v3.y, z);
        }

        public static Vector3 SetXY(this Vector3 v3, Vector3 i) {
            return new Vector3(i.x, i.y, v3.z);
        }
        
        
        public static Vector2 SetX(this Vector2 v2, float x) {
            return new Vector2(x, v2.y);
        }
        public static Vector2 SetY(this Vector2 v2, float y) {
            return new Vector2(v2.x, y);
        }
        
        public static bool IsOutOfField(this Vector3 pos) {
            return Mathf.Abs(pos.x) > 4.2f || pos.y < -4.4f || pos.y > 4.7f;
        }
        
        #endregion
    }
}
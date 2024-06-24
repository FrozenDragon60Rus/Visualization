using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visualization
{
    public class Vector2
    {
        public float x, y;
        public Vector2()
        {
            x = zero.x;
            y = zero.y;
        }
        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public static Vector2 zero
        {
            get => new Vector2(0, 0);
        }
        public static Vector2 operator +(Vector2 vector, float x)
        {
            vector.x += x;
            vector.y += x;
            return vector;
        }
        public static Vector2 operator +(Vector2 vector1, Vector2 vector2)
        {
            vector1.x += vector2.x;
            vector1.y += vector2.y;
            return vector1;
        }
        public static Vector2 operator -(Vector2 vector, float x)
        {
            vector.x -= x;
            vector.y -= x;
            return vector;
        }
        public static Vector2 operator -(Vector2 vector1, Vector2 vector2)
        {
            vector1.x -= vector2.x;
            vector1.y -= vector2.y;
            return vector1;
        }
        public static Vector2 operator *(Vector2 vector, float x)
        {
            vector.x *= x;
            vector.y *= x;
            return vector;
        }
        public static Vector2 operator *(Vector2 vector1, Vector2 vector2)
        {
            vector1.x *= vector2.x;
            vector1.y *= vector2.y;
            return vector1;
        }
        public static Vector2 operator /(Vector2 vector, float x)
        {
            vector.x /= x;
            vector.y /= x;
            return vector;
        }
        public static Vector2 operator /(Vector2 vector1, Vector2 vector2)
        {
            vector1.x /= vector2.x;
            vector1.y /= vector2.x;
            return vector1;
        }
        public static bool operator ==(Vector2 vector, float x) =>
            (vector.x == x) &&
            (vector.y == x);
        public static bool operator ==(Vector2 vector1, Vector2 vector2) =>
            (vector1.x == vector2.x) &&
            (vector1.y == vector2.y);
        public static bool operator !=(Vector2 vector, float x) =>
            (vector.x != x) &&
            (vector.y != x);
        public static bool operator !=(Vector2 vector1, Vector2 vector2) =>
            (vector1.x != vector2.x) &&
            (vector1.y != vector2.y);

        public override int GetHashCode() =>
            BitConverter.ToInt32(BitConverter.GetBytes(x), 0) >> 
            BitConverter.ToInt32(BitConverter.GetBytes(y), 0);
        public override bool Equals(object obj) =>
            obj.GetType() == typeof(Vector2)
                ? (Vector2)obj == this 
                : false;
        public override string ToString() =>
            "(" + x + ", " + y + ")";
    }
}

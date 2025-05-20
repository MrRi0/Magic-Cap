using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameWinForm.Core
{
    public static class Vector2Extensions
    {
        public static Vector2 Normalize(this Vector2 vector)
        {
            float length = vector.Length();
            return length > 0 ? new Vector2(vector.X / length, vector.Y / length) : vector;
        }
    }
}

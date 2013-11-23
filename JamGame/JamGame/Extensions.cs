using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using JamGame.GameObjects;

namespace JamGame.Extensions
{
    public static class Extensions
    {
        public static T FindNearest<T>(this ICollection<GameObject> collection, Vector2 myPosition) where T : GameObject
        {
            GameObject nearest = collection
                .Where(o => o as T != null)
                    .FirstOrDefault(o => Vector2.Distance(o.Position, myPosition) == collection
                        .Min(a => Vector2.Distance(a.Position, myPosition)));

            return nearest as T;
        }
    }
}

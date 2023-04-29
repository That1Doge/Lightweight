﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Security.Cryptography.X509Certificates;
/// <summary>
/// Samay Shah, Derek Kasmark, Dominic Lucarini, Ryan Noyes
/// Lightweight
/// </summary>

namespace Lightweight
{
    /// <summary>
    /// Collidable Interface
    /// Object that is collidable 
    /// </summary>
    internal interface ICollidable
    {
        /// <summary>
        /// Hitbox property
        /// </summary>
       public Rectangle HitBox { get; }

        /// <summary>
        /// Intersect method to be overridden
        /// </summary>
        /// <param name="hitbox">Hitbox to be checked</param>
        /// <returns>Returns if intersected or not</returns>
       public bool Intersect(Rectangle hitbox);
    }
}

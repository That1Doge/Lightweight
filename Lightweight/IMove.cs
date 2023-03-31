using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Samay Shah, Derek Kasmark, Dominic Lucarini, Ryan Noyes
/// Lightweight
/// </summary>

namespace Lightweight
{
    /// <summary>
    /// Direction enum
    /// </summary>
    public enum Direction
    { 
        Left, 
        Right, 
        Up, 
        Down
    }

    /// <summary>
    /// /// Move interface 
    /// This object can move
    /// </summary>
    internal interface IMove
    {
        /// <summary>
        /// Determines how far an object will move in one Move method call
        /// </summary>
        public float Speed { get; }

        /// <summary>
        /// Move object in the specified direction 
        /// </summary>
        public void Move(Direction direction);
    }
}

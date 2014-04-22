/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
// ReSharper disable InconsistentNaming

namespace Codefarts.GridMapping.Common
{
    /// <summary>
    /// Used to determine a predefined axis on standard planes.
    /// </summary>
    public enum CommonAxis
    {
        /// <summary>
        /// Rotate around the z axis.
        /// </summary>
        XY = 0,                           

        /// <summary>
        /// Rotate around the y axis.
        /// </summary>
        XZ = 1, 

        /// <summary>
        /// Rotate around the x axis.
        /// </summary>
        ZY = 2
    }
}
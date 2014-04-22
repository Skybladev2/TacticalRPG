/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.GridMapping.Common
{
    using System;

    using UnityEngine;

    /// <summary>
    /// Extension methods for the <see cref="Vector3"/> type.
    /// </summary>
    public partial struct Vector3
    {
        public static Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta)
        {
            var vector3 = target - current;
            var single = vector3.Length();
            if (single <= maxDistanceDelta || Math.Abs(single - 0f) < Mathf.Epsilon)
            {
                return target;
            }
            
            return current + vector3 / single * maxDistanceDelta;
        }     
    }
}
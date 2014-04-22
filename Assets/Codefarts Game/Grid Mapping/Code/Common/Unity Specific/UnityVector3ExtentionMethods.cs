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
    public static class UnityVector3ExtentionMethods
    {
        public static float Length(this UnityEngine.Vector3 vector)
        {
            return vector.magnitude;
        }
    }
}
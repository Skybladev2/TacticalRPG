/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

namespace Codefarts.CoreProjectCode
{
    using Codefarts.CoreProjectCode.Settings;

    /// <summary>
    /// Provides various keys as global constant values for use with the settings system.
    /// </summary>
    public class CoreGlobalConstants
    {
        /// <summary>
        /// Provides a unique id key for storing and retrieving <see cref="IValues{TKey}"/> settings relating to a resources folder where the Codefarts tools will look for it's resources.
        /// </summary>
        public const string ResourceFolderKey = "CoreProjectCode.CommonResourceFolder";
    }
}

/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.GridMapping.Map2D
{
    /// <summary>
    /// Provides a resource manager service for retrieving content.
    /// </summary>
    public class ResourceManagerService
    {
        /// <summary>
        /// Holds a singleton reference to a <see cref="ResourceManagerService"/>.
        /// </summary>
        private static ResourceManagerService instance;

        /// <summary>
        /// Gets a singleton instance of a <see cref="ResourceManagerService"/> type.
        /// </summary>
        public static ResourceManagerService Instance
        {
            get
            {
                return instance ?? (instance = new ResourceManagerService());
            }
        }
    }
}

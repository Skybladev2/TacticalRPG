/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.GridMapping.Editor
{
    using System;

    using Codefarts.GridMapping.Models;

    /// <summary>
    /// Provides a model for the rule list.
    /// </summary>
    public class DrawingRuleListItem
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not the item is checked.
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// Gets or sets a list of rules associated with the model.
        /// </summary>
        public DrawRuleModel[] Rules { get; set; }

        /// <summary>
        /// Gets or sets a value indicating when the rule set was last loaded.
        /// </summary>
        /// <remarks>Used for detecting rule set changes.</remarks>
        public DateTime LastLoaded { get; set; }
    }
}

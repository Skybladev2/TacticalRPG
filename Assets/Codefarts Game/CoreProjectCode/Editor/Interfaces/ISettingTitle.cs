/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.CoreProjectCode.Interfaces
{
    using Codefarts.CoreProjectCode.Settings;

    /// <summary>
    /// Provides a interface that exposes a Title property.
    /// </summary>
    /// <remarks>Used by the <see cref="SettingsManager"/> type.</remarks>
    public interface ISettingTitle
    {
        /// <summary>
        /// Gets the title for the setting.
        /// </summary>
        string Title { get;  }
    }
}
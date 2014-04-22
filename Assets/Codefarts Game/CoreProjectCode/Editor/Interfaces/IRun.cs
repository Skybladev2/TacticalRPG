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
    using Codefarts.CoreProjectCode.Models;

    /// <summary>
    /// Provides a interface for running a <see cref="CallbackModel{T}"/> type.
    /// </summary>
    public interface IRun
    {
        /// <summary>
        /// Gets a value indicating the priority.
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Runs the <see cref="CallbackModel{T}"/> type.
        /// </summary>
        void Run();
    }
}
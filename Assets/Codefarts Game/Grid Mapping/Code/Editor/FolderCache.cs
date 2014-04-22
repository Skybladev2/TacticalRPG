namespace Codefarts.GridMapping.Editor
{
    using System;
    using System.IO;

    /// <summary>
    /// Provides a class for caching a list of folders.
    /// </summary>
    public class FolderCache
    {
        /// <summary>
        /// Used to hold a list of folder names.
        /// </summary>
        private string[] cachedFolders;

        /// <summary>
        /// Used by <see cref="UpdateCachedFolders"/> when determining whether or not to update the <see cref="cachedFolders"/> array.
        /// </summary>
        private DateTime lastFolderUpdate;

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderCache"/> class. 
        /// </summary>
        public FolderCache()
        {
            this.Options = SearchOption.AllDirectories;
        }

        /// <summary>
        /// Gets or sets the folder search options when rebuilding the cache.
        /// </summary>
        /// <remarks>The default value is <see cref="SearchOption.AllDirectories"/>.</remarks>
        public SearchOption Options { get; set; }

        /// <summary>
        /// Gets or sets the root folder where folder caching starts.
        /// </summary>
        public string RootFolder { get; set; }

        /// <summary>
        /// Gets or sets the number of seconds that must elapse before the cache is refreshed.
        /// </summary>
        /// <remarks>the default is 2 seconds.</remarks>
        public int Seconds { get; set; }

        /// <summary>
        /// Return the list of folders found under the projects Assets folder
        /// </summary>
        /// <returns>Returns a list of asset folders.</returns>
        /// <remarks>This method caches the asset folder list for X number of seconds to prevent retrieval of folders at every request. 
        /// Since requests can potentially occur many times a second caching is used to help reduce the frequency of any possible lag or UI unresponsiveness.</remarks>
        /// <seealso cref="Seconds"/>
        public string[] GetFolders()
        {
            this.Seconds = 2;
            this.UpdateCachedFolders(this.Seconds);
            return this.cachedFolders;
        }

        /// <summary>
        /// Updates the <see cref="cachedFolders"/> array with the list of folders found under the projects Assets folder.
        /// </summary>
        /// <param name="seconds">The number of seconds between updates.</param>
        private void UpdateCachedFolders(double seconds)
        {
            // check if it's time to update the list of folders
            if (DateTime.Now <= this.lastFolderUpdate + TimeSpan.FromSeconds(seconds))
            {
                return;
            }

            // get all asset folders
            this.cachedFolders = Directory.GetDirectories(this.RootFolder, "*.*", this.Options);
            for (var i = 0; i < this.cachedFolders.Length; i++)
            {
                this.cachedFolders[i] = this.cachedFolders[i].Substring(this.RootFolder.Length).Replace("\\", "/");
            }  

            // record the time of the update
            this.lastFolderUpdate = DateTime.Now;
        }
    }
}
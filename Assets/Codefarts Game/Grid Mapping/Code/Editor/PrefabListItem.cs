/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
namespace Codefarts.GridMapping.Models
{
    using UnityEngine;

    /// <summary>
    /// Provides a prefab list item
    /// </summary>
    public class PrefabListItem
    {
        /// <summary>
        /// Gets or sets a reference to a game object.
        /// </summary>
        public GameObject Prefab { get; set; }

        /// <summary>
        /// Gets or sets the weight associated with this prefab.
        /// </summary>
        /// <remarks>The higher the weight value the more likely the random selection will favor this item.</remarks>
        public int Weight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this item has been checked off. 
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Object.name"/> of the <see cref="Prefab"/> property.
        /// </summary>
        /// <remarks>>This is a wrapper property for the <see cref="Prefab"/> properties <see cref="Object.name"/> value.</remarks>
        public string Name
        {
            get
            {
                return this.Prefab == null ? null : this.Prefab.name;
            }
            
            set
            {
                // if prefab is null we just exit
                if (this.Prefab == null)
                {
                    return;
                }

                // set prefab properties name field.
                this.Prefab.name = value;
            }
        }

        /// <summary>
        /// Creates a clone of the object.
        /// </summary>
        /// <returns>Returns a clone of the object.</returns>
        public PrefabListItem Clone()
        {
            var item = new PrefabListItem();
            item.IsChecked = this.IsChecked;
            item.Name = this.Name;
            item.Prefab = this.Prefab;
            item.Weight = this.Weight;

            return item;
        }
    }
}
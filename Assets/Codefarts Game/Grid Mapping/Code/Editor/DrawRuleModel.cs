namespace Codefarts.GridMapping.Models
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    /// <summary>
    /// Provides a model for defining a drawing rule.
    /// </summary>
    public class DrawRuleModel
    {
        /// <summary>
        /// Holds the neighbor states.
        /// </summary>
        private bool[] neighbors;

        /// <summary>
        /// Holds the lower neighbor states.
        /// </summary>
        private bool[] neighborsLower;

        /// <summary>
        /// Holds the upper neighbor states.
        /// </summary>
        private bool[] neighborsUpper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawRuleModel"/> class. 
        /// </summary>
        public DrawRuleModel()
        {
            this.Enabled = true;
            this.AllowOriginal = true;
            this.NeighborsUpper = new bool[9];
            this.Neighbors = new bool[9];
            this.NeighborsLower = new bool[9];
            this.Alternates = new List<PrefabListItem>();
            this.Category = string.Empty;
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not this drawing rule can use it's <see cref="Prefab"/> as an alternate prefab to use.
        /// </summary>
        /// <remarks>If false the <see cref="GameObject"/> referenced by the <see cref="Prefab"/> property will not be used as an alternative drawing rule.</remarks>
        public bool AllowOriginal { get; set; }

        /// <summary>
        /// Gets or sets a list of alternate prefabs.
        /// </summary>
        public List<PrefabListItem> Alternates { get; set; }

        /// <summary>
        /// Gets or sets the category that the rule belongs to.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the description for the drawing rule.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this drawing rule is enabled.
        /// </summary>
        /// <remarks>If false this drawing rule should not be applied.</remarks>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the name of the drawing rule.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets an array of 9 boolean values representing the surrounding neighbors.
        /// </summary>
        public bool[] Neighbors
        {
            get
            {
                return this.neighbors;
            }

            set
            {
                if (value != null && value.Length != 9)
                {
                    throw new ArgumentException("Value must be an array length of 9.");
                }

                this.neighbors = value;
            }
        }

        /// <summary>
        /// Gets or sets an array of 9 boolean values representing the surrounding neighbors in the layer below.
        /// </summary>
        public bool[] NeighborsLower
        {
            get
            {
                return this.neighborsLower;
            }

            set
            {
                if (value != null && value.Length != 9)
                {
                    throw new ArgumentException("Value must be an array length of 9.");
                }

                this.neighborsLower = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not to check against the neighbors in the layer below.
        /// </summary>
        public bool NeighborsLowerEnabled { get; set; }

        /// <summary>
        /// Gets or sets an array of 9 boolean values representing the surrounding neighbors in the layer above.
        /// </summary>
        public bool[] NeighborsUpper
        {
            get
            {
                return this.neighborsUpper;
            }
            
            set
            {
                if (value != null && value.Length != 9)
                {
                    throw new ArgumentException("Value must be an array length of 9.");
                }

                this.neighborsUpper = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not to check against the neighbors in the layer above.
        /// </summary>
        public bool NeighborsUpperEnabled { get; set; }

        /// <summary>
        /// Gets or sets a reference to a game <see cref="GameObject"/> prefab.
        /// </summary>
        public GameObject Prefab { get; set; }

        /// <summary>
        /// Creates a clone of the object.
        /// </summary>
        /// <returns>Returns a clone of the object.</returns>
        public DrawRuleModel Clone()
        {
            var rule = new DrawRuleModel();
            rule.AllowOriginal = this.AllowOriginal;
            rule.Description = this.Description;
            rule.Enabled = this.Enabled;
            rule.Name = this.Name;
            rule.NeighborsLowerEnabled = this.NeighborsLowerEnabled;
            rule.NeighborsUpperEnabled = this.NeighborsUpperEnabled;
            rule.Prefab = this.Prefab;
            rule.neighbors = this.neighbors;
            rule.neighborsLower = this.neighborsLower;
            rule.neighborsUpper = this.neighborsUpper;

            foreach (var alternate in this.Alternates)
            {
                rule.Alternates.Add(alternate.Clone());
            }

            return rule;
        }
    }
}
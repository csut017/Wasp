﻿namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a constraint.
    /// </summary>
    public class Constraint
        : ConfigurationEntry
    {
        /// <summary>
        /// Gets or sets the absolute value.
        /// </summary>
        public string? AbsoluteValue { get; set; }

        /// <summary>
        /// Gets or sets the child id.
        /// </summary>
        public string? ChildId { get; set; }

        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        public string? Field { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether child forces should be included.
        /// </summary>
        public bool IncludeChildForces { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether child selections should be included.
        /// </summary>
        public bool IncludeChildSelections { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this modifier or condition is shared.
        /// </summary>
        public bool IsShared { get; set; }

        /// <summary>
        /// Gets or sets the percentage value.
        /// </summary>
        public string? PercentValue { get; set; }

        /// <summary>
        /// Gets or sets the repeats value.
        /// </summary>
        public string? RepeatsValue { get; set; }

        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        public string? Scope { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether should round up for the validations.
        /// </summary>
        public bool ShouldRoundUp { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public string? Type { get; set; }
    }
}
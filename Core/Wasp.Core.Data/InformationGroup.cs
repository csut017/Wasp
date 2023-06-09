﻿namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines an information group.
    /// </summary>
    public class InformationGroup
        : PublicationEntryBase
    {
        /// <summary>
        /// Gets or sets the information links.
        /// </summary>
        public List<InformationLink>? InformationLinks { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this information group is hidden or not.
        /// </summary>
        public bool? IsHidden { get; set; }

        /// <summary>
        /// Gets or sets the profiles.
        /// </summary>
        public List<Profile>? Profiles { get; set; }

        /// <summary>
        /// Gets or sets the rules.
        /// </summary>
        public List<Rule>? Rules { get; set; }
    }
}
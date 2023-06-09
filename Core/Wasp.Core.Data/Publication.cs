﻿namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a publication.
    /// </summary>
    public class Publication
        : GameSystemEntryBase
    {
        /// <summary>
        /// Gets or sets the full name of the publication.
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public override string? Name
        {
            get => FullName;
            set => FullName = value;
        }

        /// <summary>
        /// Gets or sets the date of publication.
        /// </summary>
        public string? PublicationDate { get; set; }

        /// <summary>
        /// Gets or sets the publisher name.
        /// </summary>
        public string? PublisherName { get; set; }

        /// <summary>
        /// Gets or sets the publisher URL.
        /// </summary>
        public string? PublisherUrl { get; set; }

        /// <summary>
        /// Gets or sets the short name of the publication.
        /// </summary>
        public string? ShortName { get; set; }
    }
}
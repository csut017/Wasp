namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines an entity that can have profiles.
    /// </summary>
    public interface IProfilesParent
    {
        /// <summary>
        /// Gets or sets the profiles.
        /// </summary>
        List<Profile>? Profiles { get; set; }
    }
}
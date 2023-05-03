namespace Wasp.Core.Data
{
    /// <summary>
    /// Helpers for working with <see cref="EntryBase"/> instance.
    /// </summary>
    public static class Entry
    {
        /// <summary>
        /// Generates a new identifier.
        /// </summary>
        /// <returns></returns>
        public static string GenerateId()
        {
            var guid = Guid.NewGuid();
            var id = guid.ToString("D").Substring(4, 19);
            return id;
        }
    }
}
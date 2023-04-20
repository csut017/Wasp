using System.Diagnostics;
using System.Text;
using Wasp.Core.Data;

namespace Wasp.Reports.Warhammer40K
{
    [DebuggerDisplay("{Hash}")]
    internal class UnitProfile
    {
        public string Hash { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public int Number { get; set; }

        public List<Profile> Profiles { get; } = new();

        public void GenerateHash()
        {
            var builder = new StringBuilder();
            foreach (var profile in Profiles)
            {
                builder.Append(profile.Name + ":");
                if (profile.Characteristics != null)
                {
                    var first = true;
                    foreach (var characteristic in profile.Characteristics)
                    {
                        if (!first) builder.Append(',');
                        first = false;
                        builder.Append(characteristic.Name + "=" + characteristic.Value);
                    }
                }
            }

            Hash = builder.ToString();
        }
    }
}
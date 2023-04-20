using Wasp.Core.Data;

namespace Wasp.Reports.Warhammer40K
{
    internal class UnitRule
    {
        public string Name { get; set; } = string.Empty;

        public List<Rule> Rules { get; } = new();
    }
}
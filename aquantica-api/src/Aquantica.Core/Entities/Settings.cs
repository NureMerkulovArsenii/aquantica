using Aquantica.Core.Enums;

namespace Aquantica.Core.Entities;

public class Settings : BaseEntity
{
    public string Name { get; set; }
    public string Code { get; set; }
    public SettingValueType ValueType { get; set; }
    public string Value { get; set; }
    public string Description { get; set; }
}
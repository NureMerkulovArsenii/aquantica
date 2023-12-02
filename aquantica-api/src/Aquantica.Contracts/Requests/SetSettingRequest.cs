using Aquantica.Core.Enums;

namespace Aquantica.Contracts.Requests;

public class SetSettingRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Value { get; set; }
    public SettingValueType ValueType { get; set; }
    public string Description { get; set; }
}
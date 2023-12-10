using Aquantica.Contracts.Requests;
using Aquantica.Core.DTOs;
using Aquantica.Core.DTOs.Settings;

namespace Aquantica.BLL.Interfaces;

public interface ISettingsService
{
    Task<BoolSettingDTO> GetBoolSettingAsync(int key);

    BoolSettingDTO GetBoolSetting(string code);
    Task<NumberSettingDTO> GetNumberSettingAsync(int key);
    NumberSettingDTO GetNumberSetting(string code);
    Task<StringSettingDTO> GetStringSettingAsync(int key);
    StringSettingDTO GetStringSetting(string code);
    Task<List<StringSettingDTO>> GetAllSettingsAsync();
    Task<bool> CreateSettingAsync(SetSettingRequest request);
    Task<bool> UpdateSettingAsync(SetSettingRequest request);
    Task<bool> DeleteSettingAsync(int id);
}
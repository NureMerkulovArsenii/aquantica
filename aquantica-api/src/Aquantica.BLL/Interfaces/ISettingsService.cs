using Aquantica.Contracts.Requests;
using Aquantica.Core.DTOs;

namespace Aquantica.BLL.Interfaces;

public interface ISettingsService
{
    Task<SettingDTO> GetBoolSettingAsync(int key);
    Task<SettingDTO> GetNumberSettingAsync(int key);
    Task<SettingDTO> GetStringSettingAsync(int key);
    Task<List<SettingDTO>> GetAllSettingsAsync();
    Task<bool> CreateSettingAsync(SetSettingRequest request);
    Task<bool> UpdateSettingAsync(SetSettingRequest request);
    
}
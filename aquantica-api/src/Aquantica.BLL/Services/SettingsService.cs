using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Requests;
using Aquantica.Core.DTOs;
using Aquantica.Core.Entities;
using Aquantica.Core.Enums;
using Aquantica.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Aquantica.BLL.Services;

public class SettingsService : ISettingsService
{
    private readonly IUnitOfWork _unitOfWork;

    public SettingsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<SettingDTO> GetBoolSettingAsync(int id)
    {
        var setting = await _unitOfWork.SettingsRepository.FirstOrDefaultAsync(x => x.Id == id);

        if (setting == null)
            throw new Exception($"Setting with key {id} not found");

        if (setting.ValueType != SettingValueType.Boolean)
            throw new Exception($"Setting with key {id} is not boolean");

        var res = new BoolSettingDTO()
        {
            Name = setting.Name,
            Code = setting.Code,
            Description = setting.Description,
            Value = bool.Parse(setting.Value)
        };

        return res;
    }

    public async Task<SettingDTO> GetNumberSettingAsync(int id)
    {
        var setting = await _unitOfWork.SettingsRepository.FirstOrDefaultAsync(x => x.Id == id);

        if (setting == null)
            throw new Exception($"Setting with key {id} not found");

        if (setting.ValueType != SettingValueType.Number)
            throw new Exception($"Setting with key {id} is not integer");

        var res = new NumberSettingDTO
        {
            Name = setting.Name,
            Code = setting.Code,
            Description = setting.Description,
            Value = int.Parse(setting.Value)
        };

        return res;
    }

    public async Task<SettingDTO> GetStringSettingAsync(int id)
    {
        var setting = await _unitOfWork.SettingsRepository.FirstOrDefaultAsync(x => x.Id == id);

        if (setting == null)
            throw new Exception($"Setting with key {id} not found");

        if (setting.ValueType != SettingValueType.String)
            throw new Exception($"Setting with key {id} is not string");

        var res = new StringSettingDTO
        {
            Name = setting.Name,
            Code = setting.Code,
            Description = setting.Description,
            Value = setting.Value
        };

        return res;
    }

    public async Task<List<SettingDTO>> GetAllSettingsAsync()
    {
        var settings = await _unitOfWork.SettingsRepository
            .GetAll()
            .ToListAsync();


        var res = new List<SettingDTO>();

        settings.ForEach(x =>
        {
            switch (x.ValueType)
            {
                case SettingValueType.Boolean:
                    res.Add(new BoolSettingDTO()
                    {
                        Name = x.Name,
                        Code = x.Code,
                        Description = x.Description,
                        Value = bool.Parse(x.Value)
                    });
                    break;
                case SettingValueType.Number:
                    res.Add(new NumberSettingDTO()
                    {
                        Name = x.Name,
                        Code = x.Code,
                        Description = x.Description,
                        Value = int.Parse(x.Value)
                    });
                    break;
                case SettingValueType.String:
                    res.Add(new StringSettingDTO()
                    {
                        Name = x.Name,
                        Code = x.Code,
                        Description = x.Description,
                        Value = x.Value
                    });
                    break;
            }
        });

        return res;
    }

    public async Task<bool> CreateSettingAsync(SetSettingRequest request)
    {
        var setting = await _unitOfWork.SettingsRepository.FirstOrDefaultAsync(x => x.Id == request.Id || x.Code == request.Code);

        if (setting != null)
            throw new Exception($"Setting with key {request.Code} already exists");

        var newSetting = new Settings()
        {
            Name = request.Name,
            Code = request.Code,
            Description = request.Description,
            Value = request.Value,
            ValueType = request.ValueType
        };
        
        await _unitOfWork.SettingsRepository.AddAsync(newSetting);
        await _unitOfWork.SaveAsync();
        
        return true;
    }
    
    public async Task<bool> UpdateSettingAsync(SetSettingRequest request)
    {
        var setting = await _unitOfWork.SettingsRepository.FirstOrDefaultAsync(x => x.Id == request.Id || x.Code == request.Code);

        if (setting == null)
            throw new Exception($"Setting with key {request.Code} not found");

        setting.Name = request.Name;
        setting.Description = request.Description;
        setting.Value = request.Value;
        setting.ValueType = request.ValueType;
        setting.Code = request.Code;
        
        await _unitOfWork.SaveAsync();
        
        return true;
    }
    
    public async Task<bool> DeleteSettingAsync(int id)
    {
        var setting = await _unitOfWork.SettingsRepository.ExistAsync(x => x.Id == id);

        if (!setting)
            throw new Exception($"Setting with key {id} not found");

        await _unitOfWork.SettingsRepository.DeleteAsync(x => x.Id == id);
        await _unitOfWork.SaveAsync();
        
        return true;
    }
}
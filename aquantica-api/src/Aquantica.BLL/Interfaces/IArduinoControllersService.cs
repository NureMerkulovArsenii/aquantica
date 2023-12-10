using Aquantica.Core.DTOs;
using Aquantica.Core.Entities;

namespace Aquantica.BLL.Interfaces;

public interface IArduinoControllersService
{
    void StartIrrigationIfNeeded(BackgroundJob job);
    
    void StopIrrigation(BackgroundJob job);

    void GetControllerData(BackgroundJob job);

    //void WriteSensorData(GetIrrigationSectionDTO section, SensorDataDTO data);

}
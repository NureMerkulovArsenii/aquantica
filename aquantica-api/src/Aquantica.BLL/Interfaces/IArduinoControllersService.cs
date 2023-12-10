using Aquantica.Core.DTOs;
using Aquantica.Core.Entities;

namespace Aquantica.BLL.Interfaces;

public interface IArduinoControllersService
{
    void StartIrrigationIfNeeded(BackgroundJobDTO job);
    
    void StopIrrigation(BackgroundJobDTO job);

    void GetControllerData(BackgroundJobDTO job);

    //void WriteSensorData(GetIrrigationSectionDTO section, SensorDataDTO data);

}
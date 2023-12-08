using Aquantica.Core.Entities;

namespace Aquantica.BLL.Interfaces;

public interface IArduinoControllersService
{
    void StartIrrigationIfNeeded(BackgroundJob job);
    
    void StopIrrigation(BackgroundJob job);
}
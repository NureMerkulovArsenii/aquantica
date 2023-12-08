namespace Aquantica.BLL.Interfaces;

public interface IArduinoControllersService
{
    void StartIrrigationIfNeeded(int sectionId);
    
    void StopIrrigation(int sectionId);
}
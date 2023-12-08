using Aquantica.Core.Entities;

namespace Aquantica.BLL.Interfaces;

public interface IJobHelperService
{
    void AddJobEventRecord(BackgroundJob job, bool isStart, bool isError = false);
}
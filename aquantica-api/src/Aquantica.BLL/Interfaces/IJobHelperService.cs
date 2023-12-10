using Aquantica.Core.DTOs;
using Aquantica.Core.Entities;

namespace Aquantica.BLL.Interfaces;

public interface IJobHelperService
{
    void AddJobEventRecord(BackgroundJobDTO job, bool isStart, bool isError = false, string message = null);
}
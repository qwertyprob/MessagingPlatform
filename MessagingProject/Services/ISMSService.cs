using MessagingProject.Models;

namespace MessagingProject.Services
{
    public interface ISMSService
    {
        Task<GetByMonthInfoResponseModel> GetByMonthInfo(string token);
        Task<GetByWeekInfoResponseModel> GetByWeekInfo(string token);
    }
}
using MessagingProject.Models;

namespace MessagingProject.Abstractions
{
    public interface IEmailService
    {
        Task<GetByMonthInfoResponseModel> GetByMonthInfo(string token);
        Task<GetByWeekInfoResponseModel> GetByWeekInfo(string token);
    }
}
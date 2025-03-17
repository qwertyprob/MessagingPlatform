using MessagingProject.Models;

namespace MessagingProject.Abstractions
{
    public interface IEmailService
    {
        Task<GetByMonthInfoResponseModel> GetByMonthInfo();
        Task<GetByWeekInfoResponseModel> GetByWeekInfo();
    }
}
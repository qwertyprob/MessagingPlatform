using MessagingProject.Models;
using MessagingProject.Models.SMS;

namespace MessagingProject.Services
{
    public interface ISMSService
    {
        Task<GetByMonthInfoResponseModel> GetByMonthInfo(string token);
        Task<GetByWeekInfoResponseModel> GetByWeekInfo(string token);
        Task<CampaignSmsResponseModel> GetSmsGampaign(string token);
    }
}
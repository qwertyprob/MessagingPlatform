using MessagingProject.Models;
using MessagingProject.Models.SMS;

namespace MessagingProject.Services
{
    public interface ISMSService
    {
        Task<GetByMonthInfoResponseModel> GetByMonthInfo(string token);
        Task<GetByWeekInfoResponseModel> GetByWeekInfo(string token);
        Task<CampaignSmsResponseModel> GetSmsGampaign(string token);
        Task<ESMSettingsResponseModel> GetESMSettings(string token);
        Task<BaseResponseModel> DeleteSmsCampaign(string token, int id);
        Task<CampaignModel?> GetSmsGampaignById(string token, int id);
    }
}
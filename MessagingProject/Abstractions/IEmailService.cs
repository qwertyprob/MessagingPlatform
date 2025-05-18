using MessagingProject.Models;
using MessagingProject.Models.Email;

namespace MessagingProject.Abstractions
{
    public interface IEmailService
    {
        Task<GetByMonthInfoResponseModel> GetByMonthInfo(string token);
        Task<GetByWeekInfoResponseModel> GetByWeekInfo(string token);
        Task<CampaignResponseModel> GetCampaigns(string token);
        Task<CampaignData?> GetCampaignById(string token, string id);
        Task<BaseResponseModel> DeleteCampaignById(string token, string id);
        Task<BaseResponseModel> UpsertCampaign(CampaignRequestModel request);
    }
}
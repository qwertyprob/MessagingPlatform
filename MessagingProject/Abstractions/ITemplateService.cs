using MessagingProject.Models;
using MessagingProject.Models.Email.Template;

namespace MessagingProject.Abstractions
{
    public interface ITemplateService
    {

        Task<TemplateResponseModel> GetTemplates(string token);
        Task<TemplateDataModel> GetTemplatesById(int? id);
        Task<BaseResponseModel> DeleteTemplate(int id, string token);

        Task<BaseResponseModel> UpdateTemplate(TemplateRequestModel request);
    }
}
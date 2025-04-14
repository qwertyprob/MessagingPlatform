using MessagingProject.Models.Email;

namespace MessagingProject.Abstractions
{
    public interface ITemplateService
    {

        Task<TemplateResponseModel> GetTemplates(string token);
        Task<TemplateDataModel> GetTemplatesById(int id);
    }
}
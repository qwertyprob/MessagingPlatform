using MessagingProject.Models.Email.Template;

namespace MessagingProject.Abstractions
{
    public interface ITemplateService
    {

        Task<TemplateResponseModel> GetTemplates(string token);
        Task<TemplateDataModel> GetTemplatesById(int id);
    }
}
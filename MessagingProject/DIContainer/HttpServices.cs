namespace MessagingProject.DIContainer
{
    public static class HttpServices
    {
        public static void AddHttpServices(this IServiceCollection services)
        {
            //HttpContext
            services.AddHttpContextAccessor();

            services.AddHttpClient("AuthLogin", client =>
            {
                client.BaseAddress = new Uri("https://dev.edi.md/ISAuthService/json/AuthorizeUser");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }
    }
}

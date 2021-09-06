namespace layoutlovers.Net.Emailing
{
    public interface IEmailTemplateProvider
    {
        string GetDefaultTemplate(int? tenantId);
        string GetTemplate(string name);
    }
}

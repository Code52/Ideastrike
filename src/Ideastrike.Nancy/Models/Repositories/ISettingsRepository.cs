namespace Ideastrike.Nancy.Models
{
    public interface ISettingsRepository
    {
        string Title { get; set; }
        string Name { get; set; }
        string WelcomeMessage { get; set; }
        string HomePage { get; set; }
        string GAnalyticsKey { get; set; }
        string IdeaStatusDefault { get; set; }
        string IdeaStatusChoices { get; set; }
        void Add(string key, string value);
        void Set(string key, string value);
        string Get(string key);
        void Delete(string key);
    }
}
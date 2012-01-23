using System.Linq;

namespace Ideastrike.Nancy.Models.Repositories
{
    public class SettingsRepository : ISettingsRepository
    {

        private readonly IdeastrikeContext db;

        public SettingsRepository(IdeastrikeContext db)
        {
            this.db = db;
        }

        private string _title;
        public string Title
        {
            get
            {
                if (string.IsNullOrEmpty(_title))
                    _title = Get("Title");
                return _title;
            }
            set
            {
                Set("Title", value);
                _title = value;
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                    _name = Get("Name");
                return _name;
            }
            set
            {
                Set("Name", value);
                _name = value;
            }
        }

        private string _welcomeMessage;
        public string WelcomeMessage
        {
            get
            {
                if (string.IsNullOrEmpty(_welcomeMessage))
                    _welcomeMessage = Get("WelcomeMessage");
                return _welcomeMessage;
            }
            set
            {
                Set("WelcomeMessage", value);
                _welcomeMessage = value;
            }
        }

        private string _homePage;
        public string HomePage
        {
            get
            {
                if (string.IsNullOrEmpty(_homePage))
                    _homePage = Get("HomePage");
                return _homePage;
            }
            set
            {
                Set("HomePage", value);
                _homePage = value;
            }
        }

        private string _gAnalyticsKey;
        public string GAnalyticsKey
        {
            get
            {
                if (string.IsNullOrEmpty(_gAnalyticsKey))
                    _gAnalyticsKey = Get("GAnalyticsKey");
                return _gAnalyticsKey;
            }
            set
            {
                Set("GAnalyticsKey", value);
                _gAnalyticsKey = value;
            }
        }

        private string _ideaStatusChoices;
        public string IdeaStatusChoices
        {
            get
            {
                if (string.IsNullOrEmpty(_ideaStatusChoices))
                    _ideaStatusChoices = Get("IdeaStatusChoices");
                return _ideaStatusChoices;
            }
            set
            {
                Set("IdeaStatusChoices", value);
                _ideaStatusChoices = value;
            }
        }

        private string _ideaStatusDefault;
        public string IdeaStatusDefault
        {
            get
            {
                if (string.IsNullOrEmpty(_ideaStatusDefault))
                    _ideaStatusDefault = Get("IdeaStatusDefault");
                return _ideaStatusDefault;
            }
            set
            {
                Set("IdeaStatusDefault", value);
                _ideaStatusDefault = value;
            }
        }

        public void Add(string key, string value)
        {
            db.Settings.Add(new Setting { Key = key, Value = value });
            db.SaveChanges();
        }

        public void Set(string key, string value)
        {
            var setting = db.Settings.FirstOrDefault(s => s.Key == key);
            if (setting == null)
            {
                db.Settings.Add(new Setting {Key = key, Value = value});
            }
            else
            {
                setting.Value = value;
            }

            db.SaveChanges();
        }
        public string Get(string key)
        {
            var setting = db.Settings.FirstOrDefault(s => s.Key == key);
            return setting != null ? setting.Value : "";
        }

        public void Delete(string key)
        {
            db.SaveChanges();
        }
    }
}
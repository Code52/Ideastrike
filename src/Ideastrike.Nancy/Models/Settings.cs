using System.Dynamic;

namespace Ideastrike.Nancy.Models
{
    /// <summary>
    /// A dynamic wrapper around an ISettingsRepository for retrieving, adding and updating settings.
    /// </summary>
    public class Settings : DynamicObject
    {
        private readonly ISettingsRepository _settingsRepository;

        public Settings(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var key = binder.Name;
            result = GetSetting(key);

            return true;
        }

        private string GetSetting(string key)
        {
            return _settingsRepository.Get(key);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var key = binder.Name;
            AddOrUpdateSetting(key, (string)value);

            return true;
        }

        private void AddOrUpdateSetting(string key, string value)
        {
            _settingsRepository.Set(key, value);
        }
    }
}
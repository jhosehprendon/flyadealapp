using System.Collections.Generic;

namespace Flyadeal.Interceptors.Helpers
{
    public static class SettingsHelper
    {
        internal static Dictionary<string, int> ParseSSRSettings(Dictionary<string, string> settings)
        {
            var ssrSettingsDict = new Dictionary<string, int>();
            if (settings == null || settings.Count == 0)
                return ssrSettingsDict;
            var ssrSettingsStr = settings.ContainsKey("Ssrs") ? settings["Ssrs"] : "";
            var ssrSettingsSplit = ssrSettingsStr.Split('|');
            foreach (var ssrSetting in ssrSettingsSplit)
            {
                var str = (ssrSetting ?? "").Trim();
                var ssrSettingSplit = str.Split(',');
                if (ssrSettingSplit.Length >= 3)
                {
                    var ssrCode = ssrSettingSplit[0].Trim();
                    var enabled = ssrSettingSplit[1].Trim();
                    var maxPerPaxPerSegment = ssrSettingSplit[2].Trim();
                    bool result;
                    if (bool.TryParse(enabled, out result) && result && !ssrSettingsDict.ContainsKey(ssrCode))
                    {
                        int intResult;
                        if (int.TryParse(maxPerPaxPerSegment, out intResult))
                            ssrSettingsDict.Add(ssrCode, intResult);
                    }
                }
            }
            return ssrSettingsDict;
        }

        internal static Dictionary<string, string[]> ParseCurrencyOverrideSettings(Dictionary<string, string> settings)
        {
            var currencyOverrideDict = new Dictionary<string, string[]>();
            var currencyOverrideSetting = settings != null && settings.ContainsKey("CurrencyOverride") ?
                   settings["CurrencyOverride"].Trim() : "";
            if (string.IsNullOrEmpty(currencyOverrideSetting))
                return currencyOverrideDict;
            var splitPerCurrency = currencyOverrideSetting.Split('|');
            foreach (var perCurrency in splitPerCurrency)
            {
                var split = perCurrency.Split('~');
                if (split.Length > 1)
                {
                    var currency = split[0].Trim().ToUpper();
                    if (currency.Length == 3)
                    {
                        var stations = split[1].Split(',');
                        currencyOverrideDict.Add(currency, stations);
                    }
                }
            }
            return currencyOverrideDict;
        }
    }
}

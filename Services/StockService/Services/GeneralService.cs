using System.Text.RegularExpressions;

namespace StockService.Services
{
    public class GeneralService
    {
        public static string CreateUniqueName()
        {
            Guid guid = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(guid.ToByteArray());
            GuidString = Regex.Replace(GuidString, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
            return GuidString;
        }
    }
}

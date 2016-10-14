using System;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace ayo.Static
{
    public class Validate
    {
        public static bool Json(string path)
        {
            try
            {
                var content = File.ReadAllText(path);
                JToken.Parse(content);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool IsValidFolderPath(string path)
        {
            var input = path;
            var index = input.LastIndexOf("\\", StringComparison.Ordinal);
            if (index > 0)
                input = input.Substring(0, index + 1);
            return Directory.Exists(input);
        }

        public static bool UserOptionsArValid(string str)
        {
            var Validator = new Regex(@"^[wcCsn-]+$");
            return Validator.IsMatch(str);
        }
    }
}
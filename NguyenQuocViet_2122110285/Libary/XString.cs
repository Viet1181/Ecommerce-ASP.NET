using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;

namespace NguyenQuocViet_2122110285.Library
{
    public static class XString
    {
        public static string Str_Slug(string s)
        {
            string[][] symbols = {
                new string[] { "[áàảãạăắằẳẵặâấầẩẫậ]", "a" },
                new string[] { "[đ]", "d" },
                new string[] { "[éèẻẽẹêếềểễệ]", "e" },
                new string[] { "[íìỉĩị]", "i" },
                new string[] { "[óòỏõọôốồổỗộơớờởỡợ]", "o" },
                new string[] { "[úùủũụưứừửữự]", "u" },
                new string[] { "[ýỳỷỹỵ]", "y" },
                new string[] { "[\\s'\";,]", "-" }
            };
            s = s.ToLower();
            foreach (string[] ss in symbols)
            {
                s = Regex.Replace(s, ss[0], ss[1]);
            }
            return s;
        }

        public static string ToUrlSlug(string value)
        {
            // First to lower case
            value = value.ToLowerInvariant();

            // Remove all accents
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
            value = Encoding.ASCII.GetString(bytes);

            // Replace spaces
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            // Remove invalid chars
            value = Regex.Replace(value, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);

            // Trim dashes from end
            value = value.Trim('-', '_');

            // Replace double occurrences of - or _
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }

        public static string ToStringNospace(string value)
        {
            return Regex.Replace(value, @"\s", "");
        }

        public static string ToAscii(string value)
        {
            string[][] symbols = {
                new string[] { "[áàảãạăắằẳẵặâấầẩẫậ]", "a" },
                new string[] { "[đ]", "d" },
                new string[] { "[éèẻẽẹêếềểễệ]", "e" },
                new string[] { "[íìỉĩị]", "i" },
                new string[] { "[óòỏõọôốồổỗộơớờởỡợ]", "o" },
                new string[] { "[úùủũụưứừửữự]", "u" },
                new string[] { "[ýỳỷỹỵ]", "y" },
                new string[] { "[\\s'\";,]", "-" }
            };
            value = value.ToLower();
            foreach (string[] ss in symbols)
            {
                value = Regex.Replace(value, ss[0], ss[1]);
            }

            // Replace spaces with hyphens
            value = Regex.Replace(value, @"\s+", "-");
            // Remove special characters
            value = Regex.Replace(value, @"[^a-z0-9\-]", "");
            // Remove duplicate hyphens
            value = Regex.Replace(value, @"-+", "-");
            // Remove hyphens from start and end
            value = value.Trim('-');

            return value;
        }

        public static string ToMD5(string str)
        {
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(str);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}

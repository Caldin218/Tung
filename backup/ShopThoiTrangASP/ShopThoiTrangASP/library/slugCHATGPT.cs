using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;

namespace ShopThoiTrangASP // Thay YourNamespace bằng namespace thực tế của bạn
{
    public static class SlugHelper 
    {
        public static string GenerateSlug(string phrase)
        {
            // Chuyển đổi sang chữ thường
            string str = phrase.ToLower();

            // Loại bỏ các dấu trong tiếng Việt
            str = RemoveDiacritics(str);

            // Xóa ký tự không hợp lệ
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

            // Thay thế khoảng trắng và dấu gạch ngang
            str = Regex.Replace(str, @"\s+", " ").Trim(); // Thay thế nhiều khoảng trắng thành một
            str = str.Replace(" ", "-"); // Thay khoảng trắng thành dấu gạch ngang

            return str;
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new System.Text.StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}

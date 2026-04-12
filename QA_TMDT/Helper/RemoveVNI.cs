using System.Text;
using System.Text.RegularExpressions;

namespace QA_TMDT.Helper
{
    public class RemoveVNI
    {
        public static string ConvertToUnsign(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;

            // 1. Chuẩn hóa Unicode để tách dấu ra khỏi chữ cái (VD: á -> a + ´)
            string stFormD = s.Normalize(System.Text.NormalizationForm.FormD);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 0; i < stFormD.Length; i++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[i]);
                // Loại bỏ các dấu thanh (NonSpacingMark)
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[i]);
                }
            }

            // 2. Gộp lại thành chuỗi không dấu (lúc này vẫn còn Đ/đ)
            string str = sb.ToString().Normalize(System.Text.NormalizationForm.FormC);

            // 3. Xử lý thủ công Đ/đ và chuẩn hóa khoảng trắng để tìm kiếm ổn định hơn
            str = str.Replace('đ', 'd').Replace('Đ', 'D').ToLower();
            return Regex.Replace(str, @"\s+", " ").Trim();
        }
    }
}

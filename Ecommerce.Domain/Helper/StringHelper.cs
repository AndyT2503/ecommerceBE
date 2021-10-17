using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Ecommerce.Domain.Helper
{
    public static class StringHelper
    {
        public static string ConvertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public static string GenerateSlug(string productName)
        {
            productName = productName.Trim();
            productName = ConvertToUnSign(productName);
            var slug = productName.Replace(" ", "-");
            return slug;
        }
    }
}

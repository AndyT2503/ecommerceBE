namespace Ecommerce.Domain.Helper
{
    public static class StringHelper
    {
        public static string GenerateSlug(string productName)
        {
            productName = productName.Trim();
            var slug = productName.Replace(" ", "-");
            return slug;
        }
    }
}

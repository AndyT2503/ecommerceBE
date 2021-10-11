namespace Ecommerce.Domain.Helper
{
    public static class NanoIdHelper
    {
        public static string GenerateNanoId()
        {
            return Nanoid.Nanoid.Generate(size: 10);
        }
    }
}

namespace Ecommerce.Domain.Model.Common
{
    public interface ISoftDeleted
    {
        public bool IsDeleted { get; set; }
    }
}

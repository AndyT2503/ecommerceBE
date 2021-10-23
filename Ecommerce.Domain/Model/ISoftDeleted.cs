namespace Ecommerce.Domain.Model
{
    public interface ISoftDeleted
    {
        public bool IsDeleted { get; set; }
    }
}

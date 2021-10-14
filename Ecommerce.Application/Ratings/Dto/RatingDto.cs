using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Ratings.Dto
{
    public class RatingDto
    {
        public string Comment { get; init; }
        public int Rate { get; init; }
        public string ImageUrl { get; init; }
        public DateTime CreateAt { get; init; }
        public string UserName { get; init; }
    }
}

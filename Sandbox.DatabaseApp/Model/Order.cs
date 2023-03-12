using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sandbox.DatabaseApp.Model
{
    [Table("Orders")]
    internal class Order
    {
        [Key]
        public int Id { get; set; }
        public IList<Product> Products { get; set; }

        public Order()
        {
            Products = new List<Product>();
        }

        public Order(int id, IEnumerable<Product> products)
        {
            Id = id;
            Products = products.ToList();
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sandbox.DatabaseApp.Model
{
    [Table("Products")]
    internal class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public Product()
        {
            Name = string.Empty;
        }

        public Product(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

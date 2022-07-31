// CategoryItem -- a many-to-many necessary class that will hold information about the relationship between both-- named alphabetically.

namespace ToDoList.Models
{
  public class CategoryItem
    {       
        public int CategoryItemId { get; set; }
        public int ItemId { get; set; }
        public int CategoryId { get; set; }
        public virtual Item Item { get; set; }
        public virtual Category Category { get; set; }
    }
}
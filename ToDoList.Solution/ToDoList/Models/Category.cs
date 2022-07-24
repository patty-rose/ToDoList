using System.Collections.Generic;

namespace ToDoList.Models
{
    public class Category
    {
        public Category()
        {
            this.Items = new HashSet<Item>();
        }//HashSet is an unordered collection of unique elements. We use INSTEAD OF a list to avoid exceptions when no records exist yet. HashSet cannot have duplicates.

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Item> Items { get; set; }//Entity requires ICollection specifically. By declaring Items as an ICollection<Item> data type, we're ensuring Entity will be able to use all the ICollection methods it requires on the Item objects in order to act as our ORM and we wont have to manually interact with our database. Virtual allows Entity to use LazyLoading. 
    }
}
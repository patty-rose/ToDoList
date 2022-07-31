using System.Collections.Generic;

namespace ToDoList.Models
{
    public class Category
    {
        public Category()
        {
            this.JoinEntities = new HashSet<CategoryItem>();
        }//With a many to many relationship we need to include a Navigation Property. a property in 1 class that includes a reference to a related class.(for one to many we make a similar reference but to a single entitty and it's called a reference navigation property). This Constructor method links to the JoinEntities property in the model. If we don't have this reference, we won't be able to access related Items in our controllers and views.
        
        //HashSet is an unordered collection of unique elements. We use INSTEAD OF a list to avoid exceptions when no records exist yet. HashSet cannot have duplicates.

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<CategoryItem> JoinEntities { get; set; }//Entity requires ICollection specifically. By declaring Items as an ICollection<Item> data type, we're ensuring Entity will be able to use all the ICollection methods it requires on the Item objects in order to act as our ORM and we wont have to manually interact with our database. Virtual allows Entity to use LazyLoading. 
    }
}
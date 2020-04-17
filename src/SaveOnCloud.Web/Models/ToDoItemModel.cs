using System.ComponentModel.DataAnnotations;
using SaveOnCloud.Core.Entities;

namespace SaveOnCloud.Web.Models
{
    // Note: doesn't expose events or behavior
    public class ToDoItemModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; private set; }

        public static ToDoItemModel FromToDoItem(ToDoItem item)
        {
            return new ToDoItemModel()
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                IsDone = item.IsDone
            };
        }
    }
}

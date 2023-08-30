using System.ComponentModel.DataAnnotations;

namespace ToDoList.Domain.ViewModels.Task
{
    public class TaskViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Display(Name = "Status")]
        public string? IsDone { get; set; }

        [Display(Name = "Priority")]
        public string Priority { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Created date")]
        public string? Created { get; set; }
    }
}

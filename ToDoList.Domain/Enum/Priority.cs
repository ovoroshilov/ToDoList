using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ToDoList.Domain.Enum
{
    public enum Priority
    {
        [Display(Name = "Easy")]
        Easy = 1,
        [Display(Name = "Medium")]
        Medium = 2,
        [Display(Name = "Hard")]
        Hard = 3
    }
}

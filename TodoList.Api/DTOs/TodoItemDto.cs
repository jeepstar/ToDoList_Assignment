using System;

namespace TodoList.Api.DTOs
{
    public class TodoItemDto
    {
        
        public string Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}

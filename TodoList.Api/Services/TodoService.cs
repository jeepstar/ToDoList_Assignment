using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using TodoList.Api.Data;
using TodoList.Api.DTOs;
using TodoList.Api.Models;

namespace TodoList.Api.Services
{
    public class TodoService
    {
        private readonly TodoContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<TodoItem> _todoItemValidator;
        private readonly ILogger<TodoService> _logger;

        public TodoService(TodoContext context, IMapper mapper,
            IValidator<TodoItem> todoItemValidator, ILogger<TodoService> logger)
        {
            _context = context;
            _mapper = mapper;
            _todoItemValidator = todoItemValidator;
            _logger = logger;
        }

        public async Task<TodoItem> CreateTodoItemAsync(TodoItemDto todoItemDto)
        {
            var todoItem = _mapper.Map<TodoItem>(todoItemDto);

            var validationResult = await _todoItemValidator.ValidateAsync(todoItem);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException("Invalid input for creating a to-do item.");
            }

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

        public async Task<TodoItem> UpdateTodoItemAsync(Guid id, TodoItemDto todoItemDto)
        {
            var existingTodoItem = await _context.TodoItems.FindAsync(id);
            if (existingTodoItem == null)
            {
                throw new ArgumentException("Todo item not found.");
            }

            // Update properties based on todoItemDto
            existingTodoItem.Description = todoItemDto.Description;
            existingTodoItem.IsCompleted = todoItemDto.IsCompleted;

            await _context.SaveChangesAsync();
            return existingTodoItem;
        }

        // Add other methods for retrieving, deleting, etc.
    }
}

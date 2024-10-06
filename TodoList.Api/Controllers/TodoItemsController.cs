using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoList.Api.Data;
using TodoList.Api.DTOs;
using TodoList.Api.Models;
using TodoList.Api.Services; 

namespace TodoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly ILogger<TodoItemsController> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<TodoItem> _todoItemValidator;
        private readonly TodoService _todoService; // Inject TodoService

        public TodoItemsController(TodoContext context, ILogger<TodoItemsController> logger,
            IMapper mapper, IValidator<TodoItem> todoItemValidator, TodoService todoService)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _todoItemValidator = todoItemValidator;
            _todoService = todoService; // Initialize TodoService
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<IActionResult> GetTodoItems()
        {
            var results = await _context.TodoItems.Where(x => !x.IsCompleted).ToListAsync();
            return Ok(results);
        }

        // GET: api/TodoItems/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem(Guid id)
        {
            var result = await _context.TodoItems.FindAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // PUT: api/TodoItems/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(Guid id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemIdExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TodoItems
        [HttpPost]
        public async Task<IActionResult> PostTodoItem(TodoItemDto todoItemDto)
        {
            try
            {
                var createdTodoItem = await _todoService.CreateTodoItemAsync(todoItemDto);
                return CreatedAtAction(nameof(GetTodoItem), new { id = createdTodoItem.Id }, createdTodoItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating a new to-do item");
                return StatusCode(500, "An error occurred while creating the to-do item.");
            }
        }

        private bool TodoItemIdExists(Guid id)
        {
            return _context.TodoItems.Any(x => x.Id == id);
        }

        private bool TodoItemDescriptionExists(string description)
        {
            return _context.TodoItems
                .Any(x => x.Description.ToLowerInvariant() == description.ToLowerInvariant() && !x.IsCompleted);
        }
    }
}

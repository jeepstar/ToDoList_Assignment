using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using TodoList.Api.Controllers;
using TodoList.Api.Data;
using TodoList.Api.Models;
using TodoList.Api.Services;
using Xunit;

namespace TodoList.Api.UnitTests
{
    public class TodoItemsControllerTests
    {
        [Fact]
        public async Task GetTodoItems_ReturnsListOfTodoItems()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new TodoContext(options))
            {
                context.TodoItems.Add(new TodoItem { Id = Guid.NewGuid(), Description = "Task 1", IsCompleted = false });
                context.TodoItems.Add(new TodoItem { Id = Guid.NewGuid(), Description = "Task 2", IsCompleted = true });
                context.SaveChanges();
            }

            var mockContext = new Mock<TodoContext>();
            mockContext.Setup(c => c.TodoItems).ReturnsDbSet(todoItems);

            var mockTodoService = new Mock<TodoService>(mockContext.Object, null, null, null);
            var controller = new TodoItemsController(mockContext.Object, null, null, null, mockTodoService.Object);

            // Act
            var result = await controller.GetTodoItems();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedItems = Assert.IsAssignableFrom<IEnumerable<TodoItem>>(okResult.Value);
            Assert.Equal(2, returnedItems.Count());
        
        }

        [Fact]
        public async Task GetTodoItem_ExistingId_ReturnsTodoItem()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var todoItem = new TodoItem { Id = itemId, Description = "Task 1", IsCompleted = false };

            var mockContext = new Mock<TodoContext>();
            mockContext.Setup(c => c.TodoItems.FindAsync(itemId)).ReturnsAsync(todoItem);

            var mockTodoService = new Mock<TodoService>(mockContext.Object, null, null, null);
            var controller = new TodoItemsController(mockContext.Object, null, null, null, mockTodoService.Object);

            // Act
            var result = await controller.GetTodoItem(itemId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedItem = Assert.IsAssignableFrom<TodoItem>(okResult.Value);
            Assert.Equal(itemId, returnedItem.Id);
        }
    }
}

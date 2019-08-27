using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Data;

namespace AspNetCoreTodo.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext _dbContext;

        public TodoItemService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TodoItem[]> GetIncompleteItemsAnysc()
        {
            return await _dbContext.Items.Where(p => !p.IsDone).ToArrayAsync();
        }

        public async Task<bool> AddItemAsync(TodoItem newItem)
        {
            newItem.Id = Guid.NewGuid();
            newItem.IsDone = false;
            newItem.DueAt = DateTimeOffset.Now.AddDays(3);
            _dbContext.Items.Add(newItem);
            var saveResult = await _dbContext.SaveChangesAsync();
            return saveResult == 1;
        }

    }
}
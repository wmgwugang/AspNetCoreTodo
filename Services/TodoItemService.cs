using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Data;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTodo.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext _dbContext;

        public TodoItemService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TodoItem[]> GetIncompleteItemsAnysc(IdentityUser user)
        {
            return await _dbContext.Items.Where(p => !p.IsDone && p.UserId == user.Id).ToArrayAsync();
        }

        public async Task<bool> AddItemAsync(TodoItem newItem, IdentityUser user)
        {
            newItem.Id = Guid.NewGuid();
            newItem.IsDone = false;
            newItem.DueAt = DateTimeOffset.Now.AddDays(3);
            newItem.UserId = user.Id;
            _dbContext.Items.Add(newItem);
            var saveResult = await _dbContext.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<bool> MarkDoneAsync(Guid id, IdentityUser user)
        {
            var item = await _dbContext.Items
                .Where(x => x.Id == id && x.UserId == user.Id)
                .SingleOrDefaultAsync();

            if (item == null) return false;

            item.IsDone = true;

            var saveResult = await _dbContext.SaveChangesAsync();
            return saveResult == 1; // One entity should have been updated
        }


    }
}
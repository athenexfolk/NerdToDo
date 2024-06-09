﻿using Microsoft.AspNetCore.Mvc;
using NerdToDo.Models;
using NerdToDo.Services;

namespace NerdToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController(TodosService todosService) : ControllerBase
    {
        private readonly TodosService _todosService = todosService;

        [HttpGet]
        public async Task<List<Todo>> Get() =>
            await _todosService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Todo>> Get(string id)
        {
            var todo = await _todosService.GetAsync(id);

            if (todo is null)
            {
                return NotFound();
            }

            return todo;
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> Post(CreateTodoDto newTodo)
        {
            var createdTodo = await _todosService.CreateAsync(newTodo);

            return CreatedAtAction(nameof(Get), new { id = createdTodo?.Id }, createdTodo);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, UpdateTodoDto updatedTodo)
        {
            var todo = await _todosService.GetAsync(id);

            if (todo is null)
            {
                return NotFound();
            }

            await _todosService.UpdateAsync(id, updatedTodo);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var todo = await _todosService.GetAsync(id);

            if (todo is null)
            {
                return NotFound();
            }

            await _todosService.RemoveAsync(id);

            return NoContent();
        }
    }
}

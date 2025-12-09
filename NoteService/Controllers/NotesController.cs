using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteService.Models.DTOs;
using NoteService.Services.Interfaces;

namespace NoteService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly INoteServices _service;

        public NotesController(INoteServices service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<NoteDto>>> GetAll()
        {
            var notes = await _service.GetAllAsync();
            return Ok(notes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NoteDto>> GetById(string id)
        {
            var note = await _service.GetByIdAsync(id);
            if (note is null) return NotFound();
            return Ok(note);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<List<NoteDto>>> GetByPatientId(int patientId)
        {
            var notes = await _service.GetByPatientIdAsync(patientId);
            return Ok(notes);
        }

        [HttpPost]
        public async Task<ActionResult<NoteDto>> Create(CreateNoteDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<NoteDto>> Update(string id, UpdateNoteDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated is null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}

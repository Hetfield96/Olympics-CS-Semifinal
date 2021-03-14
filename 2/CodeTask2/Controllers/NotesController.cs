using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeTask2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace CodeTask2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly NotesDbContext _context;
        public NotesController(NotesDbContext dbContext)
        {
            _context = dbContext;
        }

        [Route(""), HttpGet]
        public IActionResult Get()
        {
            var res = ReplaceTitle(_context.Notes.ToList());
            return Ok(res);
        }
        
        [HttpGet("{id:int}")]
        // [Route("{id}", Name = "GetNote")]
        public IActionResult GetNote([FromRoute] int id)
        {
            Note note = _context.Notes.FirstOrDefault(n => n.Id == id);
            if (note != null)
            {
                return Ok(ReplaceTitle(note));
            }
            else
            {
                return NotFound();
            }
        }
        
        [HttpGet]
        [Route("query")]
        public IActionResult GetNoteByQuery([FromQuery] string query)
        {
            List<Note> notes = _context.Notes
                .Where(n => n.Title.Contains(query) || n.Content.Contains(query))
                .ToList();
            if (notes.Count != 0)
            {
                return Ok(ReplaceTitle(notes));
            }
            else
            {
                return NotFound();
            }
        }
        
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteNote(int id)
        {
            Note note = _context.Notes.FirstOrDefault(n => n.Id == id);
            if (note != null)
            {
                _context.Notes.Remove(note);
                _context.SaveChanges();
                return Ok();
            }

            return NotFound();
        }
        
        [HttpPut]
        public IActionResult Edit([FromBody] Note note)
        {
            Note noteDb = _context.Notes.FirstOrDefault(n => n.Id == note.Id);
            if (noteDb != null)
            {
                noteDb.Title = note.Title;
                noteDb.Content = note.Content;
                _context.SaveChanges();
                return Ok(noteDb);
            }

            return NotFound();
        }
        
        [HttpPost]
        public IActionResult Create([FromBody] Note note)
        {
            _context.Notes.Add(note);
            _context.SaveChanges();
            return Ok(note);
        }

        private Note ReplaceTitle(Note note)
        {
            var res = ReplaceTitle(new List<Note>() {note});
            return res.First();
        }

        private List<Note> ReplaceTitle(List<Note> notes)
        {
            foreach (var note in notes)
            {
                if (string.IsNullOrEmpty(note.Title))
                {
                    note.Title = note.Content.Substring(0,  Math.Min(note.Content.Length, ConfigFile.N));
                }
            }

            return notes;
        }
    }
}
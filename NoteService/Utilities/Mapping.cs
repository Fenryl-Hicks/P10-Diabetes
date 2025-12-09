using System;
using MongoDB.Bson;
using NoteService.Models.DTOs;
using NoteService.Models.Entities;

namespace NoteService.Utilities
{
    public static class Mapping
    {
        public static NoteDto ToNoteDto(Note note)
        {
            return new NoteDto
            {
                Id = note.Id!,
                PatientId = note.PatientId,
                Content = note.Content
            };
        }

        public static Note ToNote(CreateNoteDto dto)
        {
            return new Note
            {
                Id = ObjectId.GenerateNewId().ToString(),
                PatientId = dto.PatientId,
                Content = dto.Content,
                IsDeleted = false
            };
        }

        public static void UpdateNoteFromDto(Note note, UpdateNoteDto dto)
        {
            note.Content = dto.Content;
        }
    }
}


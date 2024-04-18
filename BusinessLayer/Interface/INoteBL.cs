using ModelLayer.DTO;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface INoteBL
    {
        public Task<List<Note>> GetAll(int userId);

        public Note CreateNote(CreateNoteRequest Request, int UserId);

        public Task<int> UpdateNote(UpdateNoteRequest Request, int UserId,int NoteId);

        public Task<int> DeleteNote(int NoteId,int UserId);

        public Task<string> isArchived(int NoteId, int UserId);
        public Task<string> isDeleted(int NoteId, int UserId);

    }
}

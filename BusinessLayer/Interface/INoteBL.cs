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

        public int UpdateNote(UpdateNoteRequest Request, int UserId,int NoteId);

        public int DeleteNote(int NoteId);

        public bool isArchived(int NoteId);
        public bool isDeleted(int NoteId);

    }
}

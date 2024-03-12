using ModelLayer.DTO;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface INoteRL
    {

        public Task<List<Note>> GetAll();

        public int CreateNote(CreateNoteRequest Request, int UserId);

        public int UpdateNote(UpdateNoteRequest Request, int UserId,int NoteId);

        public int DeleteNote(int NoteId);

        public bool isArchived(int NoteId);

        public bool isDeleted(int NoteId);
    }
}

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
    }
}

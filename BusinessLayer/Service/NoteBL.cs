using BusinessLayer.Interface;
using ModelLayer.DTO;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class NoteBL:INoteBL
    {   
        private readonly INoteRL service;

        public NoteBL(INoteRL _service)
        {
            this.service = _service;
        }
        public Task<List<Note>> GetAll()
        {
            return service.GetAll();
        }
        
        public int CreateNote(CreateNoteRequest Request, int UserId)
        {
            return service.CreateNote(Request,UserId);
        }

    }
}

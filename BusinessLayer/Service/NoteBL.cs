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
        public Task<List<Note>> GetAll(int userId)
        {
            return service.GetAll(userId);
        }
        
        public Note CreateNote(CreateNoteRequest Request, int UserId)
        {
            return service.CreateNote(Request,UserId);
        }

        public Task<int> UpdateNote(UpdateNoteRequest Request, int UserId,int NoteId)
        {
            return service.UpdateNote(Request,UserId,NoteId);
        }

        public Task<int> DeleteNote(int NoteId,int UserId)
        {
            return service.DeleteNote(NoteId,UserId);
        }

        public Task<string> isArchived(int NoteId, int UserId)
        {
            return service.isArchived(NoteId, UserId);
        }

        public Task<string> isDeleted(int NoteId, int UserId)
        {
            return service.isDeleted(NoteId, UserId);
        }



    }
}

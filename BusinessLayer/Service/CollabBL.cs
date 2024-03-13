using BusinessLayer.Interface;
using ModelLayer.DTO;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class CollabBL:ICollabBL
    {
        private readonly ICollabRL _context;
        public CollabBL(ICollabRL context) 
        { 
            this._context = context;
        }

        public Task<List<Collabinfo>> GetAll()
        {
            return this._context.GetAll();
        }

        public int AddCollaborator(int NoteId, CollabRequest Request, int UserId)
        {
            return this._context.AddCollaborator(NoteId,Request,UserId);
        }
    }
}

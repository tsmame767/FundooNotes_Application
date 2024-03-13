using ModelLayer.DTO;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ICollabBL
    {
        public Task<List<Collabinfo>> GetAll();

        public int AddCollaborator(int NoteId,CollabRequest Request,int UserId);
        public int RemoveCollaborator(int NoteId, CollabRequest Request);
    }
}

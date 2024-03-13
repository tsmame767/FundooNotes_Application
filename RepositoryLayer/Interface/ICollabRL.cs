using ModelLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface ICollabRL
    {
        public Task<List<Collabinfo>> GetAll();
        public int AddCollaborator(int NoteId, CollabRequest Request,int UserId);
    }
}

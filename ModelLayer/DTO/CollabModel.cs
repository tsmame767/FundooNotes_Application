using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTO
{
    public class Collabinfo
    {
        public int CollabId {  get; set; }
        public string CollaboratorsEmail { get; set; }
        public int NoteId { get; set; }
        public int UserId { get; set; }

    }

    public class CollabRequest
    {
        public string Email { get; set; }
    }

    public class CollabResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ModelLayer.DTO
{
    public class CreateNoteRequest
    {
        [JsonIgnore]
        public int NoteId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Colour { get; set; }
    }

    public class UpdateNoteRequest
    {
        //public int NoteId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Colour { get; set; }
    }

    public class NoteResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }

    public class NoteInfo
    {
        
        public int NoteId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Colour { get; set; }
        public int UserId { get; set; }

        [JsonIgnore]
        public bool IsArchived { get; set; } = false;
        [JsonIgnore]
        public bool IsDeleted { get; set; } = false;


    }


}

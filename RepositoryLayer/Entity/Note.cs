using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class Note
    {
        [Key]
        public int NoteId{get;set;}

        public string Description { get;set;}

        public string Title { get; set; }
        public string Colour { get; set; }

        [ForeignKey("Student_Details")]
        public int UserId { get; set; }

        
        public bool IsArchived { get; set; } = false;
        
        public bool IsDeleted { get; set; } = false;


        

       // [JsonIgnore]
       // public virtual Student Users { get; set; }


    }
}

using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class Collaboration
    {
        [ForeignKey("Student_Details")]
        public int userID { get; set; }

        [ForeignKey("Note")]
        public int NoteId {  get; set; }
        public string collaboratorsEmail {  get; set; }

        [Key]        
        public int collabId { get; set; }
    }
}

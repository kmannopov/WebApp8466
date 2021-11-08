using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSCC_CW1_MVCWebApp_8466.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int Isbn { get; set; }
        public int BookGenreId { get; set; }
        public Genre BookGenre { get; set; }
    }
}

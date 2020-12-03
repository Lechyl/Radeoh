using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.Models
{
    public class Favorite
    {
        public string Slug { get; set; }
        
        public string Title { get; set; }

        public string Country { get; set; }

        public string Lang { get; set; }

        public string Image { get; set; }

        public string StreamUrl { get; set; }

    }
}

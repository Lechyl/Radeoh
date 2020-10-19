using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace RadioApp.Models
{
    public class Favorite
    {
        [PrimaryKey]
        public string Slug { get; set; }


    }
}

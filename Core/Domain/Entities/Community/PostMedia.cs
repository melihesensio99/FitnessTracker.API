using System;
using System.Collections.Generic;
using System.Text;

using Domain.Enums;


namespace Domain.Entities.Community
{
    public class PostMedia
    {
        public int Id { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public string Url { get; set; }

        public MediaType Type { get; set; }
    }

}


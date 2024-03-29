﻿using PhotoForum.Models;
using System.ComponentModel.DataAnnotations;

namespace PhotoForum.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        [StringLength(64, MinimumLength=16)]
        public string Title { get; set; }

        [Required]
        [StringLength(8192, MinimumLength = 32)]
        public string Content { get; set; }
        public string PhotoUrl { get; set; }
        //Nav prop
        public User? Creator { get; set; }
        public IList<Comment> Comments { get; set; } = new List<Comment>();
        public int LikesCount { get; set; }
        public DateTime Date { get; set; }
        public IList<Tag> Tags { get; set; } = new List<Tag>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();

    }
}

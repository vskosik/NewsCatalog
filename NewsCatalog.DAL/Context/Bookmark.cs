namespace NewsCatalog.DAL.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Bookmark
    {
        [Key]
        public int BookmarkId { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public string SourceUrl { get; set; }

        public string ImageUrl { get; set; }

        public string PublishedData { get; set; }

        public string Content { get; set; }

        [Required]
        public bool IsBanned { get; set; }
    }
}

using System;
using Microsoft.AspNetCore.Http;

namespace KaniWebApp.API.Dtos
{
    public class ImageForCreationDto
    {
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }

        public ImageForCreationDto()
        {
            DateAdded = DateTime.Now;
        }
    }
}
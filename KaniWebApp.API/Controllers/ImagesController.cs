using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using KaniWebApp.API.Data;
using KaniWebApp.API.Dtos;
using KaniWebApp.API.Helpers;
using KaniWebApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace KaniWebApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/images")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IWebAppRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public ImagesController(IWebAppRepository repo, IMapper mapper,
         IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _repo = repo;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name = "GetImage")]
        public async Task<IActionResult> GetImage(int id)
        {
            var imageFromRepo = await _repo.GetImage(id);

            var image = _mapper.Map<ImageForReturnDto>(imageFromRepo);

            return Ok(image);
        }

        [HttpPost]
        public async Task<IActionResult> AddImageForUser(int userId, [FromForm]ImageForCreationDto imageForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(userId);

            var file = imageForCreationDto.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation()
                            .Width(500).Height(500).Crop("limit")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            imageForCreationDto.Url = uploadResult.Uri.ToString();
            imageForCreationDto.PublicId = uploadResult.PublicId;

            var image = _mapper.Map<Image>(imageForCreationDto);

            if (!userFromRepo.Images.Any(i => i.IsMain))
                image.IsMain = true;

            userFromRepo.Images.Add(image);

            if (await _repo.SaveAll())
            {
                var imageToReturn = _mapper.Map<ImageForReturnDto>(image);
                return CreatedAtRoute("GetImage", new { id = image.Id }, imageToReturn);
            }

            return BadRequest("Could not add the photo");
        }
    }
}
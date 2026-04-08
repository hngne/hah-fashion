using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using QA_TMDT.Utils;

namespace QA_TMDT.Helper
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(IOptions<CloudinarySetting> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );
            _cloudinary = new Cloudinary(acc);
        }

        public async Task<string?> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;
            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "QA_TMDT_Images"
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.ToString();
        }
    }
}

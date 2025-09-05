using CRM_API.Models;

namespace CRM_API.Services.ImageProcessing
{
    public interface IImageProcessor
    {
        Byte[] Process(IFormFile rawPicture);
    }
}

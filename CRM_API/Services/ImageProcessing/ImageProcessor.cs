namespace CRM_API.Services.ImageProcessing;

public class ImageProcessor : IImageProcessor
{
    private const int DesiredWidth = 300; //300px
    
    public Byte[] Process(IFormFile rawPicture)
    {
        ProfilePicture picture = new ProfilePicture(rawPicture.FileName, rawPicture.ContentType, rawPicture.OpenReadStream());

        if (picture.Content == null || picture.Content.Length == 0)
        {
            return new byte[0];
        }

        try
        {
            //get picture data from iformfile
            using var imageResult = Image.Load(picture.Content);

            var width = imageResult.Width;
            var height = imageResult.Height;

            // set image to desired width and height
            if (width > DesiredWidth)
            {
                height = Convert.ToInt32( (DesiredWidth / width) * height);
                width = DesiredWidth;
            }

            //process the image
            imageResult.Metadata.ExifProfile = null; //remove image metadata 
            imageResult.Mutate(image => image.Resize(new Size(width, height)));


            //return the byte array to store in the database
            using var memoryStream = new MemoryStream();
            imageResult.SaveAsJpeg(memoryStream, new JpegEncoder { Quality = 75 });
            return memoryStream.ToArray();
        }
        catch (Exception ex)
        {
            throw new ImageProcessingException($"Image processing failed. See inner exception: {ex.Message}");
        }
    }
}



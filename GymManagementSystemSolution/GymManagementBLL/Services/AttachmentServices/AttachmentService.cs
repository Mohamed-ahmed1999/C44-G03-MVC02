using Microsoft.AspNetCore.Http;

 
namespace GymManagementBLL.Services.AttachmentServices
{
    public class AttachmentService : IAttachmentService
    {
        private readonly string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        private readonly string[] allowedExtensions = {".jpg" , ".png" , ".jpeg"};
        private readonly long maxFileSize = 5 * 1024 * 1024;

  
        public string? Upload(string folderName, IFormFile file)
        {
            try
            {

                if (folderName == null || file == null || file.Length == 0) return null;

                if (file.Length > maxFileSize) return null;

                var extinsion = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(extinsion)) return null;

                var folderPath = Path.Combine(rootPath, "images", folderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileName = Guid.NewGuid().ToString() + extinsion;
                var filePath = Path.Combine(folderPath, fileName);

                using var filestream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(filestream);

                return $"/images/{folderName}/{fileName}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to Upload File To Folder = {folderName} : {ex}");
                return null;
            }
        }

        public bool Delete(string fileName, string folderName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName)) return false;

                var filePath = Path.Combine(rootPath, "images", folderName, fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to Delete File with Name = {fileName} From Folder = {folderName} : {ex}");
                return false;
            }

        }
    }
}

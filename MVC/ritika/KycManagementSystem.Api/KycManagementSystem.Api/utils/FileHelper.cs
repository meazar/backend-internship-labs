namespace KycManagementSystem.Api.utils
{
    public class FileHelper
    {
        public static string SaveFile(byte[] fileByte, string folderPath, string fileName)
        {
            if(!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string fullPath = Path.Combine(folderPath, fileName);
            File.WriteAllBytes(fullPath, fileByte);
            return fullPath;
        }
        public static bool DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }
        
    }
}

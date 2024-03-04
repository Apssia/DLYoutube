using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLYoutube.DataAccess
{
    internal class Storage
    {
        public async Task<bool> SaveFile(Stream stream, string title)
        {
            if (stream == null || title == null)
            {
                return false;
            }
            string titleFormat = FormateTitle(title);
            Console.WriteLine(titleFormat);
            try
            {
                // Get the path to Downloads folder
                var path = Path.Combine(System.Environment.CurrentDirectory, titleFormat + ".mp4");

                // Save the file
                var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                Console.WriteLine("Video " + titleFormat + " is being saved");
                await stream.CopyToAsync(fileStream);
                fileStream.Dispose();
                Console.WriteLine("Video is saved on " + path);
                return true;
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Video " + titleFormat + " couldn't be saved");
                Console.WriteLine(ex.Message);
                return false;
            }
            
        }

        private string FormateTitle(string title)
        {
            // Get all the chars that can be in a filename
            char[] invalidChars = Path.GetInvalidFileNameChars();

            foreach (char invalidChar in invalidChars)
            {
                // Replace all forbidden char in a filename
                title = title.Replace(invalidChar, '_');
            }

            return title;
        }
    }
}

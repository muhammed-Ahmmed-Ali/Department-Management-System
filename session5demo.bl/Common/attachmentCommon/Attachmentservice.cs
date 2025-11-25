using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace session5demo.bl.Common.attachmentCommon
{
    public class Attachmentservice : Iattachmentservice
    {
       private readonly List<string> extension = new List<string>() { ".png", ".JPG", ".jpeg" };
        private const int maxsize = 2_852_158;
        public string addattachment(IFormFile file, string folder)
        {
            var ex = Path.GetExtension(file.FileName);
            if (!extension.Contains(ex))
            {
                throw new Exception("not valid");
            }
            if (file.Length > maxsize)
            {
                throw new Exception("not valid size");
            }
            var folderpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", folder);
            if (!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
            }
            var filename = $"{Guid.NewGuid}_{file.FileName}";
            var filepath = Path.Combine(folderpath, filename);
            using var x = new FileStream(filepath, FileMode.Create);
            file.CopyTo(x);
            return filename;
        }

        public bool delete(string filepath)
        {
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
                return true;
            }
            return false;
        }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace layoutlovers.Files
{
    public static class FormFileExtensions
    {
        public static FileType GetFileType(this IFormFile formFile)
        {
            var contentType = Path.GetExtension(formFile.FileName);

            switch (contentType)
            {
                case ".sketch":
                    {
                        return FileType.Sketch;
                    }
                case ".fig":
                    {
                        return FileType.Fig;
                    }
                case ".psd":
                    {
                        return FileType.Psd;
                    }
                case ".xd":
                    {
                        return FileType.Xd;
                    }
                case ".png":
                    {
                        return FileType.Png;
                    }
                case ".jpg":
                    {
                        return FileType.Jpg;
                    }
                case ".jpeg":
                    {
                        return FileType.Jpeg;
                    }
                case ".bmp":
                    {
                        return FileType.Bmp;
                    }
                default:
                    throw new AccessViolationException($"file with this extension {contentType} is not supported by the system!");
            }
        }

        public static bool HasImage(this IFormFile formFile)
        {

            var fileType = GetFileType(formFile);
            return fileType == FileType.Png
                || fileType == FileType.Jpg
                || fileType == FileType.Jpeg
                || fileType == FileType.Bmp;
        }
    }
}

using System.IO;

namespace Pims.Api.Models
{
    public class FileStreamResponse
    {
        /// <summary>
        /// get/set - The file contents. Could be encoded.
        /// </summary>
        public Stream FilePayload { get; set; }

        /// <summary>
        /// get/set - The file size.
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// get/set - Name of the file.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// get/set - The extension of the file (pdf, docx, etc).
        /// </summary>
        public string FileNameExtension { get; set; }

        /// <summary>
        /// get/set - Complement of FileNameExtension.
        /// </summary>
        public string FileNameWithoutExtension { get; set; }

        /// <summary>
        /// get/set - The Mime-Type that the file uses.
        /// </summary>
        public string Mimetype { get; set; }

        /// <summary>
        /// get/set - The encoding type that the file uses.
        /// </summary>
        public string EncodingType { get; set; } = "base64";
    }
}

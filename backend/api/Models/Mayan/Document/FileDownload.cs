namespace Pims.Api.Models.Mayan.Document
{
    /// <summary>
    /// Represents a file download result.
    /// </summary>
    public class FileDownload
    {
        /// <summary>
        /// get/set - The file contents. Could be encoded.
        /// </summary>
        public byte[] FilePayload { get; set; }

        /// <summary>
        /// get/set - The file size.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// get/set - Name of the file.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// get/set - THe Mime-Type that the file uses.
        /// </summary>
        public string Mimetype { get; set; }
    }
}

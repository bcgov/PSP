using System;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.Mayan.Document
{
    /// <summary>
    /// Represents the latest file for a document.
    /// </summary>
    public class FileLatestModel
    {
        /// <summary>
        /// get/set - The file id.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// get/set - File comment.
        /// </summary>
        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        /// <summary>
        /// get/set - The encoding the file is using.
        /// </summary>
        [JsonPropertyName("encoding")]
        public string Encoding { get; set; }

        /// <summary>
        /// get/set - uid of the file.
        /// </summary>
        [JsonPropertyName("file")]
        public string FileUuid { get; set; }

        /// <summary>
        /// get/set - Name of the file.
        /// </summary>
        [JsonPropertyName("filename")]
        public string FileName { get; set; }

        /// <summary>
        /// get/set - THe Mime-Type that the file uses.
        /// </summary>
        [JsonPropertyName("mimetype")]
        public string Mimetype { get; set; }

        /// <summary>
        /// get/set - file checksum.
        /// </summary>
        [JsonPropertyName("checksum")]
        public string Checksum { get; set; }

        /// <summary>
        /// get/set - The size of the file.
        /// </summary>
        [JsonPropertyName("size")]
        public long? Size { get; set; }

        /// <summary>
        /// get/set - Timestamp for the file.
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime TimeStamp { get; set; }
    }
}

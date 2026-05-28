#nullable enable

namespace Pims.Api.Models.Ches
{
    /// <summary>
    /// Defines the contract for an email attachment.
    /// </summary>
    public interface IAttachment
    {
        string? Content { get; set; }

        string? ContentType { get; set; }

        string? Filename { get; set; }
    }
}

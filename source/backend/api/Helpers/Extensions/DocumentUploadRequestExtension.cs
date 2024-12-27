using System;
using Pims.Api.Models.Requests.Document.Upload;
using Pims.Core.Api.Exceptions;

namespace Pims.Api.Helpers.Extensions
{
    public static class DocumentUploadRequestExtension
    {
        public static void ThrowInvalidFileSize(this DocumentUploadRequest documentUploadRequest)
        {
            ArgumentNullException.ThrowIfNull(documentUploadRequest);

            if (documentUploadRequest.File is not null && documentUploadRequest.File.Length == 0)
            {
                throw new BadRequestException("The submitted file is empty");
            }

            return;
        }
    }
}

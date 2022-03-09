using System.Threading.Tasks;
using Pims.Api.Models;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Repositories.EDMS;

namespace Pims.Api.Services
{
    /// <summary>
    /// IDocumentRepository interface, defines the functionality for a document repository.
    /// </summary>
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository documentRepository;

        public DocumentService(IDocumentRepository documentRepository)
        {
            this.documentRepository = documentRepository;
        }

        public ExternalResult<QueryResult<DocumentDetail>> GetDocumentList(string ordering = "", int? page = null, int? pageSize = null)
        {
            Task<ExternalResult<QueryResult<DocumentDetail>>> task = documentRepository.GetDocumentsListAsync(ordering, page, pageSize);
            task.Wait();
            return task.Result;
        }

        public ExternalResult<FileDownload> DownloadFile(int documentId, int fileId)
        {
            Task<ExternalResult<FileDownload>> task = documentRepository.DownloadFileAsync(documentId, fileId);
            task.Wait();
            return task.Result;
        }


    }
}

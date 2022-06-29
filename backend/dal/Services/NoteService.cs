using System.Security.Claims;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Pims.Dal.Constants;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Services
{
    public class NoteService : BaseService, INoteService
    {
        private readonly Repositories.INoteRepository _noteRepository;
        private readonly Repositories.IEntityNoteRepository _entityNoteRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Creates a new instance of a NoteService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        /// <param name="noteRepository"></param>
        public NoteService(ClaimsPrincipal user, ILogger<BaseService> logger, IMapper mapper, Repositories.INoteRepository noteRepository, Repositories.IEntityNoteRepository entityNoteRepository) : base(user, logger)
        {
            _mapper = mapper;
            _noteRepository = noteRepository;
            _entityNoteRepository = entityNoteRepository;
        }

        public GenericNote Add(NoteType type, GenericNote noteModel)
        {
            // TODO: Implement
            throw new System.NotImplementedException();
        }
    }
}

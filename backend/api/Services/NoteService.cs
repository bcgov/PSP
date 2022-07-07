using System.Security.Claims;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Pims.Api.Constants;
using Pims.Api.Models.Concepts;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class NoteService : BaseService, INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly IEntityNoteRepository _entityNoteRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Creates a new instance of a NoteService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="noteRepository"></param>
        /// <param name="entityNoteRepository"></param>
        public NoteService(ClaimsPrincipal user, ILogger<NoteService> logger, IMapper mapper, INoteRepository noteRepository, IEntityNoteRepository entityNoteRepository) : base(user, logger)
        {
            _mapper = mapper;
            _noteRepository = noteRepository;
            _entityNoteRepository = entityNoteRepository;
        }

        public EntityNoteModel Add(NoteType type, EntityNoteModel model)
        {
            model.ThrowIfNull(nameof(model));
            this.User.ThrowIfNotAuthorized(Permissions.NoteAdd);

            EntityNoteModel result;

            switch (type)
            {
                case NoteType.Activity:
                default:
                    var pimsEntity = _mapper.Map<PimsActivityInstanceNote>(model);

                    var createdEntity = _entityNoteRepository.Add<PimsActivityInstanceNote>(pimsEntity);
                    _entityNoteRepository.CommitTransaction();

                    result = _mapper.Map<EntityNoteModel>(createdEntity);
                    break;
            }

            return result;
        }
    }
}

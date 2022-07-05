using System.Security.Claims;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Constants;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;

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

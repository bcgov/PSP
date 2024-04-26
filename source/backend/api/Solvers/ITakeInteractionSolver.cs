using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface ITakeInteractionSolver
    {
        bool ResultsInOwnedProperty(IEnumerable<PimsTake> takes);
    }
}

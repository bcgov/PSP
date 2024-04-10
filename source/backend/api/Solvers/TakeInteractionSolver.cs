using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public class TakeInteractionSolver : ITakeInteractionSolver
    {
        private readonly HashSet<string> LandActWithOwnership;

        public TakeInteractionSolver()
        {
            LandActWithOwnership = new HashSet<string>() { "Crown Grant", "Transfer Admin" };
        }

        /// <summary>
        /// Evaluates if the takes passed as an argument will result in an property owned (Core Inventory).
        /// Note: This method does not validate the take status, nor if the takes belong to the same property.
        /// </summary>
        /// <param name="takes"></param>
        public bool ResultsInOwnedProperty(IEnumerable<PimsTake> takes)
        {
            foreach (var take in takes)
            {
                if (take.IsNewHighwayDedication && take.IsAcquiredForInventory)
                {
                    return true;
                }

                if (take.IsThereSurplus)
                {
                    return true;
                }

                if (take.IsNewLandAct && LandActWithOwnership.Contains(take.LandActTypeCode))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

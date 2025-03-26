using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    public class BctfaOwnershipService : IBctfaOwnershipService
    {
        private static readonly char[] Separator = new[] { '\r', '\n' };
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly IPmbcBctfaPidRepository _bctfaOwnershipRepository;

        public BctfaOwnershipService(
            ClaimsPrincipal user,
            ILogger<AcquisitionFileService> logger,
            IPmbcBctfaPidRepository bctfaOwnershipRepository)
        {
            _user = user;
            _logger = logger;
            _bctfaOwnershipRepository = bctfaOwnershipRepository;
        }

        public int[] ParseCsvFileToIntArray(Stream fileStream)
        {
            _logger.LogInformation("Parsing CSV file stream for bctfa ownership with length {Length}", fileStream?.Length);
            if (fileStream == null)
            {
                throw new ArgumentNullException(nameof(fileStream), "File stream cannot be null.");
            }

            using var reader = new StreamReader(fileStream);
            var content = reader.ReadToEnd();
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new InvalidDataException("File is empty or contains only whitespace.");
            }

            var lines = content.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0)
            {
                throw new InvalidDataException("File does not contain any data.");
            }

            List<int> result = new List<int>();
            lines.ForEach(l =>
            {
                var values = l.Split(',');

                result.AddRange(values.Where(value => int.TryParse(value, out var pid)).Select(int.Parse));
            });
            return result.Distinct().ToArray();
        }

        public void UpdateBctfaOwnership(IEnumerable<int> allPids)
        {
            _user.ThrowIfNotAuthorized(Permissions.BctfaOwnershipView);

            _logger.LogInformation("Executing UpdateBctfaOwnership method.");
            _logger.LogDebug("All pids: {Pids}", allPids.Serialize());

            var currentBctfaPids = _bctfaOwnershipRepository.GetAll();
            var allCurrentPids = currentBctfaPids.Select(ownership => ownership.Pid).ToList();

            var pidsToAdd = allPids.Except(allCurrentPids).ToList();
            var toAddNewOwnership = pidsToAdd.Select(pid => new PmbcBctfaPid() { Pid = pid, IsBctfaOwned = true });
            _logger.LogDebug("Adding pids: {Pids}", toAddNewOwnership.Serialize());
            _bctfaOwnershipRepository.AddRange(toAddNewOwnership);

            currentBctfaPids.ForEach(curr => curr.IsBctfaOwned = allPids.Contains(curr.Pid));
            _logger.LogDebug("Updating ownership of: {Pids}", currentBctfaPids.Serialize());
            _bctfaOwnershipRepository.UpdateRange(currentBctfaPids);

            _bctfaOwnershipRepository.CommitTransaction();
        }
    }
}

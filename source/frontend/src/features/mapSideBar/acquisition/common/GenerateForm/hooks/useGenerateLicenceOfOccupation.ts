import { createFileDownload } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { useApiLeases } from '@/hooks/pims-api/useApiLeases';
import { useInsurancesRepository } from '@/hooks/repositories/useInsuranceRepository';
import { useLeasePeriodRepository } from '@/hooks/repositories/useLeasePeriodRepository';
import { useLeaseTenantRepository } from '@/hooks/repositories/useLeaseTenantRepository';
import { usePropertyLeaseRepository } from '@/hooks/repositories/usePropertyLeaseRepository';
import { useSecurityDepositRepository } from '@/hooks/repositories/useSecurityDepositRepository';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';
import { ApiGen_CodeTypes_LeaseLicenceTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseLicenceTypes';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { Api_GenerateLease } from '@/models/generate/lease/GenerateLease';
import { exists, useAxiosErrorHandler } from '@/utils';

export const useGenerateLicenceOfOccupation = () => {
  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();
  const {
    getInsurances: { execute: getInsurances },
  } = useInsurancesRepository();

  const {
    getLeaseTenants: { execute: getLeaseTenants },
  } = useLeaseTenantRepository();

  const {
    getSecurityDeposits: { execute: getLeaseDeposits },
  } = useSecurityDepositRepository();

  const {
    getLeasePeriods: { execute: getLeasePeriods },
  } = useLeasePeriodRepository();

  const {
    getPropertyLeases: { execute: getPropertyLeases },
  } = usePropertyLeaseRepository();

  const { getApiLease } = useApiLeases();
  const { execute: getLease } = useApiRequestWrapper({
    requestFunction: getApiLease,
    requestName: 'getApiLease',
    onError: useAxiosErrorHandler('Failed to load lease, reload this page to try again.'),
  });

  const VALID_LICENCE_TYPES = [
    ApiGen_CodeTypes_LeaseLicenceTypes.LIOCCACCS.toString(),
    ApiGen_CodeTypes_LeaseLicenceTypes.LIOCCTTLD.toString(),
    ApiGen_CodeTypes_LeaseLicenceTypes.LIOCCUSE.toString(),
    ApiGen_CodeTypes_LeaseLicenceTypes.LIOCCUTIL.toString(),
    ApiGen_CodeTypes_LeaseLicenceTypes.LIPPUBHWY.toString(),
  ];

  const generateLicenceOfOccupation = async (lease: ApiGen_Concepts_Lease) => {
    if (lease?.id) {
      const updatedLeasePromise = getLease(lease.id);
      const insurancesPromise = getInsurances(lease.id);
      const tenantsPromise = getLeaseTenants(lease.id);
      const securityDepositsPromise = getLeaseDeposits(lease.id);
      const periodsPromise = getLeasePeriods(lease.id);
      const propertyLeasesPromise = getPropertyLeases(lease.id);
      const [updatedLease, insurances, tenants, securityDeposits, periods, propertyLeases] =
        await Promise.all([
          updatedLeasePromise,
          insurancesPromise,
          tenantsPromise,
          securityDepositsPromise,
          periodsPromise,
          propertyLeasesPromise,
        ]);

      if (!exists(updatedLease)) {
        throw new Error('Failed to load lease, reload this page to try again.');
      }

      if (!updatedLease.type?.id || !VALID_LICENCE_TYPES.includes(updatedLease.type.id)) {
        throw new Error('Invalid licence type.');
      }

      const leaseData = new Api_GenerateLease(
        updatedLease,
        insurances ?? [],
        tenants ?? [],
        securityDeposits ?? [],
        propertyLeases ?? [],
        periods ?? [],
      );

      let formTemplateType: ApiGen_CodeTypes_FormTypes;
      switch (updatedLease.type.id) {
        case ApiGen_CodeTypes_LeaseLicenceTypes.LIOCCACCS:
        case ApiGen_CodeTypes_LeaseLicenceTypes.LIOCCTTLD:
        case ApiGen_CodeTypes_LeaseLicenceTypes.LIOCCUSE:
        case ApiGen_CodeTypes_LeaseLicenceTypes.LIOCCUTIL:
          formTemplateType = ApiGen_CodeTypes_FormTypes.H1005A;
          break;
        case ApiGen_CodeTypes_LeaseLicenceTypes.LIPPUBHWY:
          formTemplateType = ApiGen_CodeTypes_FormTypes.H1005;
          break;
        default:
          throw new Error('Invalid licence type.');
      }

      const generatedFile = await generate({
        templateType: formTemplateType.toString(),
        templateData: leaseData,
        convertToType: null,
      });
      if (
        generatedFile?.status === ApiGen_CodeTypes_ExternalResponseStatus.Success &&
        generatedFile?.payload
      ) {
        createFileDownload(generatedFile?.payload);
      } else {
        throw Error('Failed to generate file');
      }
      return generatedFile;
    }
  };
  return generateLicenceOfOccupation;
};

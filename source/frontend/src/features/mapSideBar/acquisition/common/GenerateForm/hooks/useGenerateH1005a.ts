import { showFile } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { FormTemplateTypes } from '@/features/mapSideBar/shared/content/models';
import { useApiLeases } from '@/hooks/pims-api/useApiLeases';
import { useInsurancesRepository } from '@/hooks/repositories/useInsuranceRepository';
import { useLeaseTenantRepository } from '@/hooks/repositories/useLeaseTenantRepository';
import { useLeaseTermRepository } from '@/hooks/repositories/useLeaseTermRepository';
import { usePropertyLeaseRepository } from '@/hooks/repositories/usePropertyLeaseRepository';
import { useSecurityDepositRepository } from '@/hooks/repositories/useSecurityDepositRepository';
import { useApiRequestWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ExternalResultStatus } from '@/models/api/ExternalResult';
import { Api_Lease } from '@/models/api/Lease';
import { Api_GenerateLease } from '@/models/generate/lease/GenerateLease';
import { useAxiosErrorHandler } from '@/utils';

export const useGenerateH1005a = (lease?: Api_Lease) => {
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
    getLeaseTerms: { execute: getLeaseTerms },
  } = useLeaseTermRepository();

  const {
    getPropertyLeases: { execute: getPropertyLeases },
  } = usePropertyLeaseRepository();

  const { getApiLease } = useApiLeases();
  const { execute: getLease } = useApiRequestWrapper({
    requestFunction: getApiLease,
    requestName: 'getApiLease',
    onError: useAxiosErrorHandler('Failed to load lease, reload this page to try again.'),
  });

  const generateH1005a = async (lease: Api_Lease) => {
    if (lease?.id) {
      const updatedLeasePromise = getLease(lease.id);
      const insurancesPromise = getInsurances(lease.id);
      const tenantsPromise = getLeaseTenants(lease.id);
      const securityDepositsPromise = getLeaseDeposits(lease.id);
      const termsPromise = getLeaseTerms(lease.id);
      const propertyLeasesPromise = getPropertyLeases(lease.id);
      const [updatedLease, insurances, tenants, securityDeposits, terms, propertyLeases] =
        await Promise.all([
          updatedLeasePromise,
          insurancesPromise,
          tenantsPromise,
          securityDepositsPromise,
          termsPromise,
          propertyLeasesPromise,
        ]);
      if (updatedLease === null || updatedLease === undefined)
        throw new Error('Failed to load lease, reload this page to try again.');
      const leaseData = new Api_GenerateLease(
        updatedLease,
        insurances ?? [],
        tenants ?? [],
        securityDeposits ?? [],
        propertyLeases ?? [],
        terms ?? [],
      );

      const generatedFile = await generate({
        templateType: FormTemplateTypes.H1005A,
        templateData: leaseData,
        convertToType: null,
      });
      if (generatedFile?.status === ExternalResultStatus.Success!! && generatedFile?.payload) {
        showFile(generatedFile?.payload);
      } else {
        throw Error('Failed to generate file');
      }
      return generatedFile;
    }
  };
  return generateH1005a;
};

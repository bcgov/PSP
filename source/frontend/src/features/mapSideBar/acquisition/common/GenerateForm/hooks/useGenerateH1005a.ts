import { showFile } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { FormTemplateTypes } from '@/features/mapSideBar/shared/content/models';
import { useApiLeases } from '@/hooks/pims-api/useApiLeases';
import { useInsurancesRepository } from '@/hooks/repositories/useInsuranceRepository';
import { useLeaseTenantRepository } from '@/hooks/repositories/useLeaseTenantRepository';
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

  const { getApiLease } = useApiLeases();
  const { execute: getLease } = useApiRequestWrapper({
    requestFunction: getApiLease,
    requestName: 'getApiLease',
    onError: useAxiosErrorHandler('Failed to load lease, reload this page to try again.'),
  });

  const generateH1005a = async (lease: Api_Lease) => {
    if (lease?.id) {
      const updatedLease = (await getLease(lease.id)) ?? null;
      const insurances = (await getInsurances(lease.id)) ?? [];
      const tenants = (await getLeaseTenants(lease.id)) ?? [];
      const getSecurityDeposits = (await getLeaseDeposits(lease.id)) ?? [];
      if (updatedLease === null)
        throw new Error('Failed to load lease, reload this page to try again.');
      const leaseData = new Api_GenerateLease(
        updatedLease,
        insurances,
        tenants,
        getSecurityDeposits,
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

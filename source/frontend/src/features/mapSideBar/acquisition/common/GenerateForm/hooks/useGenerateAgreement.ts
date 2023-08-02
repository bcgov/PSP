import { InterestHolderType } from '@/constants/interestHolderTypes';
import { showFile } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { FormTemplateTypes } from '@/features/mapSideBar/shared/content/models';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { AgreementTypes, Api_Agreement } from '@/models/api/Agreement';
import { ExternalResultStatus } from '@/models/api/ExternalResult';
import { Api_GenerateAcquisitionFile } from '@/models/generate/acquisition/GenerateAcquisitionFile';
import { Api_GenerateAgreement } from '@/models/generate/GenerateAgreement';

export const useGenerateAgreement = () => {
  const { getPersonConcept, getOrganizationConcept } = useApiContacts();
  const { getAcquisitionFile, getAcquisitionProperties } = useAcquisitionProvider();
  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();
  const generateAgreement = async (agreement: Api_Agreement) => {
    if (agreement?.agreementType?.id === undefined) {
      throw Error('user must choose agreement type in order to generate a document');
    }
    const file = await getAcquisitionFile.execute(agreement.acquisitionFileId);
    const properties = await getAcquisitionProperties.execute(agreement.acquisitionFileId);
    if (!file) {
      throw Error('Acquisition file not found');
    }
    file.fileProperties = properties;

    const coordinator = file.acquisitionTeam?.find(
      team => team.personProfileTypeCode === 'PROPCOORD',
    );
    const negotiatingAgent = file.acquisitionTeam?.find(
      team => team.personProfileTypeCode === 'NEGOTAGENT',
    );
    const provincialSolicitor = file.acquisitionTeam?.find(
      team => team.personProfileTypeCode === 'MOTILAWYER',
    );
    const ownerSolicitor = file.acquisitionFileInterestHolders?.find(
      x => x.interestHolderType?.id === InterestHolderType.OWNER_SOLICITOR,
    );

    const coordinatorPromise = coordinator?.personId
      ? getPersonConcept(coordinator?.personId)
      : Promise.resolve(null);
    const negotiatingAgentPromise = negotiatingAgent?.personId
      ? getPersonConcept(negotiatingAgent?.personId)
      : Promise.resolve(null);
    const provincialSolicitorPromise = provincialSolicitor?.personId
      ? getPersonConcept(provincialSolicitor?.personId)
      : Promise.resolve(null);

    // Owner solicitor can be either a Person or an Organization (with optional primary contact)
    const ownerSolicitorPersonPromise = ownerSolicitor?.personId
      ? getPersonConcept(ownerSolicitor?.personId)
      : Promise.resolve(null);
    const ownerSolicitorOrganizationPromise = ownerSolicitor?.organizationId
      ? getOrganizationConcept(ownerSolicitor?.organizationId)
      : Promise.resolve(null);
    const ownerSolicitorPrimaryContactPromise =
      ownerSolicitor?.organizationId && ownerSolicitor?.primaryContactId
        ? getPersonConcept(ownerSolicitor?.primaryContactId)
        : Promise.resolve(null);

    const [
      coordinatorConcept,
      negotiatingAgentConcept,
      provincialSolicitorConcept,
      ownerSolicitorPersonConcept,
      ownerSolicitorOrganizationConcept,
      ownerSolicitorPrimaryContactConcept,
    ] = await Promise.all([
      coordinatorPromise,
      negotiatingAgentPromise,
      provincialSolicitorPromise,
      ownerSolicitorPersonPromise,
      ownerSolicitorOrganizationPromise,
      ownerSolicitorPrimaryContactPromise,
    ]);

    if (ownerSolicitor) {
      ownerSolicitor.person = ownerSolicitorPersonConcept?.data ?? null;
      ownerSolicitor.organization = ownerSolicitorOrganizationConcept?.data ?? null;
      ownerSolicitor.primaryContact = ownerSolicitorPrimaryContactConcept?.data ?? null;
    }

    const fileData = new Api_GenerateAcquisitionFile({
      file,
      coordinatorContact: coordinatorConcept?.data ?? null,
      negotiatingAgent: negotiatingAgentConcept?.data ?? null,
      provincialSolicitor: provincialSolicitorConcept?.data ?? null,
      ownerSolicitor: ownerSolicitor ?? null,
      interestHolders: [],
    });
    const agreementData = new Api_GenerateAgreement(agreement, fileData);
    const generatedFile = await generate({
      templateType: getTemplateTypeFromAgreementType(agreement.agreementType.id),
      templateData: agreementData,
      convertToType: null,
    });
    generatedFile?.status === ExternalResultStatus.Success!! &&
      generatedFile?.payload &&
      showFile(generatedFile?.payload);
  };
  return generateAgreement;
};

/**
 * Get the form type based on the corresponding agreement type.
 * Note that while currently those string values match that is not a safe general assumption that they will always match
 * @param agreementType
 * @returns
 */
const getTemplateTypeFromAgreementType = (agreementType: string) => {
  switch (agreementType) {
    case AgreementTypes.H179A:
      return FormTemplateTypes.H179A;
    case AgreementTypes.H179P:
      return FormTemplateTypes.H179P;
    case AgreementTypes.H179T:
      return FormTemplateTypes.H179T;
    case AgreementTypes.H0074:
      return FormTemplateTypes.H0074;
    default:
      throw Error(`Unable to find form type for agreement type: ${agreementType}`);
  }
};

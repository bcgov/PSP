import { InterestHolderType } from '@/constants/interestHolderTypes';
import { showFile } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { FormTemplateTypes } from '@/features/mapSideBar/shared/content/models';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { ApiGen_CodeTypes_AgreementTypes } from '@/models/api/generated/ApiGen_CodeTypes_AgreementTypes';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { ApiGen_Concepts_Agreement } from '@/models/api/generated/ApiGen_Concepts_Agreement';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { Api_GenerateAcquisitionFile } from '@/models/generate/acquisition/GenerateAcquisitionFile';
import { Api_GenerateAgreement } from '@/models/generate/GenerateAgreement';
import { exists } from '@/utils/utils';

export const useGenerateAgreement = () => {
  const { getPersonConcept, getOrganizationConcept } = useApiContacts();
  const { getAcquisitionFile, getAcquisitionProperties } = useAcquisitionProvider();
  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();
  const generateAgreement = async (agreement: ApiGen_Concepts_Agreement) => {
    if (!exists(agreement?.agreementType?.id)) {
      throw Error('user must choose agreement type in order to generate a document');
    }
    const file = await getAcquisitionFile.execute(agreement.acquisitionFileId);
    const properties = await getAcquisitionProperties.execute(agreement.acquisitionFileId);
    if (!file) {
      throw Error('Acquisition file not found');
    }
    file.fileProperties = properties ?? null;

    const coordinator = file.acquisitionTeam?.find(
      team => team.teamProfileTypeCode === 'PROPCOORD',
    );
    const negotiatingAgent = file.acquisitionTeam?.find(
      team => team.teamProfileTypeCode === 'NEGOTAGENT',
    );
    const provincialSolicitor = file.acquisitionTeam?.find(
      team => team.teamProfileTypeCode === 'MOTILAWYER',
    );
    const ownerSolicitor = file.acquisitionFileInterestHolders?.find(
      x => x.interestHolderType?.id === InterestHolderType.OWNER_SOLICITOR,
    );

    const coordinatorPromise = coordinator?.personId
      ? getPersonConcept(coordinator?.personId).then(p => (coordinator.person = p?.data))
      : coordinator?.organizationId
      ? getOrganizationConcept(coordinator?.organizationId).then(o =>
          coordinator ? setOrganization(coordinator, o?.data) : null,
        )
      : Promise.resolve();
    const negotiatingAgentPromise = negotiatingAgent?.personId
      ? getPersonConcept(negotiatingAgent?.personId).then(p => (negotiatingAgent.person = p?.data))
      : negotiatingAgent?.organizationId
      ? getOrganizationConcept(negotiatingAgent?.organizationId).then(o =>
          negotiatingAgent ? setOrganization(negotiatingAgent, o?.data) : null,
        )
      : Promise.resolve();
    const provincialSolicitorPromise = provincialSolicitor?.personId
      ? getPersonConcept(provincialSolicitor?.personId).then(
          p => (provincialSolicitor.person = p?.data),
        )
      : provincialSolicitor?.organizationId
      ? getOrganizationConcept(provincialSolicitor?.organizationId).then(o =>
          provincialSolicitor ? setOrganization(provincialSolicitor, o?.data) : null,
        )
      : Promise.resolve();

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

    await Promise.all([coordinatorPromise, negotiatingAgentPromise, provincialSolicitorPromise]);
    const ownerSolicitorPersonConcept = await ownerSolicitorPersonPromise;
    const ownerSolicitorOrganizationConcept = await ownerSolicitorOrganizationPromise;
    const ownerSolicitorPrimaryContactConcept = await ownerSolicitorPrimaryContactPromise;

    if (ownerSolicitor) {
      ownerSolicitor.person = ownerSolicitorPersonConcept?.data ?? null;
      ownerSolicitor.organization = ownerSolicitorOrganizationConcept?.data ?? null;
      ownerSolicitor.primaryContact = ownerSolicitorPrimaryContactConcept?.data ?? null;
    }

    const fileData = new Api_GenerateAcquisitionFile({
      file,
      coordinatorContact: coordinator ?? null,
      negotiatingAgent: negotiatingAgent ?? null,
      provincialSolicitor: provincialSolicitor ?? null,
      ownerSolicitor: ownerSolicitor ?? null,
      interestHolders: [],
    });
    const agreementData = new Api_GenerateAgreement(agreement, fileData);
    const generatedFile = await generate({
      templateType: getTemplateTypeFromAgreementType(agreement.agreementType!.id),
      templateData: agreementData,
      convertToType: null,
    });
    generatedFile?.status === ApiGen_CodeTypes_ExternalResponseStatus.Success &&
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
const getTemplateTypeFromAgreementType = (agreementType: string | null) => {
  switch (agreementType) {
    case ApiGen_CodeTypes_AgreementTypes.H179A:
      return FormTemplateTypes.H179A;
    case ApiGen_CodeTypes_AgreementTypes.H179P:
      return FormTemplateTypes.H179P;
    case ApiGen_CodeTypes_AgreementTypes.H179T:
      return FormTemplateTypes.H179T;
    case ApiGen_CodeTypes_AgreementTypes.H0074:
      return FormTemplateTypes.H0074;
    default:
      throw Error(`Unable to find form type for agreement type: ${agreementType}`);
  }
};

const setOrganization = (
  team: ApiGen_Concepts_AcquisitionFileTeam,
  organization: ApiGen_Concepts_Organization,
) => {
  if (team) {
    team.organization = organization;
    team.primaryContact =
      organization?.organizationPersons?.find(op => op.personId === team.primaryContactId)
        ?.person ?? team.primaryContact;
  }
};

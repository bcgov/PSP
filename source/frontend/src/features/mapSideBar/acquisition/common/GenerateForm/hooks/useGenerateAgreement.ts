import { InterestHolderType } from '@/constants/interestHolderTypes';
import { createFileDownload } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import { useModalContext } from '@/hooks/useModalContext';
import { ApiGen_CodeTypes_AgreementTypes } from '@/models/api/generated/ApiGen_CodeTypes_AgreementTypes';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { ApiGen_Concepts_Agreement } from '@/models/api/generated/ApiGen_Concepts_Agreement';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { ApiGen_Concepts_DispositionFileTeam } from '@/models/api/generated/ApiGen_Concepts_DispositionFileTeam';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { Api_GenerateAcquisitionFile } from '@/models/generate/acquisition/GenerateAcquisitionFile';
import { Api_GenerateAgreement } from '@/models/generate/GenerateAgreement';
import { exists, firstOrNull } from '@/utils/utils';

import { getCancelModalProps } from './../../../../../../hooks/useModalContext';

export const useGenerateAgreement = (
  getFile:
    | ReturnType<typeof useAcquisitionProvider>['getAcquisitionFile']
    | ReturnType<typeof useDispositionProvider>['getDispositionFile'],
  getProperties:
    | ReturnType<typeof useAcquisitionProvider>['getAcquisitionProperties']
    | ReturnType<typeof useDispositionProvider>['getDispositionProperties'],
  isAcquisition = false,
) => {
  const { getPersonConcept, getOrganizationConcept } = useApiContacts();
  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();
  const { setModalContent, setDisplayModal } = useModalContext();
  const generateAgreement = async (agreement: ApiGen_Concepts_Agreement) => {
    if (!exists(agreement?.agreementType?.id)) {
      throw Error('user must choose agreement type in order to generate a document');
    }
    const file = await getFile.execute(agreement.fileId);
    const properties = await getProperties.execute(agreement.fileId);

    if (!file) {
      throw Error('File not found');
    }
    file.fileProperties = properties ?? null;

    const team = isAcquisition
      ? (file as ApiGen_Concepts_AcquisitionFile).acquisitionTeam
      : (file as ApiGen_Concepts_DispositionFile).dispositionTeam;

    const interestHolders = isAcquisition
      ? (file as ApiGen_Concepts_AcquisitionFile).acquisitionFileInterestHolders
      : [];

    const coordinators = team?.filter(team => team.teamProfileTypeCode === 'PROPCOORD');
    const negotiatingAgents = team?.filter(team => team.teamProfileTypeCode === 'NEGOTAGENT');
    const provincialSolicitors = team?.filter(team => team.teamProfileTypeCode === 'MOTILAWYER');
    const ownerSolicitor = interestHolders?.find(
      x => x.interestHolderType?.id === InterestHolderType.OWNER_SOLICITOR,
    );

    if (
      coordinators?.length > 1 ||
      negotiatingAgents?.length > 1 ||
      provincialSolicitors?.length > 1
    ) {
      setModalContent({
        ...getCancelModalProps(),
        cancelButtonText: null,
        okButtonText: 'Ok',
        title: 'Warning',
        message:
          'This file has more then one property coordinator, negotiating agent, or MOTI lawyer. You may need to correct the generated report.',
      });
      setDisplayModal(true);
    }

    const coordinator = firstOrNull(coordinators);
    const negotiatingAgent = firstOrNull(negotiatingAgents);
    const provincialSolicitor = firstOrNull(provincialSolicitors);

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
      createFileDownload(generatedFile?.payload);
  };
  return generateAgreement;
};

/**
 * Get the form type based on the corresponding agreement type.
 * Note that while currently those string values match that is not a safe general assumption that they will always match
 * @param agreementType
 * @returns
 */
const getTemplateTypeFromAgreementType = (agreementType: string | null): string => {
  switch (agreementType) {
    case ApiGen_CodeTypes_AgreementTypes.H179A:
      return ApiGen_CodeTypes_FormTypes.H179A.toString();
    case ApiGen_CodeTypes_AgreementTypes.H179B:
      return ApiGen_CodeTypes_FormTypes.H179B.toString();
    case ApiGen_CodeTypes_AgreementTypes.H179D:
      return ApiGen_CodeTypes_FormTypes.H179D.toString();
    case ApiGen_CodeTypes_AgreementTypes.H179P:
      return ApiGen_CodeTypes_FormTypes.H179P.toString();
    case ApiGen_CodeTypes_AgreementTypes.H179T:
      return ApiGen_CodeTypes_FormTypes.H179T.toString();
    case ApiGen_CodeTypes_AgreementTypes.H0074:
      return ApiGen_CodeTypes_FormTypes.H0074.toString();
    case ApiGen_CodeTypes_AgreementTypes.H179FSPART:
      return ApiGen_CodeTypes_FormTypes.H179FSPART.toString();
    case ApiGen_CodeTypes_AgreementTypes.H179PTO:
      return ApiGen_CodeTypes_FormTypes.H179PTO.toString();
    case ApiGen_CodeTypes_AgreementTypes.H179FS:
      return ApiGen_CodeTypes_FormTypes.H179FS.toString();
    case ApiGen_CodeTypes_AgreementTypes.H179RC:
      return ApiGen_CodeTypes_FormTypes.H179RC.toString();
    default:
      throw Error(`Unable to find form type for agreement type: ${agreementType}`);
  }
};

const setOrganization = (
  team: ApiGen_Concepts_AcquisitionFileTeam | ApiGen_Concepts_DispositionFileTeam,
  organization: ApiGen_Concepts_Organization,
) => {
  if (team) {
    team.organization = organization;
    team.primaryContact =
      organization?.organizationPersons?.find(op => op.personId === team.primaryContactId)
        ?.person ?? team.primaryContact;
  }
};

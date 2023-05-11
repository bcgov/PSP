import { showFile } from 'features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from 'features/documents/hooks/useDocumentGenerationRepository';
import { FormTemplateTypes } from 'features/properties/map/shared/content/models';
import { useApiContacts } from 'hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from 'hooks/repositories/useAcquisitionProvider';
import { AgreementTypes, Api_Agreement } from 'models/api/Agreement';
import { ExternalResultStatus } from 'models/api/ExternalResult';
import { Api_GenerateAgreement } from 'models/generate/GenerateAgreement';
import { Api_GenerateFile } from 'models/generate/GenerateFile';

export const useGenerateAgreement = () => {
  const { getPersonConcept } = useApiContacts();
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
    const ownerSolicitor = file.acquisitionFileOwnerSolicitors?.length
      ? file.acquisitionFileOwnerSolicitors[0]
      : undefined;

    const coordinatorConcept = coordinator?.personId
      ? getPersonConcept(coordinator?.personId)
      : Promise.resolve(null);
    const negotiatingAgentConcept = negotiatingAgent?.personId
      ? getPersonConcept(negotiatingAgent?.personId)
      : Promise.resolve(null);
    const provincialSolicitorConcept = provincialSolicitor?.personId
      ? getPersonConcept(provincialSolicitor?.personId)
      : Promise.resolve(null);
    const ownerSolicitorConcept = ownerSolicitor?.personId
      ? getPersonConcept(ownerSolicitor?.personId)
      : Promise.resolve(null);

    const persons = await Promise.all([
      coordinatorConcept,
      negotiatingAgentConcept,
      provincialSolicitorConcept,
      ownerSolicitorConcept,
    ]);

    const fileData = new Api_GenerateFile(
      file,
      persons[0]?.data,
      persons[1]?.data,
      persons[2]?.data,
      persons[3]?.data,
    );
    const agreementData = new Api_GenerateAgreement(agreement, fileData);
    const generatedFile = await generate({
      templateType: getTemplateTypeFromAgreementType(agreement.agreementType.id),
      templateData: agreementData,
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

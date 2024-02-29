import chunk from 'lodash/chunk';

import { ConvertToTypes } from '@/constants/convertToTypes';
import { showFile } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { FormTemplateTypes } from '@/features/mapSideBar/shared/content/models';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAdminBoundaryMapLayer } from '@/hooks/repositories/mapLayer/useAdminBoundaryMapLayer';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useH120CategoryRepository } from '@/hooks/repositories/useH120CategoryRepository';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { Api_GenerateAcquisitionFile } from '@/models/generate/acquisition/GenerateAcquisitionFile';
import { Api_GenerateCompensation } from '@/models/generate/acquisition/GenerateCompensation';
import { Api_GenerateH120Property } from '@/models/generate/acquisition/GenerateH120Property';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';
import { getLatLng } from '@/utils/mapPropertyUtils';

export const useGenerateH120 = () => {
  const { getPersonConcept, getOrganizationConcept } = useApiContacts();
  const { getAcquisitionFile, getAcquisitionProperties, getAcquisitionCompReqH120s } =
    useAcquisitionProvider();
  const { getAcquisitionInterestHolders } = useInterestHolderRepository();
  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();
  const getH120Categories = useH120CategoryRepository();

  const adminBoundaryService = useAdminBoundaryMapLayer();
  const { getSystemConstant } = useSystemConstants();
  const client = getSystemConstant(SystemConstants.CLIENT);

  const getElectoralDistrict = async (property: Api_GenerateH120Property) => {
    const latLng = getLatLng(property.location);
    const layerData =
      latLng !== null ? await adminBoundaryService.findElectoralDistrict(latLng) : null;

    return layerData?.properties.ED_NAME ?? '';
  };

  const generateCompensation = async (compensation: ApiGen_Concepts_CompensationRequisition) => {
    if (!compensation?.id) {
      throw Error(
        'user must choose a valid compensation requisition in order to generate a document',
      );
    }
    const filePromise = getAcquisitionFile.execute(compensation.acquisitionFileId);
    const propertiesPromise = getAcquisitionProperties.execute(compensation.acquisitionFileId);
    const compReqFinalH120sPromise = getAcquisitionCompReqH120s.execute(
      compensation.acquisitionFileId,
      true,
    );
    const h120CategoriesPromise = getH120Categories.execute();
    const interestHoldersPromise = getAcquisitionInterestHolders.execute(
      compensation.acquisitionFileId,
    );

    // Team members can be either a person or an organization
    const acquisitionFileTeamPersonPromise = compensation?.acquisitionFileTeam?.personId
      ? getPersonConcept(compensation.acquisitionFileTeam.personId)
      : Promise.resolve(null);
    const acquisitionFileTeamOrganizationPromise = compensation?.acquisitionFileTeam?.organizationId
      ? getOrganizationConcept(compensation.acquisitionFileTeam.organizationId)
      : Promise.resolve(null);

    const [
      file,
      properties,
      h120Categories,
      compReqFinalH120s,
      interestHolders,
      acquisitionFileTeamPerson,
      acquisitionFileTeamOrganization,
    ] = await Promise.all([
      filePromise,
      propertiesPromise,
      h120CategoriesPromise,
      compReqFinalH120sPromise,
      interestHoldersPromise,
      acquisitionFileTeamPersonPromise,
      acquisitionFileTeamOrganizationPromise,
    ]);

    if (!file) {
      throw Error('Acquisition file not found');
    }
    file.fileProperties = properties ?? null;

    // Add ELECTORAL_DISTRICT info to each property (from map layer request)
    const fileData = new Api_GenerateAcquisitionFile({
      file: file,
      interestHolders: interestHolders ?? [],
    });
    const batches = chunk(fileData.properties, 5);

    // Run async functions batch-by-batch, with each batch of functions executed in parallel
    for (const currentBatch of batches) {
      const currentBatchPromises = currentBatch.map(async property => {
        property.electoral_dist = await getElectoralDistrict(property);
        return property;
      });
      await Promise.all(currentBatchPromises);
    }

    // Populate payee information
    if (compensation?.acquisitionFileTeam) {
      if (compensation?.acquisitionFileTeam?.personId) {
        compensation.acquisitionFileTeam.person = acquisitionFileTeamPerson?.data ?? null;
      } else if (compensation?.acquisitionFileTeam?.organizationId) {
        compensation.acquisitionFileTeam.organization =
          acquisitionFileTeamOrganization?.data ?? null;
      }
    } else if (compensation?.interestHolderId) {
      const matchedInterestHolder =
        interestHolders?.find(ih => ih.interestHolderId === compensation?.interestHolderId) ?? null;
      compensation.interestHolder = matchedInterestHolder;
    }

    const compensationData = new Api_GenerateCompensation(
      compensation,
      fileData,
      h120Categories ?? [],
      compReqFinalH120s ?? [],
      client?.value,
    );
    const generatedFile = await generate({
      templateType: FormTemplateTypes.H120,
      templateData: compensationData,
      convertToType: ConvertToTypes.PDF,
    });
    if (
      generatedFile?.status === ApiGen_CodeTypes_ExternalResponseStatus.Success &&
      generatedFile?.payload
    ) {
      showFile(generatedFile?.payload);
    } else {
      throw Error('Failed to generate file');
    }
  };
  return generateCompensation;
};

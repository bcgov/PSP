import chunk from 'lodash/chunk';

import { ConvertToTypes } from '@/constants/convertToTypes';
import { createFileDownload } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAdminBoundaryMapLayer } from '@/hooks/repositories/mapLayer/useAdminBoundaryMapLayer';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useH120CategoryRepository } from '@/hooks/repositories/useH120CategoryRepository';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { useLeaseRepository } from '@/hooks/repositories/useLeaseRepository';
import { useLeaseStakeholderRepository } from '@/hooks/repositories/useLeaseStakeholderRepository';
import { usePropertyLeaseRepository } from '@/hooks/repositories/usePropertyLeaseRepository';
import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';
import { ApiGen_CodeTypes_LessorTypes } from '@/models/api/generated/ApiGen_CodeTypes_LessorTypes';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { Api_GenerateAcquisitionFile } from '@/models/generate/acquisition/GenerateAcquisitionFile';
import { Api_GenerateCompensation } from '@/models/generate/acquisition/GenerateCompensation';
import { Api_GenerateH120Property } from '@/models/generate/acquisition/GenerateH120Property';
import { Api_GenerateCompReqFileLease } from '@/models/generate/CompensationRequisition/GenerateCompReqFileLease';
import { ICompensationRequisitionFile } from '@/models/generate/CompensationRequisition/ICompensationRequisitionFile';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';
import { getLatLng } from '@/utils/mapPropertyUtils';

export const useGenerateH120 = () => {
  const adminBoundaryService = useAdminBoundaryMapLayer();
  const { getSystemConstant } = useSystemConstants();
  const client = getSystemConstant(SystemConstants.CLIENT);

  const { getPersonConcept, getOrganizationConcept } = useApiContacts();
  const { getAcquisitionFile, getAcquisitionProperties } = useAcquisitionProvider();
  const { getAcquisitionInterestHolders } = useInterestHolderRepository();
  const { getCompensationRequisitionProperties, getCompensationRequisitionFinancials } =
    useCompensationRequisitionRepository();
  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();
  const getH120Categories = useH120CategoryRepository();

  const { getLeaseStakeholders } = useLeaseStakeholderRepository();
  const { getLease } = useLeaseRepository();
  const { getPropertyLeases } = usePropertyLeaseRepository();

  const getElectoralDistrict = async (property: Api_GenerateH120Property) => {
    const latLng = getLatLng(property.location);
    const layerData =
      latLng !== null ? await adminBoundaryService.findElectoralDistrict(latLng) : null;

    return layerData?.properties.ED_NAME ?? '';
  };

  const VALID_FILE_TYPES = [
    ApiGen_CodeTypes_FileTypes.Acquisition,
    ApiGen_CodeTypes_FileTypes.Lease,
  ];

  const generateCompensation = async (
    fileType: ApiGen_CodeTypes_FileTypes,
    compensation: ApiGen_Concepts_CompensationRequisition,
  ) => {
    if (compensation === null || !compensation?.id) {
      throw Error(
        'user must choose a valid compensation requisition in order to generate a document',
      );
    }

    if (!VALID_FILE_TYPES.includes(fileType)) {
      throw new Error('Invalid file type.');
    }

    let fileData: ICompensationRequisitionFile;
    switch (fileType) {
      case ApiGen_CodeTypes_FileTypes.Acquisition:
        {
          const filePromise = getAcquisitionFile.execute(compensation.acquisitionFileId);
          const filePropertiesPromise = getAcquisitionProperties.execute(
            compensation.acquisitionFileId,
          );
          const interestHoldersPromise = getAcquisitionInterestHolders.execute(
            compensation.acquisitionFileId,
          );

          // Team members can be either a person or an organization
          const acquisitionFileTeamPersonPromise = compensation?.acquisitionFileTeam?.personId
            ? getPersonConcept(compensation.acquisitionFileTeam.personId)
            : Promise.resolve(null);
          const acquisitionFileTeamOrganizationPromise = compensation?.acquisitionFileTeam
            ?.organizationId
            ? getOrganizationConcept(compensation.acquisitionFileTeam.organizationId)
            : Promise.resolve(null);

          const [
            file,
            fileProperties,
            interestHolders,
            acquisitionFileTeamPerson,
            acquisitionFileTeamOrganization,
          ] = await Promise.all([
            filePromise,
            filePropertiesPromise,
            interestHoldersPromise,
            acquisitionFileTeamPersonPromise,
            acquisitionFileTeamOrganizationPromise,
          ]);

          if (!file) {
            throw Error('Acquisition file not found');
          }

          file.fileProperties = fileProperties;

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
              interestHolders?.find(ih => ih.interestHolderId === compensation?.interestHolderId) ??
              null;
            compensation.interestHolder = matchedInterestHolder;
          }

          fileData = new Api_GenerateAcquisitionFile({
            file: file,
            interestHolders: interestHolders ?? [],
          });
        }
        break;
      case ApiGen_CodeTypes_FileTypes.Lease:
        {
          const filePromise = getLease.execute(compensation.leaseId);
          const filePropertiesPromise = getPropertyLeases.execute(compensation.leaseId);
          const leaseStakeholdersPromise = getLeaseStakeholders.execute(compensation.leaseId);

          const [file, fileProperties, leaseStakeHolders] = await Promise.all([
            filePromise,
            filePropertiesPromise,
            leaseStakeholdersPromise,
          ]);

          file.fileProperties = fileProperties;

          if (compensation.compReqLeaseStakeholder?.length > 0) {
            const leaseStakeHolder = compensation.compReqLeaseStakeholder[0].leaseStakeholder;
            if (leaseStakeHolder.lessorType.id === ApiGen_CodeTypes_LessorTypes.PER) {
              const { data } = await getPersonConcept(leaseStakeHolder.personId);
              compensation.compReqLeaseStakeholder[0].leaseStakeholder.person = data;
            } else if (leaseStakeHolder.lessorType.id === ApiGen_CodeTypes_LessorTypes.ORG) {
              const { data } = await getOrganizationConcept(leaseStakeHolder.organizationId);
              compensation.compReqLeaseStakeholder[0].leaseStakeholder.organization = data;
            }
          }

          fileData = new Api_GenerateCompReqFileLease(file, fileProperties, leaseStakeHolders);
        }
        break;
    }

    const compReqPropertiesPromise = getCompensationRequisitionProperties.execute(
      fileType,
      compensation.id,
    );
    const compReqFinalH120sPromise = getCompensationRequisitionFinancials.execute(compensation.id);
    const h120CategoriesPromise = getH120Categories.execute();

    const [compReqProperties, h120Categories, compReqFinancialActivities] = await Promise.all([
      compReqPropertiesPromise,
      h120CategoriesPromise,
      compReqFinalH120sPromise,
    ]);

    const compensationData = new Api_GenerateCompensation(
      compensation,
      compReqProperties ?? [],
      fileData,
      h120Categories ?? [],
      compReqFinancialActivities ?? [],
      client?.value,
    );

    // Run async functions batch-by-batch, with each batch of functions executed in parallel
    const batches = chunk(compensationData.properties, 5);
    for (const currentBatch of batches) {
      const currentBatchPromises = currentBatch.map(async property => {
        property.electoral_dist = await getElectoralDistrict(property);
        return property;
      });
      await Promise.all(currentBatchPromises);
    }

    const generatedFile = await generate({
      templateType: ApiGen_CodeTypes_FormTypes.H120.toString(),
      templateData: compensationData,
      convertToType: ConvertToTypes.PDF,
    });

    if (
      generatedFile?.status === ApiGen_CodeTypes_ExternalResponseStatus.Success &&
      generatedFile?.payload
    ) {
      createFileDownload(generatedFile?.payload);
    } else {
      throw Error('Failed to generate file');
    }
  };

  return generateCompensation;
};

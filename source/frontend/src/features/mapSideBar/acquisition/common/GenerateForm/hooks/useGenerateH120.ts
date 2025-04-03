import chunk from 'lodash/chunk';

import { ConvertToTypes } from '@/constants/convertToTypes';
import { createFileDownload } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
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

  const { getAcquisitionFile, getAcquisitionProperties } = useAcquisitionProvider();
  const { getAcquisitionInterestHolders } = useInterestHolderRepository();
  const {
    getCompensationRequisitionProperties,
    getCompensationRequisitionFinancials,
    getCompensationRequisitionAcqPayees,
    getCompensationRequisitionLeasePayees,
  } = useCompensationRequisitionRepository();
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

          const [file, fileProperties, interestHolders] = await Promise.all([
            filePromise,
            filePropertiesPromise,
            interestHoldersPromise,
          ]);

          if (!file) {
            throw Error('Acquisition file not found');
          }

          file.fileProperties = fileProperties;

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

          fileData = new Api_GenerateCompReqFileLease(file, fileProperties, leaseStakeHolders);
        }
        break;
    }

    const compReqPropertiesPromise = getCompensationRequisitionProperties.execute(
      fileType,
      compensation.id,
    );
    const compReqFinalH120sPromise = getCompensationRequisitionFinancials.execute(compensation.id);
    const compReqPayeesAcqPromise = getCompensationRequisitionAcqPayees.execute(compensation.id);
    const compReqPayeesLeasePromise = getCompensationRequisitionLeasePayees.execute(
      compensation.id,
    );
    const h120CategoriesPromise = getH120Categories.execute();

    const [
      compReqProperties,
      h120Categories,
      compReqFinancialActivities,
      compReqAcqPayees,
      compReqLeasePayees,
    ] = await Promise.all([
      compReqPropertiesPromise,
      h120CategoriesPromise,
      compReqFinalH120sPromise,
      compReqPayeesAcqPromise,
      compReqPayeesLeasePromise,
    ]);

    const compensationData = new Api_GenerateCompensation(
      compensation,
      compReqProperties ?? [],
      compReqAcqPayees ?? [],
      compReqLeasePayees ?? [],
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

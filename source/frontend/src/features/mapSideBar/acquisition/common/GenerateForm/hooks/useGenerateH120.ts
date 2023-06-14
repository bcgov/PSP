import chunk from 'lodash/chunk';

import { ConvertToTypes } from '@/constants/convertToTypes';
import { showFile } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { FormTemplateTypes } from '@/features/mapSideBar/shared/content/models';
import { useLayerQuery } from '@/hooks/layer-api/useLayerQuery';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useH120CategoryRepository } from '@/hooks/repositories/useH120CategoryRepository';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { useCompensationRequisitionRepository } from '@/hooks/repositories/useRequisitionCompensationRepository';
import { Api_CompensationRequisition } from '@/models/api/CompensationRequisition';
import { ExternalResultStatus } from '@/models/api/ExternalResult';
import { Api_GenerateAcquisitionFile } from '@/models/generate/acquisition/GenerateAcquisitionFile';
import { Api_GenerateCompensation } from '@/models/generate/acquisition/GenerateCompensation';
import { Api_GenerateH120Property } from '@/models/generate/acquisition/GenerateH120Property';
import { SystemConstants, useSystemConstants } from '@/store/slices/systemConstants';
import { useTenant } from '@/tenants';
import { getLatLng } from '@/utils/mapPropertyUtils';

export const useGenerateH120 = () => {
  const { getAcquisitionFile, getAcquisitionProperties, getAcquisitionCompReqH120s } =
    useAcquisitionProvider();
  const { getAcquisitionInterestHolders } = useInterestHolderRepository();
  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();
  const getH120Categories = useH120CategoryRepository();
  const { getCompensationRequisitionPayee } = useCompensationRequisitionRepository();

  const { electoralLayerUrl } = useTenant();
  const electoralService = useLayerQuery(electoralLayerUrl);
  const { getSystemConstant } = useSystemConstants();
  const client = getSystemConstant(SystemConstants.CLIENT);

  const getElectoralDistrict = async (property: Api_GenerateH120Property) => {
    const latLng = getLatLng(property.location);
    const layerData =
      latLng !== null ? await electoralService.findMetadataByLocation(latLng) : null;

    return (layerData?.ED_NAME as string) ?? '';
  };

  const generateCompensation = async (compensation: Api_CompensationRequisition) => {
    if (!compensation?.id) {
      throw Error(
        'user must choose a valid compensation requisition in order to generate a document',
      );
    }
    const filePromise = getAcquisitionFile.execute(compensation.acquisitionFileId);
    const propertiesPromise = getAcquisitionProperties.execute(compensation.acquisitionFileId);
    const compReqH120sPromise = getAcquisitionCompReqH120s.execute(
      compensation.acquisitionFileId,
      true,
    );
    const h120CategoriesPromise = getH120Categories.execute();
    const interestHoldersPromise = getAcquisitionInterestHolders.execute(
      compensation.acquisitionFileId,
    );
    const payeePromise = getCompensationRequisitionPayee.execute(compensation.id);

    const [file, properties, h120Categories, compReqH120s, interestHolders, payee] =
      await Promise.all([
        filePromise,
        propertiesPromise,
        h120CategoriesPromise,
        compReqH120sPromise,
        interestHoldersPromise,
        payeePromise,
      ]);
    if (!file) {
      throw Error('Acquisition file not found');
    }
    file.fileProperties = properties;

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

    const compensationData = new Api_GenerateCompensation(
      compensation,
      fileData,
      h120Categories ?? [],
      compReqH120s ?? [],
      payee,
      client?.value,
    );
    const generatedFile = await generate({
      templateType: FormTemplateTypes.H120,
      templateData: compensationData,
      convertToType: ConvertToTypes.PDF,
    });
    if (generatedFile?.status === ExternalResultStatus.Success!! && generatedFile?.payload) {
      showFile(generatedFile?.payload);
    } else {
      throw Error('Failed to generate file');
    }
  };
  return generateCompensation;
};

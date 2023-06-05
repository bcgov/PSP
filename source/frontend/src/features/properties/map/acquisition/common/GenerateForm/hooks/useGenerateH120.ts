import { useLayerQuery } from 'components/maps/leaflet/LayerPopup';
import { ConvertToTypes } from 'constants/convertToTypes';
import { showFile } from 'features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from 'features/documents/hooks/useDocumentGenerationRepository';
import { FormTemplateTypes } from 'features/properties/map/shared/content/models';
import { useAcquisitionProvider } from 'hooks/repositories/useAcquisitionProvider';
import { useH120CategoryRepository } from 'hooks/repositories/useH120CategoryRepository';
import chunk from 'lodash/chunk';
import { Api_Compensation } from 'models/api/Compensation';
import { ExternalResultStatus } from 'models/api/ExternalResult';
import { Api_GenerateCompensation } from 'models/generate/GenerateCompensation';
import { Api_GenerateFile } from 'models/generate/GenerateFile';
import { Api_GenerateProperty } from 'models/generate/GenerateProperty';
import { useTenant } from 'tenants';
import { getLatLng } from 'utils/mapPropertyUtils';

export const useGenerateH120 = () => {
  const { getAcquisitionFile, getAcquisitionProperties, getAcquisitionCompReqH120s } =
    useAcquisitionProvider();
  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();
  const getH120Categories = useH120CategoryRepository();

  const { electoralLayerUrl } = useTenant();
  const electoralService = useLayerQuery(electoralLayerUrl);

  const getElectoralDistrict = async (property: Api_GenerateProperty) => {
    const latLng = getLatLng(property.location);
    const layerData =
      latLng !== null ? await electoralService.findMetadataByLocation(latLng) : null;

    return (layerData?.ED_NAME as string) ?? '';
  };

  const generateCompensation = async (compensation: Api_Compensation) => {
    if (compensation?.id === undefined) {
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

    const [file, properties, h120Categories, compReqH120s] = await Promise.all([
      filePromise,
      propertiesPromise,
      h120CategoriesPromise,
      compReqH120sPromise,
    ]);
    if (!file) {
      throw Error('Acquisition file not found');
    }
    file.fileProperties = properties;

    // Add ELECTORAL_DISTRICT info to each property (from map layer request)
    const fileData = new Api_GenerateFile(file);
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

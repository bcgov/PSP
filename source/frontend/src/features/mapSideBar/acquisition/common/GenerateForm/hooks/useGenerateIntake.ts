import { FeatureCollection, Geometry } from 'geojson';

import { createFileDownload } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { ComposedProperty } from '@/features/mapSideBar/property/ComposedProperty';
import { useFullyAttributedParcelMapLayer } from '@/hooks/repositories/mapLayer/useFullyAttributedParcelMapLayer';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { Api_GenerateAcquisitionPropertyIntake } from '@/models/generate/acquisition/GenerateAcquisitionPropertyIntake';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { firstOrNull, isValidId, isValidString } from '@/utils';

export const useGenerateIntake = () => {
  const {
    getAcquisitionFile: { execute: getAcquisitionFile },
    //getAcquisitionProperties: { execute: getAcquisitionProperties },
  } = useAcquisitionProvider();

  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();
  const fullyAttrubutedRepository = useFullyAttributedParcelMapLayer();

  const generateAcquisitionIntake = async (
    acquisitionFileId: number,
    selectedProperty: ApiGen_Concepts_FileProperty,
  ) => {
    const file = await getAcquisitionFile(acquisitionFileId);
    if (file) {
      // Retrieve Properties
      let fullyAttributedResult: FeatureCollection<
        Geometry,
        PMBC_FullyAttributed_Feature_Properties
      > | null = null;

      if (isValidId(selectedProperty?.property.pid)) {
        fullyAttributedResult = await fullyAttrubutedRepository.findByPid(
          selectedProperty.property.pid.toString(),
        );
      }

      const fullyAttributed = firstOrNull(fullyAttributedResult.features)?.properties;
      const pimsProperty = selectedProperty.property;

      const composedProperty: ComposedProperty = {
        pid: pimsProperty?.pid?.toString() ?? fullyAttributed?.PID,
        pin: pimsProperty?.pin?.toString() ?? fullyAttributed?.PIN?.toString(),
        planNumber: isValidString(fullyAttributed?.PLAN_NUMBER)
          ? fullyAttributed.PLAN_NUMBER
          : pimsProperty?.planNumber,
        id: pimsProperty?.id ?? 0,
        pimsProperty: pimsProperty,
        parcelMapFeatureCollection: {
          features: [
            {
              type: 'Feature',
              properties: fullyAttributed,
              geometry: undefined,
            },
          ],
          type: 'FeatureCollection',
        },
        crownTenureFeatures: [],
        crownLeaseFeatures: [],
        crownLicenseFeatures: [],
        crownInclusionFeatures: [],
        crownInventoryFeatures: [],
        highwayFeatures: [],
        municipalityFeatures: [],
        ltsaOrders: undefined,
        spcpOrder: undefined,
        propertyAssociations: undefined,
        pimsGeoserverFeatureCollection: undefined,
        bcAssessmentSummary: undefined,
        firstNationFeatures: undefined,
        alrFeatures: undefined,
        electoralFeatures: undefined,
      };

      const intakeData = new Api_GenerateAcquisitionPropertyIntake(file, composedProperty);

      const generatedFile = await generate({
        templateType: ApiGen_CodeTypes_FormTypes.FORMINTAKE, // TODO
        templateData: intakeData,
        convertToType: null,
      });
      generatedFile?.status === ApiGen_CodeTypes_ExternalResponseStatus.Success &&
        generatedFile?.payload &&
        createFileDownload(generatedFile?.payload);
    }
  };

  return generateAcquisitionIntake;
};

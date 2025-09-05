import { FeatureCollection, Geometry } from 'geojson';

import { createFileDownload } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { ComposedProperty } from '@/features/mapSideBar/property/ComposedProperty';
import { useFullyAttributedParcelMapLayer } from '@/hooks/repositories/mapLayer/useFullyAttributedParcelMapLayer';
import { useProperties } from '@/hooks/repositories/useProperties';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { Api_GenerateForm12 } from '@/models/generate/GenerateForm12';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { exists, isValidId, isValidString } from '@/utils';

export const useGenerateForm12 = () => {
  const {
    getMultiplePropertiesById: { execute: getMultipleProperties },
  } = useProperties();

  const fullyAttrubutedRepository = useFullyAttributedParcelMapLayer();

  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();

  const generateForm12 = async (properties: ApiGen_Concepts_Property[]) => {
    const fullyAttributedTasks: Promise<
      FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties>
    >[] = [];

    properties.forEach(pimsProperty => {
      if (isValidId(pimsProperty?.pid)) {
        fullyAttributedTasks.push(fullyAttrubutedRepository.findByPid(pimsProperty.pid.toString()));
      } else if (isValidId(pimsProperty?.pin)) {
        fullyAttributedTasks.push(fullyAttrubutedRepository.findByPin(pimsProperty.pin.toString()));
      }
    });

    // Retrieve Properties
    const propertyIds =
      properties
        ?.filter(exists)
        .map(p => p.id)
        .filter(isValidId) || [];

    const pimsProperties = await getMultipleProperties(propertyIds);
    const fulltyAttributedResults: FeatureCollection<
      Geometry,
      PMBC_FullyAttributed_Feature_Properties
    >[] = await Promise.all(fullyAttributedTasks);

    const flatFA = fulltyAttributedResults.flatMap(x => x.features).map(x => x.properties);

    const composedProperties = pimsProperties.map<ComposedProperty>(pimsProperty => {
      const fullyAttributed = flatFA.find(
        fa =>
          (isValidId(fa.PID_NUMBER) && fa.PID_NUMBER === pimsProperty.pid) ||
          (isValidId(fa.PIN) && fa.PIN === pimsProperty.pin),
      );

      const composed: ComposedProperty = {
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
      };
      return composed;
    });

    const formData = new Api_GenerateForm12(composedProperties);

    const generatedFile = await generate({
      templateType: ApiGen_CodeTypes_FormTypes.FORM12,
      templateData: formData,
      convertToType: null,
    });
    generatedFile?.status === ApiGen_CodeTypes_ExternalResponseStatus.Success &&
      generatedFile?.payload &&
      createFileDownload(generatedFile?.payload);
    return generateForm12;
  };

  return generateForm12;
};

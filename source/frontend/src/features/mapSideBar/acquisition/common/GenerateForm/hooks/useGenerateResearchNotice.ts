import { FeatureCollection, Geometry } from 'geojson';

import { createFileDownload } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { ComposedProperty } from '@/features/mapSideBar/property/ComposedProperty';
import { useGetResearch } from '@/features/mapSideBar/research/hooks/useGetResearch';
import { useFullyAttributedParcelMapLayer } from '@/hooks/repositories/mapLayer/useFullyAttributedParcelMapLayer';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';
import { Api_GenerateNotice } from '@/models/generate/GenerateNotice';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { isValidId, isValidString } from '@/utils';

export const useGenerateResearchNotice = () => {
  const {
    retrieveResearchFile: { execute: getResearchFile },
    retrieveResearchFileProperties: { execute: getResearchProperties },
  } = useGetResearch();

  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();
  const fullyAttributedRepository = useFullyAttributedParcelMapLayer();

  const generateResearchNotice = async (researchFileId: number) => {
    const file = await getResearchFile(researchFileId);
    if (file) {
      // Retrieve Properties
      const pimsProperties = await getResearchProperties(researchFileId);
      file.fileProperties = pimsProperties ?? [];

      const fullyAttributedTasks: Promise<
        FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties>
      >[] = [];

      pimsProperties.forEach(pimsProperty => {
        if (isValidId(pimsProperty?.property?.pid)) {
          fullyAttributedTasks.push(
            fullyAttributedRepository.findByPid(pimsProperty.property.pid.toString()),
          );
        } else if (isValidId(pimsProperty?.property?.pin)) {
          fullyAttributedTasks.push(
            fullyAttributedRepository.findByPin(pimsProperty.property.pin.toString()),
          );
        }
      });

      const fulltyAttributedResults: FeatureCollection<
        Geometry,
        PMBC_FullyAttributed_Feature_Properties
      >[] = await Promise.all(fullyAttributedTasks);

      const flatFA = fulltyAttributedResults.flatMap(x => x.features).map(x => x.properties);

      const composedProperties = pimsProperties.map(pimsResearchProperty => {
        const pimsProperty = pimsResearchProperty.property;
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
          firstNationFeatures: undefined,
          alrFeatures: undefined,
          electoralFeatures: undefined,
        };
        return composed;
      });

      const noticeData = new Api_GenerateNotice(null, null, null, null, composedProperties);

      const generatedFile = await generate({
        templateType: ApiGen_CodeTypes_FormTypes.H0224,
        templateData: noticeData,
        convertToType: null,
      });
      generatedFile?.status === ApiGen_CodeTypes_ExternalResponseStatus.Success &&
        generatedFile?.payload &&
        createFileDownload(generatedFile?.payload);
    }
  };

  return generateResearchNotice;
};

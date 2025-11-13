import { FeatureCollection, Geometry } from 'geojson';

import { createFileDownload } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { ComposedProperty } from '@/features/mapSideBar/property/ComposedProperty';
import { useFullyAttributedParcelMapLayer } from '@/hooks/repositories/mapLayer/useFullyAttributedParcelMapLayer';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';
import { ApiGen_Concepts_AcquisitionFileOwner } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileOwner';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { Api_GenerateNotice } from '@/models/generate/GenerateNotice';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { isValidId, isValidString } from '@/utils';

export const useGenerateNotice = () => {
  const {
    getAcquisitionFile: { execute: getAcquisitionFile },
    getAcquisitionProperties: { execute: getAcquisitionProperties },
  } = useAcquisitionProvider();

  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();
  const fullyAttrubutedRepository = useFullyAttributedParcelMapLayer();

  const generateAcquisitionNotice = async (
    acquisitionFileId: number,
    selectedOwners: ApiGen_Concepts_AcquisitionFileOwner[],
    responsibleTeamMember: ApiGen_Concepts_AcquisitionFileTeam | null,
    signingTeamMember: ApiGen_Concepts_AcquisitionFileTeam | null,
  ) => {
    const file = await getAcquisitionFile(acquisitionFileId);
    if (file) {
      // Retrieve Properties
      const pimsProperties = await getAcquisitionProperties(acquisitionFileId);
      file.fileProperties = pimsProperties ?? [];

      const fullyAttributedTasks: Promise<
        FeatureCollection<Geometry, PMBC_FullyAttributed_Feature_Properties>
      >[] = [];

      pimsProperties.forEach(pimsProperty => {
        if (isValidId(pimsProperty?.property?.pid)) {
          fullyAttributedTasks.push(
            fullyAttrubutedRepository.findByPid(pimsProperty.property.pid.toString()),
          );
        } else if (isValidId(pimsProperty?.property?.pin)) {
          fullyAttributedTasks.push(
            fullyAttrubutedRepository.findByPin(pimsProperty.property.pin.toString()),
          );
        }
      });

      const fulltyAttributedResults: FeatureCollection<
        Geometry,
        PMBC_FullyAttributed_Feature_Properties
      >[] = await Promise.all(fullyAttributedTasks);

      const flatFA = fulltyAttributedResults.flatMap(x => x.features).map(x => x.properties);

      const composedProperties = pimsProperties.map(pimsAcqProperty => {
        const pimsProperty = pimsAcqProperty.property;
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

      const noticeData = new Api_GenerateNotice(
        file.project,
        selectedOwners,
        signingTeamMember,
        responsibleTeamMember,
        composedProperties,
      );

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

  return generateAcquisitionNotice;
};

import moment from 'moment';

import { createFileDownload } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_Concepts_AcquisitionFileProperty } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileProperty';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { Api_GenerateAcquisitionFile } from '@/models/generate/acquisition/GenerateAcquisitionFile';
import { exists, isValidId } from '@/utils';

import {
  ExpropriationForm1Model,
  ExpropriationForm4Model,
  ExpropriationForm5Model,
  ExpropriationForm6Model,
  ExpropriationForm7Model,
  ExpropriationForm9Model,
} from '../../../tabs/expropriation/models';

// Interface for getExtraFields with attached promise creators
export interface GetExtraFieldsFn {
  (
    formModel:
      | ExpropriationForm1Model
      | ExpropriationForm4Model
      | ExpropriationForm5Model
      | ExpropriationForm6Model
      | ExpropriationForm7Model
      | ExpropriationForm9Model,
    acquisitionFileId: number,
    file: ApiGen_Concepts_File,
    properties: ApiGen_Concepts_AcquisitionFileProperty[],
    interestHolders: ApiGen_Concepts_InterestHolder[],
    expAuthority: ApiGen_Concepts_Organization | null,
  ): Promise<any>;
  interestHoldersPromise?: (acquisitionFileId: number) => Promise<any>;
  expAuthorityPromise?: (formModel: any, acquisitionFileId: number) => Promise<any>;
}

export function useGenerateExpropriationForm(
  templateType: string,
  DtoClass: any,
  getExtraFields?: GetExtraFieldsFn,
) {
  const { getAcquisitionFile, getAcquisitionProperties } = useAcquisitionProvider();
  const { generateDocumentDownloadWrappedRequest: generate } = useDocumentGenerationRepository();
  return async (acquisitionFileId: number, formModel: any) => {
    // Prepare all promises in parallel
    const filePromise = getAcquisitionFile.execute(acquisitionFileId);
    const propertiesPromise = getAcquisitionProperties.execute(acquisitionFileId);
    // Allow getExtraFields to optionally provide these, or default to Promise.resolve(null)
    const interestHoldersPromise = getExtraFields?.interestHoldersPromise
      ? getExtraFields.interestHoldersPromise(acquisitionFileId)
      : Promise.resolve(null);
    const expAuthorityPromise = getExtraFields?.expAuthorityPromise
      ? getExtraFields.expAuthorityPromise(formModel, acquisitionFileId)
      : Promise.resolve(null);

    // Await all in parallel
    const [file, properties, interestHolders, expAuthority] = await Promise.all([
      filePromise,
      propertiesPromise,
      interestHoldersPromise,
      expAuthorityPromise,
    ]);
    if (!exists(file)) throw Error('Acquisition file not found');
    file.fileProperties = properties ?? null;

    // getExtraFields can now use all resolved values
    const extraFields = getExtraFields
      ? await getExtraFields(
          formModel,
          acquisitionFileId,
          file,
          properties,
          interestHolders,
          expAuthority,
        )
      : {};

    const fileData = new Api_GenerateAcquisitionFile({
      file: file,
      interestHolders: extraFields.interestHolders ?? formModel.interestHolders ?? [],
    });

    const filePropertyIds = new Set(
      formModel.impactedProperties?.map(fp => fp?.id).filter(isValidId) ?? [],
    );
    const selectedProperties = properties?.filter(fp => filePropertyIds.has(Number(fp.id)));

    const dto = new DtoClass({
      file: fileData,
      interestHolders: extraFields.interestHolders ?? formModel.interestHolders ?? [],
      expropriationAuthority:
        extraFields.expropriationAuthority ?? formModel.expropriationAuthority ?? null,
      impactedProperties: selectedProperties,
      ...extraFields,
    });

    const generatedFile = await generate({
      templateType,
      templateData: dto,
      convertToType: null,
    });

    if (
      generatedFile?.status === ApiGen_CodeTypes_ExternalResponseStatus.Success &&
      generatedFile?.payload
    ) {
      const fileExt = generatedFile?.payload?.fileNameExtension ?? 'docx';
      const fileName = `${templateType}-${file.fileNumber}-${moment().format(
        'yyyyMMDD_hhmmss',
      )}.${fileExt}`;
      createFileDownload(generatedFile?.payload, fileName);
    } else {
      throw Error('Failed to generate file');
    }
  };
}

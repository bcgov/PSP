import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';
import { ApiGen_Concepts_InterestHolderProperty } from '@/models/api/generated/ApiGen_Concepts_InterestHolderProperty';
import { exists } from '@/utils';

import { InterestHolderViewForm, InterestHolderViewRow } from '../update/models';

class StakeholderOrganizer {
  private readonly acquisitionFile: ApiGen_Concepts_AcquisitionFile;
  private readonly interestHolders: ApiGen_Concepts_InterestHolder[] | undefined;

  constructor(
    acquisitionFile: ApiGen_Concepts_AcquisitionFile,
    interestHolders: ApiGen_Concepts_InterestHolder[] | undefined,
  ) {
    this.acquisitionFile = acquisitionFile;
    this.interestHolders = interestHolders;
  }

  getInterestProperties() {
    const allInterestProperties =
      this.interestHolders
        ?.flatMap(interestHolder => interestHolder.interestHolderProperties)
        .filter(exists) ?? [];

    const interestProperties = allInterestProperties
      .filter(
        ip =>
          ip.propertyInterestTypes != null &&
          ip.propertyInterestTypes.some(pit => pit?.id !== 'NIP'),
      )
      .map<ApiGen_Concepts_InterestHolderProperty>(ip => {
        const filteredTypes = ip.propertyInterestTypes?.filter(pit => pit?.id !== 'NIP') ?? null;
        return { ...ip, propertyInterestTypes: filteredTypes };
      });

    return this.generateFormFromProperties(interestProperties);
  }

  getNonInterestProperties() {
    const allInterestProperties =
      this.interestHolders
        ?.flatMap(interestHolder => interestHolder.interestHolderProperties)
        .filter(exists) ?? [];

    const nonInterestProperties = allInterestProperties
      .filter(ip => ip.propertyInterestTypes?.some(pit => pit?.id === 'NIP'))
      .map<ApiGen_Concepts_InterestHolderProperty>(ip => {
        const filteredTypes = ip.propertyInterestTypes?.filter(pit => pit?.id === 'NIP') ?? null;
        return { ...ip, propertyInterestTypes: filteredTypes };
      });

    return this.generateFormFromProperties(nonInterestProperties);
  }

  private generateFormFromProperties(interestProperties: ApiGen_Concepts_InterestHolderProperty[]) {
    const groupedInterestProperties: InterestHolderViewForm[] = [];
    interestProperties.forEach((interestHolderProperty: ApiGen_Concepts_InterestHolderProperty) => {
      const matchingGroup = groupedInterestProperties.find(
        gip => gip.id === interestHolderProperty.acquisitionFilePropertyId,
      );
      const matchingFileProperty = this.acquisitionFile.fileProperties?.find(
        fp => fp.id === interestHolderProperty.acquisitionFilePropertyId,
      );
      if (matchingFileProperty && interestHolderProperty) {
        interestHolderProperty.acquisitionFileProperty = matchingFileProperty;
      }
      const interestHolder = this.interestHolders?.find(
        ih => ih.interestHolderId === interestHolderProperty.interestHolderId,
      );

      if (!matchingGroup) {
        const newGroup = InterestHolderViewForm.fromApi(interestHolderProperty);
        newGroup.groupedPropertyInterests =
          interestHolderProperty.propertyInterestTypes?.map((itc: ApiGen_Base_CodeType<string>) =>
            InterestHolderViewRow.fromApi(interestHolderProperty, interestHolder, itc),
          ) ?? [];
        groupedInterestProperties.push(newGroup);
      } else {
        interestHolderProperty.propertyInterestTypes?.forEach((itc: ApiGen_Base_CodeType<string>) =>
          matchingGroup.groupedPropertyInterests.push(
            InterestHolderViewRow.fromApi(interestHolderProperty, interestHolder, itc),
          ),
        );
      }
    });
    return groupedInterestProperties;
  }
}

export default StakeholderOrganizer;

import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';
import { Api_InterestHolder, Api_InterestHolderProperty } from '@/models/api/InterestHolder';
import Api_TypeCode from '@/models/api/TypeCode';

import { InterestHolderViewForm, InterestHolderViewRow } from '../update/models';

class StakeholderOrganizer {
  private readonly acquisitionFile: Api_AcquisitionFile;
  private readonly interestHolders: Api_InterestHolder[] | undefined;

  constructor(
    acquisitionFile: Api_AcquisitionFile,
    interestHolders: Api_InterestHolder[] | undefined,
  ) {
    this.acquisitionFile = acquisitionFile;
    this.interestHolders = interestHolders;
  }

  getInterestProperties() {
    const allInterestProperties =
      this.interestHolders?.flatMap(interestHolder => interestHolder.interestHolderProperties) ??
      [];

    const interestProperties = allInterestProperties
      .filter(ip => ip.propertyInterestTypes.some(pit => pit?.id !== 'NIP'))
      .map(ip => {
        const filteredTypes = ip.propertyInterestTypes.filter(pit => pit?.id !== 'NIP');
        return { ...ip, propertyInterestTypes: filteredTypes };
      });

    return this.generateFormFromProperties(interestProperties);
  }

  getNonInterestProperties() {
    const allInterestProperties =
      this.interestHolders?.flatMap(interestHolder => interestHolder.interestHolderProperties) ??
      [];

    const nonInterestProperties = allInterestProperties
      .filter(ip => ip.propertyInterestTypes.some(pit => pit?.id === 'NIP'))
      .map(ip => {
        const filteredTypes = ip.propertyInterestTypes.filter(pit => pit?.id === 'NIP');
        return { ...ip, propertyInterestTypes: filteredTypes };
      });

    return this.generateFormFromProperties(nonInterestProperties);
  }

  private generateFormFromProperties(interestProperties: Api_InterestHolderProperty[]) {
    const groupedInterestProperties: InterestHolderViewForm[] = [];
    interestProperties.forEach((interestHolderProperty: Api_InterestHolderProperty) => {
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
        newGroup.groupedPropertyInterests = interestHolderProperty.propertyInterestTypes.map(
          (itc: Api_TypeCode<string>) =>
            InterestHolderViewRow.fromApi(interestHolderProperty, interestHolder, itc),
        );
        groupedInterestProperties.push(newGroup);
      } else {
        interestHolderProperty.propertyInterestTypes.forEach((itc: Api_TypeCode<string>) =>
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

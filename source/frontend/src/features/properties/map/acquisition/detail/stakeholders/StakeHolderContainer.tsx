import * as React from 'react';
import { useEffect } from 'react';

import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';
import { Api_InterestHolder, Api_InterestHolderProperty } from '@/models/api/InterestHolder';

import {
  InterestHolderViewForm,
  InterestHolderViewRow,
  StakeHolderForm,
} from '../../update/stakeholders/models';
import { IStakeHolderViewProps } from './StakeHolderView';

export interface IStakeHolderContainerProps {
  View: React.FC<IStakeHolderViewProps>;
  onEdit: () => void;
  acquisitionFile: Api_AcquisitionFile;
}

export const StakeHolderContainer: React.FunctionComponent<IStakeHolderContainerProps> = ({
  View,
  onEdit,
  acquisitionFile,
}) => {
  const {
    getAcquisitionInterestHolders: {
      execute: getInterestHolders,
      response: apiInterestHolders,
      loading,
    },
  } = useInterestHolderRepository();

  useEffect(() => {
    if (acquisitionFile?.id) {
      getInterestHolders(acquisitionFile.id);
    }
  }, [acquisitionFile.id, getInterestHolders]);

  const form = StakeHolderForm.fromApi(apiInterestHolders ?? []);

  const allInterestProperties = form.interestHolders
    .concat(form.nonInterestPayees)
    .flatMap(interestHolder => interestHolder.impactedProperties);
  const interestProperties = allInterestProperties.filter(ip => ip.interestTypeCode?.id !== 'NIP');
  const nonInterestProperties = allInterestProperties.filter(
    ip => ip.interestTypeCode?.id === 'NIP',
  );

  return (
    <View
      loading={loading}
      groupedInterestProperties={getGroupedInterestProperties(
        interestProperties,
        apiInterestHolders ?? [],
        acquisitionFile,
      )}
      groupedNonInterestProperties={getGroupedInterestProperties(
        nonInterestProperties,
        apiInterestHolders ?? [],
        acquisitionFile,
      )}
      onEdit={onEdit}
    />
  );
};

const getGroupedInterestProperties = (
  interestProperties: Api_InterestHolderProperty[],
  apiInterestHolders: Api_InterestHolder[],
  acquisitionFile: Api_AcquisitionFile,
) => {
  const groupedInterestProperties: InterestHolderViewForm[] = [];
  interestProperties.forEach((interestHolderProperty: Api_InterestHolderProperty) => {
    const matchingGroup = groupedInterestProperties.find(
      gip => gip.id === interestHolderProperty.acquisitionFilePropertyId,
    );
    const matchingFileProperty = acquisitionFile.fileProperties?.find(
      fp => fp.id === interestHolderProperty.acquisitionFilePropertyId,
    );
    if (matchingFileProperty && interestHolderProperty) {
      interestHolderProperty.acquisitionFileProperty = matchingFileProperty;
    }
    const interestHolder = apiInterestHolders?.find(
      ih => ih.interestHolderId === interestHolderProperty.interestHolderId,
    );
    if (!matchingGroup) {
      const newGroup = InterestHolderViewForm.fromApi(interestHolderProperty);
      newGroup.groupedPropertyInterests = [
        InterestHolderViewRow.fromApi(interestHolderProperty, interestHolder),
      ];
      groupedInterestProperties.push(newGroup);
    } else {
      matchingGroup.groupedPropertyInterests.push(
        InterestHolderViewRow.fromApi(interestHolderProperty, interestHolder),
      );
    }
  });
  return groupedInterestProperties;
};

export default StakeHolderContainer;

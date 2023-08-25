import * as React from 'react';
import { useEffect } from 'react';

import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';
import { Api_InterestHolder, Api_InterestHolderProperty } from '@/models/api/InterestHolder';
import Api_TypeCode from '@/models/api/TypeCode';

import { InterestHolderViewForm, InterestHolderViewRow } from '../update/models';
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

  const allInterestProperties =
    apiInterestHolders?.flatMap(interestHolder => interestHolder.interestHolderProperties) ?? [];
  const interestProperties = allInterestProperties
    .filter(ip => ip.propertyInterestTypes.some(pit => pit?.id !== 'NIP'))
    .map(ip => {
      const filteredTypes = ip.propertyInterestTypes.filter(pit => pit?.id !== 'NIP');
      return { ...ip, propertyInterestTypes: filteredTypes };
    });
  const nonInterestProperties = allInterestProperties
    .filter(ip => ip.propertyInterestTypes.some(pit => pit?.id === 'NIP'))
    .map(ip => {
      const filteredTypes = ip.propertyInterestTypes.filter(pit => pit?.id === 'NIP');
      return { ...ip, propertyInterestTypes: filteredTypes };
    });

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
      legacyStakeHolders={acquisitionFile.legacyStakeholders ?? []}
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
};

export default StakeHolderContainer;

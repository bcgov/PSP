import { Api_AcquisitionFileOwner } from 'models/api/AcquisitionFile';
import { Api_Address } from 'models/api/Address';
import { useCallback, useEffect, useState } from 'react';

import { useAcquisitionProvider } from '../hooks/useAcquisitionProvider';
import { DetailAcquisitionFileOwner } from './models';

export interface IAcquisitionOwnersContainerProps {
  acquisitionFileId: number;
  View: React.FC<IAcquisitionOwnersSummaryViewProps>;
}

export interface IAcquisitionOwnersSummaryViewProps {
  ownersList?: DetailAcquisitionFileOwner[];
  isLoading: boolean;
}

const AcquisitionOwnersSummaryContainer: React.FunctionComponent<
  React.PropsWithChildren<IAcquisitionOwnersContainerProps>
> = ({ acquisitionFileId, View }) => {
  const [ownersDetails, setOwnersDetails] = useState<DetailAcquisitionFileOwner[] | undefined>(
    undefined,
  );

  const {
    getAcquisitionOwners: {
      execute: retrieveAcquisitionFileOwners,
      loading: loadingAcquisitionFileOwners,
    },
  } = useAcquisitionProvider();

  const fetchOwnersApi = useCallback(async () => {
    if (acquisitionFileId) {
      const acquisitionOwners = await retrieveAcquisitionFileOwners(acquisitionFileId);
      if (acquisitionOwners !== undefined) {
        const ownerDetailList = acquisitionOwners.map(o => {
          return {
            ownerName: getOwnerDisplayName(o),
            ownerOtherName: o.lastNameOrCorp2?.trim(),
            ownerDisplayAddress: getFormattedAddress(o.address),
          };
        });
        setOwnersDetails([...ownerDetailList]);
      }
    }
  }, [acquisitionFileId, retrieveAcquisitionFileOwners]);

  useEffect(() => {
    fetchOwnersApi();
  }, [fetchOwnersApi]);

  return <View ownersList={ownersDetails} isLoading={loadingAcquisitionFileOwners} />;
};

export default AcquisitionOwnersSummaryContainer;

const getOwnerDisplayName = (owner: Api_AcquisitionFileOwner): string => {
  let nameDisplay = concatValues([owner.givenName, owner.lastNameOrCorp1]);
  if (owner.incorporationNumber && owner.incorporationNumber.trim() !== '') {
    nameDisplay = nameDisplay.concat(` (${owner.incorporationNumber})`);
  }
  return nameDisplay;
};

const getFormattedAddress = (address?: Api_Address): string => {
  if (address === null || address === undefined) {
    return '';
  }

  let addressDisplay = '';
  let streetAddress1 = address?.streetAddress1 ? address?.streetAddress1.trim() : null;
  let streetAddress2 = address?.streetAddress2 ? address?.streetAddress2.trim() : null;
  let streetAddress3 = address?.streetAddress3 ? address?.streetAddress3.trim() : null;
  let streetAddress4 = concatValues(
    [address?.municipality, address?.province?.description, address?.postal],
    ', ',
  );

  if (streetAddress1) {
    addressDisplay = addressDisplay.concat(streetAddress1, '\n');
  }

  if (streetAddress2) {
    addressDisplay = addressDisplay.concat(streetAddress2, '\n');
  }

  if (streetAddress3) {
    addressDisplay = addressDisplay.concat(streetAddress3, '\n');
  }

  if (streetAddress4) {
    addressDisplay = addressDisplay.concat(streetAddress4, '\n');
  }

  if (address?.country?.description) {
    let countryDisplay = address?.country?.description?.trim() || '';
    addressDisplay = addressDisplay.concat(countryDisplay);
  }

  return addressDisplay;
};

const concatValues = (
  nameParts: Array<string | undefined | null>,
  separator: string = ' ',
): string => {
  return nameParts.filter(n => n !== null && n !== undefined && n.trim() !== '').join(separator);
};

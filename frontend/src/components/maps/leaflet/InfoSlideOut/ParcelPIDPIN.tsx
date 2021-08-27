import { IProperty } from 'interfaces';
import * as React from 'react';
import { pidFormatter } from 'utils/propertyUtils';

import { OuterRow } from './InfoContent';
import { ThreeColumnItem } from './ThreeColumnItem';

interface IParcelPIDPIN {
  /** the selected parcel information */
  parcelInfo: IProperty;
}

/**
 * Displays PID/PIN information in property popout for selected parcel
 * @param parcelInfo parcel data
 */
export const ParcelPIDPIN: React.FC<IParcelPIDPIN> = ({ parcelInfo }) => {
  return (
    <OuterRow>
      {parcelInfo?.pid && (
        <ThreeColumnItem leftSideLabel={'PID'} rightSideItem={pidFormatter(parcelInfo?.pid)} />
      )}
      {parcelInfo?.pin && <ThreeColumnItem leftSideLabel={'PIN'} rightSideItem={parcelInfo?.pin} />}
    </OuterRow>
  );
};

export default ParcelPIDPIN;

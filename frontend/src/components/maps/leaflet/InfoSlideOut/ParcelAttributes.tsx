import './InfoSlideOut.scss';

import { Label } from 'components/common/Label';
import { IProperty } from 'interfaces';
import * as React from 'react';
import ListGroup from 'react-bootstrap/ListGroup';

import { OuterRow } from './InfoContent';
import { ThreeColumnItem } from './ThreeColumnItem';

interface IParcelAttributes {
  /** the selected parcel information */
  parcelInfo: IProperty;
  /** whether the user has the correct organization/permissions to view all the details */
  canViewDetails: boolean;
}

/**
 * Displays parcel specific information needed on the information slide out
 * @param parcelInfo the selected parcel data
 * @param canViewDetails user can view all property details
 */
export const ParcelAttributes: React.FC<IParcelAttributes> = ({ parcelInfo, canViewDetails }) => {
  return (
    <>
      <ListGroup>
        <Label className="header">Parcel attributes</Label>
        <OuterRow>
          <ThreeColumnItem
            leftSideLabel={'Lot size:'}
            rightSideItem={(parcelInfo?.landArea ?? '') + ` ${parcelInfo?.areaUnit ?? ''}`}
          />
        </OuterRow>
      </ListGroup>
      <ListGroup>
        <Label className="header">Legal description</Label>
        <OuterRow>
          <ListGroup.Item className="legal">
            {parcelInfo?.landLegalDescription ?? ''}
          </ListGroup.Item>
        </OuterRow>
      </ListGroup>
    </>
  );
};

export default ParcelAttributes;

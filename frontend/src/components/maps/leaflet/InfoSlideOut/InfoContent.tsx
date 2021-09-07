import './InfoSlideOut.scss';

import { LandSvg } from 'components/common/Icons';
import { Label } from 'components/common/Label';
import { PropertyTypes } from 'constants/propertyTypes';
import { IProperty } from 'interfaces';
import { Moment } from 'moment';
import * as React from 'react';
import ListGroup from 'react-bootstrap/ListGroup';
import Row from 'react-bootstrap/Row';
import styled from 'styled-components';
import { formatAddress, pidFormatter } from 'utils';

import ParcelAttributes from './ParcelAttributes';
import { ThreeColumnItem } from './ThreeColumnItem';

/**
 * Compare two dates to evaluation which is earlier.
 * This should handle 'undefined' values by treating them as earlier.
 * @param a First date to compare.
 * @param b Second date to compare.
 * @returns A negative number indicating earlier, 0 as equal, positive number as later.
 */
export const compareDate = (
  a: Date | string | Moment | undefined,
  b: Date | string | Moment | undefined,
): number => {
  if (a === undefined && b === undefined) return 0;
  if (a === undefined && b !== undefined) return -1;
  if (a !== undefined && b === undefined) return 1;
  const aDate = typeof a === 'string' ? new Date(a) : (a as Date);
  const bDate = typeof b === 'string' ? new Date(b) : (b as Date);
  return aDate.valueOf() - bDate.valueOf();
};

interface IInfoContent {
  /** the selected property information */
  propertyInfo: IProperty | null;
  /** The property type [Parcel, Building] */
  propertyTypeId: PropertyTypes | null;
  /** whether the user has the correct organization/permissions to view all the details */
  canViewDetails: boolean;
}

export const OuterRow = styled(Row)`
  margin: 0px 0px 10px 0px;
`;

/**
 * Component that displays the appropriate information about the selected property
 * in the property info slide-out
 * @param {IInfoContent} propertyInfo the selected property
 * @param {IInfoContent} propertyTypeId the property type [Parcel, Building]
 * @param canViewDetails user can view all property details
 */
export const InfoContent: React.FC<IInfoContent> = ({
  propertyInfo,
  propertyTypeId,
  canViewDetails,
}) => {
  return (
    <>
      <ListGroup>
        <Label className="header">
          <LandSvg className="svg" style={{ height: 25, width: 25, marginRight: 5 }} />
          Property Identification
        </Label>
        <OuterRow>
          <ThreeColumnItem leftSideLabel={'PID'} rightSideItem={pidFormatter(propertyInfo?.pid)} />
          <ThreeColumnItem leftSideLabel={'PIN'} rightSideItem={propertyInfo?.pin} />
        </OuterRow>
        <OuterRow>
          {canViewDetails && (
            <>
              {propertyInfo?.name && (
                <ThreeColumnItem leftSideLabel={'Name'} rightSideItem={propertyInfo?.name} />
              )}
              {propertyInfo?.organizations && propertyInfo?.organizations.length && (
                <ThreeColumnItem
                  leftSideLabel={'Organization'}
                  rightSideItem={propertyInfo?.organizations[0].name}
                />
              )}
            </>
          )}
          <ThreeColumnItem leftSideLabel={'Owner'} rightSideItem={null} />
        </OuterRow>
      </ListGroup>
      <ListGroup>
        <Label className="header">Location data</Label>
        <OuterRow>
          <ThreeColumnItem
            leftSideLabel={'Civic address'}
            rightSideItem={formatAddress(propertyInfo?.address)}
          />
          <ThreeColumnItem
            leftSideLabel={'Location'}
            rightSideItem={propertyInfo?.address?.district}
          />
          <ThreeColumnItem
            leftSideLabel={'Municipality'}
            rightSideItem={propertyInfo?.address?.municipality}
          />
          <ThreeColumnItem
            leftSideLabel={'Regional District'}
            rightSideItem={propertyInfo?.address?.region}
          />
        </OuterRow>
      </ListGroup>
      <ParcelAttributes parcelInfo={propertyInfo as IProperty} canViewDetails={canViewDetails} />
    </>
  );
};

export default InfoContent;

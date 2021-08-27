import './InfoSlideOut.scss';

import { BuildingSvg, LandSvg, SubdivisionSvg } from 'components/common/Icons';
import { Label } from 'components/common/Label';
import { PropertyTypes } from 'constants/propertyTypes';
import { IProperty } from 'interfaces';
import { Moment } from 'moment';
import * as React from 'react';
import ListGroup from 'react-bootstrap/ListGroup';
import Row from 'react-bootstrap/Row';
import styled from 'styled-components';

import BuildingAttributes from './BuildingAttributes';
import ParcelAttributes from './ParcelAttributes';
import { ParcelPIDPIN } from './ParcelPIDPIN';
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

const getHeading = (propertyTypeId: PropertyTypes | null) => {
  switch (propertyTypeId) {
    case PropertyTypes.Subdivision:
      return (
        <Label className="header">
          <SubdivisionSvg className="svg" style={{ height: 25, width: 25, marginRight: 5 }} />
          Potential Subdivision
        </Label>
      );
    case PropertyTypes.Building:
      return (
        <Label className="header">
          <BuildingSvg className="svg" style={{ height: 25, width: 25, marginRight: 5 }} />
          Building Identification
        </Label>
      );
    default:
      return (
        <Label className="header">
          <LandSvg className="svg" style={{ height: 25, width: 25, marginRight: 5 }} />
          Parcel Identification
        </Label>
      );
  }
};

/**
 * Component that displays the appropriate information about the selected property
 * in the property info slideout
 * @param {IInfoContent} propertyInfo the selected property
 * @param {IInfoContent} propertyTypeId the property type [Parcel, Building]
 * @param canViewDetails user can view all property details
 */
export const InfoContent: React.FC<IInfoContent> = ({
  propertyInfo,
  propertyTypeId,
  canViewDetails,
}) => {
  const isParcel =
    propertyTypeId !== null &&
    [PropertyTypes.Land, PropertyTypes.Subdivision].includes(propertyTypeId);
  return (
    <>
      <ListGroup>
        {getHeading(propertyTypeId)}
        {isParcel && <ParcelPIDPIN parcelInfo={propertyInfo as IProperty} />}
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
          <ThreeColumnItem
            leftSideLabel={'Classification'}
            rightSideItem={propertyInfo?.classification}
          />
        </OuterRow>
      </ListGroup>
      <ListGroup>
        <Label className="header">Location data</Label>
        <OuterRow>
          <ThreeColumnItem
            leftSideLabel={'Civic address'}
            rightSideItem={propertyInfo?.address?.streetAddress1}
          />
          <ThreeColumnItem
            leftSideLabel={'Location'}
            rightSideItem={propertyInfo?.address?.municipality}
          />
        </OuterRow>
        <OuterRow>
          <ThreeColumnItem leftSideLabel={'Latitude'} rightSideItem={propertyInfo?.latitude} />
          <ThreeColumnItem leftSideLabel={'Longitude'} rightSideItem={propertyInfo?.longitude} />
        </OuterRow>
      </ListGroup>
      {isParcel && (
        <ParcelAttributes parcelInfo={propertyInfo as IProperty} canViewDetails={canViewDetails} />
      )}
      {propertyTypeId === PropertyTypes.Building && (
        <BuildingAttributes
          buildingInfo={propertyInfo as IProperty}
          canViewDetails={canViewDetails}
        />
      )}
    </>
  );
};

export default InfoContent;

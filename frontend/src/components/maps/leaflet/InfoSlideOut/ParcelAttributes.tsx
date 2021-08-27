import './InfoSlideOut.scss';

import { Label } from 'components/common/Label';
import { IProperty } from 'interfaces';
import * as React from 'react';
import ListGroup from 'react-bootstrap/ListGroup';
import { getCurrentYearEvaluation } from 'utils';
import { formatMoney } from 'utils/numberFormatUtils';

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
  let formatAssessed = '';
  const assessed = getCurrentYearEvaluation(parcelInfo?.evaluations ?? [], 1);
  if (assessed) formatAssessed = formatMoney(assessed.value);

  let formatImprovements = '';
  const improvements = getCurrentYearEvaluation(parcelInfo?.evaluations ?? [], 2);
  if (improvements) formatImprovements = formatMoney(improvements.value);

  return (
    <>
      <ListGroup>
        <Label className="header">Parcel attributes</Label>
        <OuterRow>
          <ThreeColumnItem
            leftSideLabel={'Lot size:'}
            rightSideItem={parcelInfo?.landArea + ' hectares'}
          />
        </OuterRow>
      </ListGroup>
      {canViewDetails && (
        <>
          {parcelInfo?.landLegalDescription && (
            <ListGroup>
              <Label className="header">Legal description</Label>
              <OuterRow>
                <ListGroup.Item className="legal">
                  {parcelInfo?.landLegalDescription}
                </ListGroup.Item>
              </OuterRow>
            </ListGroup>
          )}
          <ListGroup>
            <Label className="header">Valuation</Label>
            <OuterRow>
              <ThreeColumnItem leftSideLabel={'Assessed Land:'} rightSideItem={formatAssessed} />
              {!!improvements && (
                <ThreeColumnItem
                  leftSideLabel={'Assessed Building(s):'}
                  rightSideItem={formatImprovements}
                />
              )}
            </OuterRow>
          </ListGroup>
        </>
      )}
    </>
  );
};

export default ParcelAttributes;

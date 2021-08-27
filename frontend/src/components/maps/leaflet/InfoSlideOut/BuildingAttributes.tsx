import './InfoSlideOut.scss';

import { Label } from 'components/common/Label';
import { getIn } from 'formik';
import { IProperty } from 'interfaces';
import * as React from 'react';
import ListGroup from 'react-bootstrap/ListGroup';
import { formatMoney } from 'utils/numberFormatUtils';

import { compareDate, OuterRow } from './InfoContent';
import { ThreeColumnItem } from './ThreeColumnItem';

interface IBuildingAttributes {
  /** the selected building information */
  buildingInfo: IProperty;
  /** whether the user has the correct organization/permissions to view all the details */
  canViewDetails: boolean;
}

/**
 * Displays Building specific information needed on the information slide out
 * @param buildingInfo the selected parcel data
 * @param canViewDetails user can view all property details
 */
export const BuildingAttributes: React.FC<IBuildingAttributes> = ({
  buildingInfo,
  canViewDetails,
}) => {
  let formatAssessed;
  if (buildingInfo?.evaluations?.length) {
    const sortedEvaluations = [...buildingInfo?.evaluations]
      .sort((a, b) => compareDate(a?.evaluatedOn, b?.evaluatedOn))
      .reverse();
    formatAssessed = formatMoney(getIn(sortedEvaluations, '0')?.value);
  } else {
    formatAssessed = '';
  }

  return (
    <>
      <ListGroup>
        <Label className="header">Building Attributes</Label>
        <OuterRow>
          {canViewDetails && (
            <>
              <ThreeColumnItem leftSideLabel={'Name'} rightSideItem={buildingInfo.name} />
              {buildingInfo.description && (
                <ThreeColumnItem
                  leftSideLabel={'Description'}
                  rightSideItem={buildingInfo.description}
                />
              )}
            </>
          )}
          <ThreeColumnItem
            leftSideLabel={'Land area'}
            rightSideItem={buildingInfo.landArea + ' ' + buildingInfo.areaUnit}
          />
        </OuterRow>
      </ListGroup>
      {canViewDetails && (
        <ListGroup>
          <Label className="header">Valuation</Label>
          <OuterRow>
            <ThreeColumnItem leftSideLabel={'Assessed value:'} rightSideItem={formatAssessed} />
          </OuterRow>
        </ListGroup>
      )}
    </>
  );
};

export default BuildingAttributes;

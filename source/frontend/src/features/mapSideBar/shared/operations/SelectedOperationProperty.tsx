import { Col, Row } from 'react-bootstrap';

import { RemoveButton } from '@/components/common/buttons';
import { InlineInput } from '@/components/common/form/styles';
import OverflowTip from '@/components/common/OverflowTip';
import { ZoomIconType, ZoomToLocation } from '@/components/maps/ZoomToLocation';
import DraftCircleNumber from '@/components/propertySelector/selectedPropertyList/DraftCircleNumber';
import { ApiGen_CodeTypes_AreaUnitTypes } from '@/models/api/generated/ApiGen_CodeTypes_AreaUnitTypes';
import { convertArea, formatFormAddress, formatNumber, pidFormatter } from '@/utils';

import { PropertyForm } from '../models';

interface ISelectedOperationPropertyProps {
  property: PropertyForm;
  getMarkerIndex: (property: PropertyForm) => number;
  nameSpace: string;
  onRemove: () => void;
  isEditable?: boolean;
}

export const SelectedOperationProperty: React.FunctionComponent<
  ISelectedOperationPropertyProps
> = ({ property, getMarkerIndex, nameSpace, onRemove, isEditable }) => {
  const getAreaValue = (area: number, unit: string) => {
    const sqm = convertArea(area, unit, ApiGen_CodeTypes_AreaUnitTypes.M2);
    return formatNumber(sqm, 0, 4);
  };

  return (
    <Row className="align-items-center mb-3 no-gutters">
      <Col md={3}>
        <div className="mb-0 d-flex align-items-center">
          <DraftCircleNumber text={(getMarkerIndex(property) + 1).toString()} />
          <OverflowTip
            fullText={property.pid ? pidFormatter(property.pid.toString()) : ''}
            className="pl-3"
          ></OverflowTip>
        </div>
      </Col>
      <Col md={2}>{property.planNumber}</Col>
      <Col md={3}>
        {isEditable ? (
          <InlineInput className="w-75" field={`${nameSpace}.landArea`} />
        ) : (
          getAreaValue(property.landArea ?? 0, property.areaUnit ?? '')
        )}
      </Col>
      <Col md={3}>{formatFormAddress(property?.address) ?? ''}</Col>
      <Col xs="auto">
        <ZoomToLocation icon={ZoomIconType.single} formProperties={[property]} />
      </Col>
      <Col md={'auto'} className="d-flex justify-content-center pl-2">
        <RemoveButton onRemove={onRemove} />
      </Col>
    </Row>
  );
};

export default SelectedOperationProperty;

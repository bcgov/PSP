import { Col, Row } from 'react-bootstrap';

import { RemoveButton } from '@/components/common/buttons';
import { InlineInput } from '@/components/common/form/styles';
import OverflowTip from '@/components/common/OverflowTip';
import DraftCircleNumber from '@/components/propertySelector/selectedPropertyList/DraftCircleNumber';
import { AreaUnitTypes } from '@/constants';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { convertArea, formatApiAddress, formatNumber, pidFormatter } from '@/utils';

interface ISelectedOperationPropertyProps {
  property: ApiGen_Concepts_Property;
  getMarkerIndex: (property: ApiGen_Concepts_Property) => number;
  nameSpace: string;
  onRemove: () => void;
  isEditable?: boolean;
}

export const SelectedOperationProperty: React.FunctionComponent<
  ISelectedOperationPropertyProps
> = ({ property, getMarkerIndex, nameSpace, onRemove, isEditable }) => {
  const getAreaValue = (area: number, unit: string) => {
    const sqm = convertArea(area, unit, AreaUnitTypes.SquareMeters);
    return formatNumber(sqm, 0, 3);
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
          getAreaValue(property.landArea ?? 0, property.areaUnit?.id ?? '')
        )}
      </Col>
      <Col md={3}>{formatApiAddress(property?.address) ?? ''}</Col>
      <Col md={1} className="d-flex justify-content-center">
        <RemoveButton onRemove={onRemove} />
      </Col>
    </Row>
  );
};

export default SelectedOperationProperty;

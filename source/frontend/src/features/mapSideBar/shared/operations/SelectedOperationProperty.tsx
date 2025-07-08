import { geoJSON } from 'leaflet';
import { useCallback } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaSearchPlus } from 'react-icons/fa';

import { LinkButton, RemoveButton } from '@/components/common/buttons';
import { InlineInput } from '@/components/common/form/styles';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import OverflowTip from '@/components/common/OverflowTip';
import DraftCircleNumber from '@/components/propertySelector/selectedPropertyList/DraftCircleNumber';
import { AreaUnitTypes } from '@/constants';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import {
  convertArea,
  exists,
  formatApiAddress,
  formatNumber,
  pidFormatter,
  pimsGeomeryToGeometry,
} from '@/utils';

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
    return formatNumber(sqm, 0, 4);
  };

  const mapMachine = useMapStateMachine();

  const onZoomToProperty = useCallback(
    (property: ApiGen_Concepts_Property) => {
      const geom = property?.boundary ?? pimsGeomeryToGeometry(property?.location);
      const bounds = geoJSON(geom).getBounds();

      if (exists(bounds)) {
        mapMachine.requestFlyToBounds(bounds);
      }
    },
    [mapMachine],
  );

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
      <Col xs="auto">
        <LinkButton onClick={() => onZoomToProperty(property)}>
          <FaSearchPlus size={18} className="ml-4" />
        </LinkButton>
      </Col>
      <Col md={'auto'} className="d-flex justify-content-center pl-2">
        <RemoveButton onRemove={onRemove} />
      </Col>
    </Row>
  );
};

export default SelectedOperationProperty;

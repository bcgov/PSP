import { RemoveButton } from 'components/common/buttons';
import { InlineInput } from 'components/common/form/styles';
import OverflowTip from 'components/common/OverflowTip';
import { IMapProperty } from 'components/propertySelector/models';
import DraftCircleNumber from 'components/propertySelector/selectedPropertyList/DraftCircleNumber';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import { withNameSpace } from 'utils/formUtils';
import { getPropertyName, NameSourceType } from 'utils/mapPropertyUtils';

export interface ISelectedPropertyRowProps {
  index: number;
  nameSpace?: string;
  onRemove: () => void;
  property: IMapProperty;
}

export const SelectedPropertyRow: React.FunctionComponent<
  React.PropsWithChildren<ISelectedPropertyRowProps>
> = ({ nameSpace, onRemove, index, property }) => {
  const propertyName = getPropertyName(property);
  let propertyIdentifier = '';
  switch (propertyName.label) {
    case NameSourceType.PID:
    case NameSourceType.PIN:
    case NameSourceType.PLAN:
    case NameSourceType.ADDRESS:
      propertyIdentifier = `${propertyName.label}: ${propertyName.value}`;
      break;
    case NameSourceType.LOCATION:
      propertyIdentifier = `${propertyName.value}`;
      break;
    default:
      propertyIdentifier = '';
      break;
  }
  return (
    <Row className="align-items-center mb-3 no-gutters">
      <Col md={3}>
        <div className="mb-0 d-flex align-items-center">
          <DraftCircleNumber text={(index + 1).toString()} />
          <OverflowTip fullText={propertyIdentifier} className="pl-3"></OverflowTip>
        </div>
      </Col>
      <Col md={7}>
        <InlineInput
          className="mb-0 w-100"
          label=""
          field={withNameSpace(nameSpace, 'name')}
          displayErrorTooltips={true}
        />
      </Col>
      <Col md={2}>
        <RemoveButton onRemove={onRemove} />
      </Col>
    </Row>
  );
};

export default SelectedPropertyRow;

import { RemoveButton } from 'components/common/buttons';
import { InlineInput } from 'components/common/form/styles';
import { NoPaddingRow } from 'components/common/styles';
import DraftCircleNumber from 'features/properties/selector/components/DraftCircleNumber';
import { IMapProperty } from 'features/properties/selector/models';
import { getPropertyName, NameSourceType } from 'features/properties/selector/utils';
import * as React from 'react';
import { Col } from 'react-bootstrap';
import { withNameSpace } from 'utils/formUtils';

export interface ISelectedPropertyRowProps {
  index: number;
  nameSpace?: string;
  onRemove: () => void;
  property: IMapProperty;
}

export const SelectedPropertyRow: React.FunctionComponent<ISelectedPropertyRowProps> = ({
  nameSpace,
  onRemove,
  index,
  property,
}) => {
  const propertyName = getPropertyName(property);
  let propertyIdentifier = '';
  switch (propertyName.label) {
    case NameSourceType.PID:
      propertyIdentifier = `${propertyName.label}: ${propertyName.value}`;
      break;
    case NameSourceType.PIN:
      propertyIdentifier = `${propertyName.label}: ${propertyName.value}`;
      break;
    case NameSourceType.PLAN:
      propertyIdentifier = `${propertyName.label}: ${propertyName.value}`;
      break;
    case NameSourceType.LOCATION:
      propertyIdentifier = `${propertyName.value}`;
      break;
    case NameSourceType.RESEARCH:
      propertyIdentifier = `${propertyName.value}`;
      break;
    default:
      propertyIdentifier = '';
      break;
  }
  return (
    <NoPaddingRow className="align-items-center mb-3">
      <Col md={3}>
        <p className="mb-0">
          <DraftCircleNumber text={(index + 1).toString()} />
          <span className="pl-3">{propertyIdentifier}</span>
        </p>
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
    </NoPaddingRow>
  );
};

export default SelectedPropertyRow;

import { RemoveButton } from 'components/common/buttons';
import { InlineInput } from 'components/common/form/styles';
import { NoPaddingRow } from 'components/common/styles';
import DraftCircleNumber from 'features/properties/selector/components/DraftCircleNumber';
import { getIn, useFormikContext } from 'formik';
import { compact } from 'lodash';
import * as React from 'react';
import { Col } from 'react-bootstrap';
import { pidFormatter } from 'utils';
import { withNameSpace } from 'utils/formUtils';

import { ResearchForm } from './models';

export interface ISelectedPropertyRowProps {
  index: number;
  nameSpace?: string;
  onRemove: () => void;
}

export const SelectedPropertyRow: React.FunctionComponent<ISelectedPropertyRowProps> = ({
  nameSpace,
  onRemove,
  index,
}) => {
  const { values } = useFormikContext<ResearchForm>();
  const pid = getIn(values, `${withNameSpace(nameSpace, 'pid')}`);
  const pin = getIn(values, `${withNameSpace(nameSpace, 'pin')}`);
  const planNumber = getIn(values, `${withNameSpace(nameSpace, 'planNumber')}`);
  let propertyIdentifier = getLatLngText(values, nameSpace);

  if (!!pid) {
    propertyIdentifier = `PID: ${pidFormatter(pid)}`;
  } else if (!!pin) {
    propertyIdentifier = `PIN: ${pin}`;
  } else if (!!planNumber) {
    propertyIdentifier = `Plan #: ${planNumber}`;
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

const getLatLngText = (values: ResearchForm, nameSpace?: string) => {
  const latitude = getIn(values, `${withNameSpace(nameSpace, 'latitude')}`)?.toFixed(5);
  const longitude = getIn(values, `${withNameSpace(nameSpace, 'longitude')}`)?.toFixed(5);
  return compact([latitude, longitude]).join(', ');
};

export default SelectedPropertyRow;

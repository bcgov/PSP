import { InlineFlexDiv } from 'components/common/styles';
import * as API from 'constants/API';
import { getIn, useFormikContext } from 'formik';
import { useLookupCodeHelpers } from 'hooks/useLookupCodeHelpers';
import { IFormLease } from 'interfaces';
import * as React from 'react';
import { Row } from 'react-bootstrap';
import { MdClose } from 'react-icons/md';
import styled from 'styled-components';
import { withNameSpace } from 'utils/formUtils';

import * as Styled from './styles';

interface IPropertyRowProps {
  nameSpace?: string;
  onRemove: () => void;
}

export const PropertyRow: React.FunctionComponent<React.PropsWithChildren<IPropertyRowProps>> = ({
  nameSpace,
  onRemove,
}) => {
  const { getOptionsByType } = useLookupCodeHelpers();
  const areaUnitTypes = getOptionsByType(API.AREA_UNIT_TYPES);
  const { values } = useFormikContext<IFormLease>();
  const pid = getIn(values, `${withNameSpace(nameSpace, 'property.pid')}`);
  const pin = getIn(values, `${withNameSpace(nameSpace, 'property.pin')}`);
  const coordinates = getIn(values, `${withNameSpace(nameSpace, 'property.coordinates')}`);
  return (
    <Row key={nameSpace}>
      <Styled.PropertyCol>
        <Styled.SmallInlineInput
          disabled={!!pin && !pid}
          label="PID:"
          field={withNameSpace(nameSpace, 'property.pid')}
          type="number"
          displayErrorTooltips={true}
        />
        <Styled.SmallInlineInput
          disabled={!!pid && !pin}
          label="PIN:"
          field={withNameSpace(nameSpace, 'property.pin')}
          type="number"
          displayErrorTooltips={true}
        />
        <Styled.SmallInlineInput
          disabled={!!pid || !!pin}
          label="Coordinates:"
          field={withNameSpace(nameSpace, 'property.coordinates')}
          displayErrorTooltips={true}
        />

        <StyledInlineFlexDiv>
          <Styled.SmallInlineInput
            disabled={!pid && !pin && !coordinates}
            label="Lease Area:"
            field={withNameSpace(nameSpace, 'landArea')}
          />
          <Styled.SmallInlineSelect
            disabled={!pid && !pin && !coordinates}
            options={areaUnitTypes}
            field={withNameSpace(nameSpace, 'areaUnitType.id')}
            placeholder=" "
          />
          <Styled.RemoveButton variant="link" onClick={onRemove}>
            <MdClose />
            Remove
          </Styled.RemoveButton>
        </StyledInlineFlexDiv>
      </Styled.PropertyCol>
    </Row>
  );
};

const StyledInlineFlexDiv = styled(InlineFlexDiv)`
  gap: 1rem;
`;

export default PropertyRow;

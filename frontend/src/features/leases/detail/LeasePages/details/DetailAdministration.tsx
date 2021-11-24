import { Form, Input } from 'components/common/form';
import TooltipIcon from 'components/common/TooltipIcon';
import * as Styled from 'features/leases/detail/styles';
import * as React from 'react';
import styled from 'styled-components';
import { withNameSpace } from 'utils/formUtils';

export interface IDetailAdministrationProps {
  nameSpace?: string;
  disabled?: boolean;
}

/**
 * Sub-form containing lease detail administration fields
 * @param {IDetailAdministrationProps} param0
 */
export const DetailAdministration: React.FunctionComponent<IDetailAdministrationProps> = ({
  nameSpace,
  disabled,
}) => {
  return (
    <li>
      <Styled.LeaseH3>Administration</Styled.LeaseH3>
      <Styled.FormGrid>
        <Form.Label>Program:</Form.Label>
        <LargeTextInput disabled={disabled} field={withNameSpace(nameSpace, 'programName')} />
        <br />
        <Form.Label>Type</Form.Label>
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'type.description')} />
        <Form.Label>Receivable To:</Form.Label>
        <Input
          disabled={disabled}
          field={withNameSpace(nameSpace, 'paymentReceivableType.description')}
        />
        <Form.Label>Category:</Form.Label>
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'categoryType.description')} />
        <Form.Label>Purpose:</Form.Label>
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'purposeType.description')} />
        <br />
        <Form.Label>
          Initiator:&nbsp;
          <TooltipIcon
            toolTipId="initiator-tooltip"
            toolTip="Where did this lease/license initiate?"
          />
        </Form.Label>
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'initiatorType.description')} />
        <Form.Label>
          Responsibility:&nbsp;
          <TooltipIcon toolTipId="responsibility-tooltip" toolTip="Who is currently responsible?" />
        </Form.Label>
        <Input
          disabled={disabled}
          field={withNameSpace(nameSpace, 'responsibilityType.description')}
        />
        <Form.Label>Effective Date:</Form.Label>
        <Input
          disabled={disabled}
          field={withNameSpace(nameSpace, 'responsibilityEffectiveDate')}
        />
        <br />
        <Form.Label>L-file #:</Form.Label>
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'lFileNo')} />
        <Form.Label>MoTI contact:</Form.Label>
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'motiName')} />
        <Form.Label>LIS #:</Form.Label>
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'tfaFileNo')} />
        <Form.Label>PS #:</Form.Label>
        <Input disabled={disabled} field={withNameSpace(nameSpace, 'psFileNo')} />
      </Styled.FormGrid>
    </li>
  );
};

const LargeTextInput = styled(Input)`
  input.form-control {
    font-size: 1.8rem;
  }
`;

export default DetailAdministration;

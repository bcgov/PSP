import { Form, Input } from 'components/common/form';
import * as React from 'react';
import styled from 'styled-components';
import { withNameSpace } from 'utils/formUtils';

import * as Styled from '../styles';

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

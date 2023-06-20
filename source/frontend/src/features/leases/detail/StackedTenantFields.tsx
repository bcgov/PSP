import * as React from 'react';
import styled from 'styled-components';

import OverflowTip from '@/components/common/OverflowTip';
import { InlineFlexDiv } from '@/components/common/styles';
import { Api_LeaseTenant } from '@/models/api/LeaseTenant';

import { getAllNames } from '../leaseUtils';
export interface IStackedTenantFieldsProps {
  tenants?: Api_LeaseTenant[];
}

/**
 * Layout component that displays vertically stacked tenant names
 * @param {IStackedTenantFieldsProps} param0
 */
export const StackedTenantFields: React.FunctionComponent<
  React.PropsWithChildren<IStackedTenantFieldsProps>
> = ({ tenants }) => {
  const commaSeparatedNames = getAllNames(tenants ?? []).join(', ');
  return (
    <>
      <StyledStackedDivs>
        <label>Tenant:</label>
        <OverflowTip fullText={commaSeparatedNames} />
      </StyledStackedDivs>
    </>
  );
};

const StyledStackedDivs = styled(InlineFlexDiv)`
  width: 100%;
  text-align: left;

  div {
    font-family: 'BCSans-Bold';
  }
  @media only screen and (max-width: 1250px) {
    justify-content: center;
  }
`;

export default StackedTenantFields;

import OverflowTip from 'components/common/OverflowTip';
import { InlineFlexDiv } from 'components/common/styles';
import { ILease } from 'interfaces/ILease';
import * as React from 'react';
import styled from 'styled-components';

import { getAllNames } from '../leaseUtils';
export interface IStackedTenantFieldsProps {
  lease?: ILease;
}

/**
 * Layout component that displays vertically stacked tenant names
 * @param {IStackedTenantFieldsProps} param0
 */
export const StackedTenantFields: React.FunctionComponent<IStackedTenantFieldsProps> = ({
  lease,
}) => {
  return (
    <>
      <StyledStackedDivs>
        <label>Tenant:</label>
        <OverflowTip fullText={getAllNames(lease)} />
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

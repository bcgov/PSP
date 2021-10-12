import OverflowTip from 'components/common/OverflowTip';
import { InlineFlexDiv } from 'components/common/styles';
import { ILease } from 'interfaces/ILease';
import * as React from 'react';
import styled from 'styled-components';
export interface IStackedPidTenantFieldsProps {
  lease?: ILease;
}

/**
 * Layout component that displays vertically stacked lease pids and tenant names
 * @param {IStackedPidTenantFieldsProps} param0
 */
export const StackedPidTenantFields: React.FunctionComponent<IStackedPidTenantFieldsProps> = ({
  lease,
}) => {
  const properties = lease?.properties ?? [];
  const pids = properties.map(property => property.pid);
  return (
    <>
      <StyledStackedDivs>
        <label>PID:</label>
        <OverflowTip title="pids" fullText={pids.join(', ')} />
      </StyledStackedDivs>
      <StyledStackedDivs>
        <label>Tenant:</label>
        <OverflowTip title="tenant" fullText={lease?.tenant?.name} />
      </StyledStackedDivs>
    </>
  );
};

const StyledStackedDivs = styled(InlineFlexDiv)`
  width: 100%;
  text-align: left;
  div {
    font-weight: 700;
  }
`;

export default StackedPidTenantFields;

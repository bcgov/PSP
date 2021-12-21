import { ISelectedTableHeaderProps } from 'components/common/form';
import * as CommonStyled from 'components/common/styles';
import * as React from 'react';

import * as Styled from './styles';

const SelectedTableHeader: React.FC<ISelectedTableHeaderProps> = ({ selectedCount }) => {
  return (
    <>
      <Styled.TenantH2>Step 2 - Review Tenant(s)</Styled.TenantH2>
      <p>
        Your selections will be saved to the lease/license when you click the final "Save" button.
      </p>
      <CommonStyled.SelectedText>
        {selectedCount} Tenants associated with this Lease/License
      </CommonStyled.SelectedText>
    </>
  );
};

export default SelectedTableHeader;

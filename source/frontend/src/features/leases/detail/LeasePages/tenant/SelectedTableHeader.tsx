import * as React from 'react';

import { ISelectedTableHeaderProps } from '@/components/common/form';
import * as CommonStyled from '@/components/common/styles';

const SelectedTableHeader: React.FC<React.PropsWithChildren<ISelectedTableHeaderProps>> = ({
  selectedCount,
}) => {
  return (
    <>
      <CommonStyled.SelectedText>
        {selectedCount} Tenants associated with this Lease/License
      </CommonStyled.SelectedText>
    </>
  );
};

export default SelectedTableHeader;

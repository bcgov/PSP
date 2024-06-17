import { ISelectedTableHeaderProps } from '@/components/common/form';
import * as CommonStyled from '@/components/common/styles';

const SelectedTableHeader: React.FC<React.PropsWithChildren<ISelectedTableHeaderProps>> = ({
  selectedCount,
}) => {
  return (
    <>
      <CommonStyled.SelectedText>
        {selectedCount} Tenants associated with this Lease/Licence
      </CommonStyled.SelectedText>
    </>
  );
};

export default SelectedTableHeader;

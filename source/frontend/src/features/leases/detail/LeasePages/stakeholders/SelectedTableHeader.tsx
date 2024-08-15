import { ISelectedTableHeaderProps } from '@/components/common/form';
import * as CommonStyled from '@/components/common/styles';

const SelectedTableHeader: React.FC<React.PropsWithChildren<ISelectedTableHeaderProps>> = ({
  selectedCount,
  isPayableLease,
}) => {
  return (
    <>
      <CommonStyled.SelectedText>
        {selectedCount} {isPayableLease ? 'Payee(s)' : 'Tenant(s)'} associated with this
        Lease/Licence
      </CommonStyled.SelectedText>
    </>
  );
};

export default SelectedTableHeader;

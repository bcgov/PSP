import { FormSection } from 'components/common/form/styles';
import { getIn, useFormikContext } from 'formik';
import { IFormLease, ILeaseSecurityDeposit, ILeaseSecurityDepositReturn } from 'interfaces';

import DepositsReceivedTable from './components/DepositsReceivedTable/DepositsReceivedTable';
import DepositsReturnedTable from './components/DepositsReturnedTable/DepositsReturnedTable';
import * as Styled from './styles';

export interface IDepositsProps {}

export const Deposits: React.FunctionComponent<IDepositsProps> = props => {
  const { values } = useFormikContext<IFormLease>();
  const securityDeposits: ILeaseSecurityDeposit[] = getIn(values, 'securityDeposits') ?? [];
  const depositReturns: ILeaseSecurityDepositReturn[] =
    getIn(values, 'securityDepositReturns') ?? [];

  return (
    <Styled.DepositsContainer>
      <FormSection>
        <Styled.SectiontHeader>Deposits Received</Styled.SectiontHeader>
        <DepositsReceivedTable dataSource={securityDeposits} />
      </FormSection>

      <FormSection>
        <Styled.SectiontHeader>Deposits Returned</Styled.SectiontHeader>
        <DepositsReturnedTable dataSource={depositReturns} />
      </FormSection>
    </Styled.DepositsContainer>
  );
};

export default Deposits;

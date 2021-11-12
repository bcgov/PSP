import { Form } from 'components/common/form';
import * as Styled from 'features/leases/detail/LeasePages/improvements/styles';
import { getIn, useFormikContext } from 'formik';
import { IFormLease } from 'interfaces';
import { Form as BsForm } from 'react-bootstrap';

export interface ISummaryProps {
  disabled?: boolean;
}

/**
 * Sub-form containing lease improvements summary
 * @param {ISummaryProps} param0
 */
export const Summary: React.FunctionComponent<ISummaryProps> = ({ disabled }) => {
  const { values } = useFormikContext<IFormLease>();
  const isResidential = getIn(values, 'isResidential');
  const isCommercial = getIn(values, 'isCommercialBuilding');
  const isOther = getIn(values, 'isOtherImprovement');

  return (
    <>
      <Styled.LeaseH3>Agreement includes:</Styled.LeaseH3>
      <Styled.FormGrid>
        <Form.Label>Residential:</Form.Label>
        <BsForm.Control disabled={disabled} value={yesNo(isResidential)} />
        <Form.Label>Commercial:</Form.Label>
        <BsForm.Control disabled={disabled} value={yesNo(isCommercial)} />
        <Form.Label>Other improvements:</Form.Label>
        <BsForm.Control disabled={disabled} value={yesNo(isOther)} />
      </Styled.FormGrid>
    </>
  );
};

const yesNo = (value: boolean) => (!!value ? 'Yes' : 'No');

export default Summary;

import { InlineInput } from 'components/common/form/styles';
import * as Styled from 'features/leases/detail/styles';
import { getIn, useFormikContext } from 'formik';
import { IFormLease } from 'interfaces';
import { ILeaseTerm } from 'interfaces/ILeaseTerm';
import moment from 'moment';
import * as React from 'react';
import { withNameSpace } from 'utils/formUtils';

import { leaseTermColumns } from './columns';

export interface IDetailTermsProps {
  nameSpace?: string;
  disabled?: boolean;
}

/**
 * Display all term related information in a table.
 * @param param0
 */
export const DetailTerms: React.FunctionComponent<IDetailTermsProps> = ({
  disabled,
  nameSpace,
}) => {
  const formikProps = useFormikContext<IFormLease>();
  const { values } = formikProps;
  const startDate = getIn(values, withNameSpace(nameSpace, 'startDate'));
  const expiryDate = getIn(values, withNameSpace(nameSpace, 'expiryDate'));
  const renewalDate = getIn(values, withNameSpace(nameSpace, 'renewalDate'));

  //TODO: this should be populated by the API when the schema is updated to support renewal terms.
  const terms: ILeaseTerm[] = [
    {
      id: 'initial term',
      startDate: startDate,
      endDate: expiryDate,
      renewalDate: renewalDate,
      termStatus: 'exercised',
    },
  ];
  const currentTerm = terms.find(term => moment().isSameOrBefore(moment(term.endDate), 'day'));

  return (
    <>
      <Styled.LeaseH3>Terms</Styled.LeaseH3>
      <Styled.TableHeadFields>
        <Styled.LeftInlineFastCurrencyInput
          formikProps={formikProps}
          disabled={disabled}
          label="Payment amount:"
          field="amount"
        />
        <InlineInput disabled={disabled} label="Payment frequency:" field="paymentFrequencyType" />
        <InlineInput disabled={disabled} label="Total renewal terms:" field="renewalCount" />
      </Styled.TableHeadFields>
      <Styled.TermsTable<ILeaseTerm>
        name="leaseTermsTable"
        data={terms || []}
        columns={leaseTermColumns}
        hideToolbar
        selectedRows={currentTerm ? [currentTerm] : []}
      ></Styled.TermsTable>
    </>
  );
};

export default DetailTerms;

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
export const DetailTerms: React.FunctionComponent<React.PropsWithChildren<IDetailTermsProps>> = ({
  disabled,
  nameSpace,
}) => {
  const formikProps = useFormikContext<IFormLease>();
  const { values } = formikProps;
  const terms = getIn(values, withNameSpace(nameSpace, 'terms'));
  const currentTerm = terms.find((term: ILeaseTerm) =>
    moment().isSameOrBefore(moment(term.expiryDate), 'day'),
  );

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
        <InlineInput
          disabled={disabled}
          label="Payment frequency:"
          field="paymentFrequencyType.description"
        />
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

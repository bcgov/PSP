import { getIn, useFormikContext } from 'formik';
import moment from 'moment';

import { InlineInput } from '@/components/common/form/styles';
import * as Styled from '@/features/leases/detail/styles';
import { LeaseFormModel } from '@/features/leases/models';
import { ApiGen_Concepts_LeasePeriod } from '@/models/api/generated/ApiGen_Concepts_LeasePeriod';
import { withNameSpace } from '@/utils/formUtils';

import { leasePeriodColumns } from './columns';

export interface IDetailPeriodsProps {
  nameSpace?: string;
  disabled?: boolean;
}

/**
 * Display all period related information in a table.
 * @param param0
 */
export const DetailPeriods: React.FunctionComponent<
  React.PropsWithChildren<IDetailPeriodsProps>
> = ({ disabled, nameSpace }) => {
  const formikProps = useFormikContext<LeaseFormModel>();
  const { values } = formikProps;
  const periods = getIn(values, withNameSpace(nameSpace, 'periods'));
  const currentPeriod = periods.find((period: ApiGen_Concepts_LeasePeriod) =>
    moment().isSameOrBefore(moment(period.expiryDate), 'day'),
  );

  return (
    <>
      <Styled.LeaseH3>Periods</Styled.LeaseH3>
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
        <InlineInput disabled={disabled} label="Total renewal periods:" field="renewalCount" />
      </Styled.TableHeadFields>
      <Styled.PeriodsTable<ApiGen_Concepts_LeasePeriod>
        name="leasePeriodsTable"
        data={periods || []}
        columns={leasePeriodColumns}
        hideToolbar
        selectedRows={currentPeriod ? [currentPeriod] : []}
      ></Styled.PeriodsTable>
    </>
  );
};

export default DetailPeriods;

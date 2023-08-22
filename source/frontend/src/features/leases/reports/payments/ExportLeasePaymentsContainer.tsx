import { Formik } from 'formik';
import * as React from 'react';
import { toast } from 'react-toastify';

import { InlineSelect } from '@/components/common/form/styles';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { ClickableDownload } from '@/components/layout/SideNavBar/styles';
import {
  FlexRowDiv,
  UnOrderedListNoStyle,
} from '@/features/leases/detail/LeasePages/payment/styles';
import { useLeaseExport } from '@/features/leases/hooks/useLeaseExport';
import { getCurrentFiscalYear } from '@/utils';

import { generateFiscalYearOptions } from '../reportUtils';

interface IExportLeasePaymentsContainerProps {}

export interface IExportLeasePaymentsContainer {
  fiscalYear: number;
}

export const ExportLeasePaymentsContainer: React.FunctionComponent<
  React.PropsWithChildren<IExportLeasePaymentsContainerProps>
> = props => {
  const { exportLeasePayments } = useLeaseExport();
  const fiscalYearOptions = generateFiscalYearOptions(1990, getCurrentFiscalYear());
  return (
    <Formik
      initialValues={{
        fiscalYear: fiscalYearOptions[fiscalYearOptions.length - 1].value as number,
      }}
      onSubmit={async (values: IExportLeasePaymentsContainer) => {
        try {
          await exportLeasePayments(values.fiscalYear);
        } catch (error) {
          toast.error(
            'Failed to export report. If this error persists, please contact your System Administrator.',
          );
        }
      }}
    >
      {({ submitForm }) => (
        <UnOrderedListNoStyle>
          <li>
            <FlexRowDiv>
              <InlineSelect
                label="Select fiscal year"
                field="fiscalYear"
                options={fiscalYearOptions}
              ></InlineSelect>
              <TooltipWrapper toolTipId="download-lease-payments-report" toolTip="Download">
                <ClickableDownload title="Export Aggregated Report" onClick={() => submitForm()} />
              </TooltipWrapper>
            </FlexRowDiv>
          </li>
        </UnOrderedListNoStyle>
      )}
    </Formik>
  );
};

export default ExportLeasePaymentsContainer;

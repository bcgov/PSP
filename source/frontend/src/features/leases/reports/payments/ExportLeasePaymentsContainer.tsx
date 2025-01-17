import { Formik } from 'formik';
import { toast } from 'react-toastify';

import DownloadButton from '@/components/common/buttons/DownloadButton';
import { InlineSelect } from '@/components/common/form/styles';
import {
  FlexRowDiv,
  UnOrderedListNoStyle,
} from '@/features/leases/detail/LeasePages/payment/styles';
import { useLeaseExport } from '@/features/leases/hooks/useLeaseExport';
import { getCurrentFiscalYear } from '@/utils';

import { generateFiscalYearOptions } from '../reportUtils';

export interface IExportLeasePaymentsContainer {
  fiscalYear: number;
}

export const ExportLeasePaymentsContainer: React.FunctionComponent<unknown> = () => {
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
            <FlexRowDiv className="align-items-center">
              <InlineSelect
                label="Select fiscal year"
                field="fiscalYear"
                options={fiscalYearOptions}
                className="mb-0"
              ></InlineSelect>
              <DownloadButton
                title="Export Lease Payments Report"
                toolId="download-lease-payments-report"
                toolText="Download"
                onClick={() => submitForm()}
              />
            </FlexRowDiv>
          </li>
        </UnOrderedListNoStyle>
      )}
    </Formik>
  );
};

export default ExportLeasePaymentsContainer;

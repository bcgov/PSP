import { Formik } from 'formik';
import * as React from 'react';
import { FaDownload } from 'react-icons/fa';
import { toast } from 'react-toastify';
import styled from 'styled-components';

import { InlineSelect } from '@/components/common/form/styles';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import {
  FlexRowDiv,
  UnOrderedListNoStyle,
} from '@/features/leases/detail/LeasePages/payment/styles';
import { useLeaseExport } from '@/features/leases/hooks/useLeaseExport';
import { getCurrentFiscalYear } from '@/utils';

import { generateFiscalYearOptions } from '../reportUtils';

interface IExportAggregatedLeasesContainerProps {}

export interface IExportAggregatedLeasesContainer {
  fiscalYear: number;
}

export const ExportAggregatedLeasesContainer: React.FunctionComponent<
  React.PropsWithChildren<IExportAggregatedLeasesContainerProps>
> = props => {
  const { exportAggregatedLeases } = useLeaseExport();
  const fiscalYearOptions = generateFiscalYearOptions(1990, getCurrentFiscalYear());
  return (
    <Formik
      initialValues={{
        fiscalYear: fiscalYearOptions[fiscalYearOptions.length - 1].value as number,
      }}
      onSubmit={async (values: IExportAggregatedLeasesContainer) => {
        try {
          await exportAggregatedLeases(values.fiscalYear);
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
              <TooltipWrapper toolTipId="download-aggregated-lease-report" toolTip="Download">
                <ClickableDownload title="Export Aggregated Report" onClick={() => submitForm()} />
              </TooltipWrapper>
            </FlexRowDiv>
          </li>
        </UnOrderedListNoStyle>
      )}
    </Formik>
  );
};

const ClickableDownload = styled(FaDownload)`
  &:hover {
    cursor: pointer;
  }
  align-self: center;
  color: ${({ theme }) => theme.css.slideOutBlue};
`;

export default ExportAggregatedLeasesContainer;

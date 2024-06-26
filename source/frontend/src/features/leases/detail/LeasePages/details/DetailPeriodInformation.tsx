import { getIn, useFormikContext } from 'formik';
import moment from 'moment';
import styled from 'styled-components';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_LeasePeriod } from '@/models/api/generated/ApiGen_Concepts_LeasePeriod';
import { withNameSpace } from '@/utils/formUtils';

import { DetailPeriodInformationBox } from './DetailPeriodInformationBox';

export interface IDetailPeriodInformationProps {
  nameSpace?: string;
}

/**
 * Sub-form displaying the original and renewal period information presented in styled boxes.
 * @param {IDetailPeriodInformationProps} param0
 */
export const DetailPeriodInformation: React.FunctionComponent<
  React.PropsWithChildren<IDetailPeriodInformationProps>
> = ({ nameSpace }) => {
  const { values } = useFormikContext<ApiGen_Concepts_Lease>();
  const startDate = getIn(values, withNameSpace(nameSpace, 'startDate'));
  const expiryDate = getIn(values, withNameSpace(nameSpace, 'expiryDate'));
  const periods = getIn(values, withNameSpace(nameSpace, 'periods'));
  const currentPeriod = periods.find(
    (period: ApiGen_Concepts_LeasePeriod) =>
      moment().isSameOrBefore(moment(period.expiryDate), 'day') ||
      (moment().isSameOrAfter(moment(period.startDate), 'day') && period.expiryDate === null),
  );
  const projectName = values?.project
    ? `${values?.project?.code} - ${values?.project?.description}`
    : '';

  return (
    <>
      <Section>
        <StyledDiv>
          <DetailPeriodInformationBox
            title="Lease / Licence"
            startDate={startDate}
            expiryDate={expiryDate}
          />
          <DetailPeriodInformationBox
            title="Current Period"
            startDate={currentPeriod?.startDate}
            expiryDate={currentPeriod?.expiryDate}
            inverted
          />
        </StyledDiv>
      </Section>
      <Section header="Project">
        <SectionField label="Ministry project">{projectName}</SectionField>
      </Section>
    </>
  );
};

export default DetailPeriodInformation;

const StyledDiv = styled.div`
  display: flex;
  justify-content: space-between;
  padding-left: 10rem;
  padding-right: 10rem;
`;

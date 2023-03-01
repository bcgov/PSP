import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { getIn, useFormikContext } from 'formik';
import { IFormLease } from 'interfaces';
import { ILeaseTerm } from 'interfaces/ILeaseTerm';
import moment from 'moment';
import * as React from 'react';
import styled from 'styled-components';
import { withNameSpace } from 'utils/formUtils';

import { DetailTermInformationBox } from './DetailTermInformationBox';

export interface IDetailTermInformationProps {
  nameSpace?: string;
}

/**
 * Sub-form displaying the original and renewal term information presented in styled boxes.
 * @param {IDetailTermInformationProps} param0
 */
export const DetailTermInformation: React.FunctionComponent<
  React.PropsWithChildren<IDetailTermInformationProps>
> = ({ nameSpace }) => {
  const { values } = useFormikContext<IFormLease>();
  const startDate = getIn(values, withNameSpace(nameSpace, 'startDate'));
  const expiryDate = getIn(values, withNameSpace(nameSpace, 'expiryDate'));
  const terms = getIn(values, withNameSpace(nameSpace, 'terms'));
  const currentTerm = terms.find((term: ILeaseTerm) =>
    moment().isSameOrBefore(moment(term.expiryDate), 'day'),
  );
  const projectName =
    values?.project !== undefined
      ? `${values?.project?.code} - ${values?.project?.description}`
      : '';

  return (
    <>
      <Section>
        <StyledDiv>
          <DetailTermInformationBox
            title="Lease / License"
            startDate={startDate}
            expiryDate={expiryDate}
          />
          <DetailTermInformationBox
            title="Current Term"
            startDate={currentTerm?.startDate}
            expiryDate={currentTerm?.expiryDate}
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

export default DetailTermInformation;

const StyledDiv = styled.div`
  display: flex;
  justify-content: space-between;
  padding-left: 10rem;
  padding-right: 10rem;
`;

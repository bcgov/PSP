import { FormSection } from 'components/common/form/styles';
import {
  DetailAdministration,
  DetailDescription,
  DetailNotes,
  DetailTermInformation,
  DetailTerms,
  PropertiesInformation,
} from 'features/leases';
import * as React from 'react';
import styled from 'styled-components';

export interface IDetailsProps {}

export const Details: React.FunctionComponent<React.PropsWithChildren<IDetailsProps>> = () => {
  return (
    <StyledDetails>
      <FormSectionOne>
        <DetailTermInformation />
        <DetailAdministration disabled={true} />
        <DetailDescription disabled={true} />
        <PropertiesInformation disabled={true} />
      </FormSectionOne>
      <FormSection>
        <DetailTerms disabled={true} />
      </FormSection>
      <FormSection>
        <DetailNotes disabled={true}></DetailNotes>
      </FormSection>
    </StyledDetails>
  );
};

export const FormSectionOne = styled(FormSection)`
  column-count: 2;
  & > * {
    break-inside: avoid-column;
  }
  column-gap: 10rem;
  li {
    list-style-type: none;
    padding: 2rem 0;
    margin: 0;
  }
  @media only screen and (max-width: 1500px) {
    column-count: 1;
  }
`;

const StyledDetails = styled.div`
  display: flex;
  flex-direction: column;
  width: 100%;
  gap: 2.5rem;
`;

export default Details;

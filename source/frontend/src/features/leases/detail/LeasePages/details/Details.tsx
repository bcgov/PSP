import { FormSection } from 'components/common/form/styles';
import {
  DetailAdministration,
  DetailTermInformation,
  PropertiesInformation,
} from 'features/leases';
import * as React from 'react';
import styled from 'styled-components';

import DetailDocumentation from './DetailDocumentation';

export interface IDetailsProps {}

export const Details: React.FunctionComponent<React.PropsWithChildren<IDetailsProps>> = () => {
  return (
    <StyledDetails>
      <DetailTermInformation />
      <PropertiesInformation disabled={true} />
      <DetailAdministration disabled={true} />
      <DetailDocumentation disabled={true} />
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
  margin-top: 4rem;
`;

export default Details;

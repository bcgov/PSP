import styled from 'styled-components';

import { Section } from '@/components/common/Section/Section';

export const StyledNoTabSection = styled(Section)`
  &.form-section {
    margin-left: 0;
    margin-right: 0;
    padding-left: 0;
    padding-right: 0;
  }
`;

export const StyledBorderSection = styled(Section)`
  border: 0.1rem solid lightgrey;
  border-radius: 0.5rem;
`;

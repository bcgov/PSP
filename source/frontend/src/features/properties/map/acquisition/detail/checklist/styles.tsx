import styled from 'styled-components';

import { Section } from '@/components/common/Section/Section';

export const StyledSectionCentered = styled(Section)`
  font-size: 1.4rem;
  text-align: center;
`;

export const StyledChecklistItemStatus = styled.span<{ color?: string }>`
  color: ${props => props.color ?? props.theme.css.textColor};
  min-width: 11rem;
`;

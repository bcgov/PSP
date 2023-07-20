import styled from 'styled-components';

import { FormDescriptionLabel } from '@/features/leases/detail/styles';

// common ui styling
export * from '@/features/leases/detail/styles';

export const ImprovementsContainer = styled.div`
  display: flex;
  flex-direction: column;
  width: 100%;
  gap: 2.5rem;
  .formgrid .textarea:not(.notes) {
    grid-column: span 2;
    border-left: 0;
  }
`;

export const ImprovementsListHeader = styled(FormDescriptionLabel)`
  color: ${props => props.theme.css.primaryColor};
  font-size: 1.8rem;
  margin-bottom: 2rem;
`;

import styled from 'styled-components';

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

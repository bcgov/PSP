import { Button } from 'components/common/buttons';
import styled from 'styled-components';

export const FileIcon = styled(Button)`
  &&.btn {
    background-color: #fff;
    color: ${({ theme }) => theme.css.primaryColor};
    padding: 6px 0.5rem;
  }
`;

export const EditIconButton = styled(FileIcon)`
  margin-right: 1.2rem;
`;

export const VerticalDivider = styled.div`
  border-left: 6px solid rgba(96, 96, 96, 0.2);
  height: 4rem;
  $margin-left: 1%;
  margin-right: 1%;
  border-width: 0.2rem;
`;

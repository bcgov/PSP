import variables from '_variables.module.scss';
import Button from 'react-bootstrap/Button';
import styled from 'styled-components';

export const FileIcon = styled(Button)`
  background-color: #fff !important;
  color: ${variables.primaryColor} !important;
  padding: 6px 5px;
`;

export const EditIconButton = styled(FileIcon)`
  margin-right: 12px;
`;

export const VerticalDivider = styled.div`
  border-left: 6px solid rgba(96, 96, 96, 0.2);
  height: 40px;
  margin-left: 1%;
  margin-right: 1%;
  border-width: 2px;
`;

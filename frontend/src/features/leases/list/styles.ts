import { Scrollable as ScrollableBase } from 'components/common/Scrollable/Scrollable';
import Button from 'react-bootstrap/Button';
import styled from 'styled-components';

export const ListPage = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  width: 100%;
  gap: 2.5rem;
  padding: 0;
`;

export const Scrollable = styled(ScrollableBase)`
  padding: 1.6rem 3.2rem;
  width: 100%;
`;

export const PageHeader = styled.h3`
  text-align: left;
`;

export const PageToolbar = styled.div`
  display: flex;
  flex-direction: row;
  align-items: center;
  padding: 0;
  padding-bottom: 2rem;
`;

export const Spacer = styled.div`
  display: flex;
  flex: 1 1 auto;
`;

export const FileIcon = styled(Button)`
  background-color: #fff !important;
  color: ${({ theme, disabled }) =>
    disabled ? theme.css.disabledColor : theme.css.primaryColor} !important;
  padding: 6px 0.5rem;
`;

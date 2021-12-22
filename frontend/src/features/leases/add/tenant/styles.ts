import { ContactListView } from 'features/contacts';
import styled from 'styled-components';

export const TenantH2 = styled.h2`
  font-size: 2rem;
  line-height: 2.8rem;
  text-align: left;
  color: ${props => props.theme.css.textColor};
`;

export const ContactListViewWrapper = styled(ContactListView)`
  & > div {
    padding: 0;
  }
  .thead .tr .th:last-of-type {
    display: none !important;
  }
  .tbody .tr .td:last-of-type {
    display: none !important;
  }
`;

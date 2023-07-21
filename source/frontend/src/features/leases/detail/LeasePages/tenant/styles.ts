import styled from 'styled-components';

import ContactManagerView from '@/components/contact/ContactManagerView/ContactManagerView';

export const TenantH2 = styled.h2`
  font-size: 2rem;
  font-family: BcSans-Bold;
  line-height: 2.8rem;
  text-align: left;
  color: ${props => props.theme.css.textColor};
`;

export const ContactListViewWrapper = styled(ContactManagerView)`
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

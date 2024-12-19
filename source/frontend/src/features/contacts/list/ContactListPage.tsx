import styled from 'styled-components';

import ContactIcon from '@/assets/images/contact-icon.svg?react';
import { Scrollable as ScrollableBase } from '@/components/common/Scrollable/Scrollable';
import * as CommonStyled from '@/components/common/styles';
import ContactManagerView from '@/components/contact/ContactManagerView/ContactManagerView';

/**
 * Page that displays a list of contacts.
 */
export const ContactListPage = () => {
  return (
    <StyledListPage>
      <StyledScrollable>
        <CommonStyled.H1>
          <ContactIcon title="Contact manager icon" fill="currentColor" />
          <span className="ml-2">Contacts</span>
        </CommonStyled.H1>
        <ContactManagerView showActiveSelector showAddButton />
      </StyledScrollable>
    </StyledListPage>
  );
};

const StyledListPage = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  width: 100%;
  font-size: 14px;
  gap: 2.5rem;
  padding: 0;
`;

const StyledScrollable = styled(ScrollableBase)`
  padding: 1.6rem 3.2rem;
  width: 100%;
`;

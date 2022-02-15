import { Scrollable as ScrollableBase } from 'components/common/Scrollable/Scrollable';
import ContactManagerView from 'components/contact/ContactManagerView/ContactManagerView';
import styled from 'styled-components';

/**
 * Page that displays a list of contacts.
 */
export const ContactListPage = () => {
  return (
    <StyledListPage>
      <StyledScrollable>
        <StyledPageHeader>Contacts</StyledPageHeader>
        <ContactManagerView showActiveSelector showAddButton />
      </StyledScrollable>
    </StyledListPage>
  );
};

const StyledListPage = styled.div`
  width: 100%;
  font-size: 14px;
`;

const StyledScrollable = styled(ScrollableBase)`
  padding: 1.6rem 3.2rem;
  width: 100%;
`;

const StyledPageHeader = styled.h3`
  text-align: left;
`;

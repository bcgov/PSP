import { Scrollable as ScrollableBase } from 'components/common/Scrollable/Scrollable';
import ContactManagerView from 'components/contact/ContactManagerView/ContactManagerView';
import styled from 'styled-components';

/**
 * Component that displays a list of leases within PSP as well as a filter bar to control the displayed leases.
 */
export const ContactListPage = () => {
  return (
    <ListPage>
      <Scrollable>
        <PageHeader>Contacts</PageHeader>
        <ContactManagerView showActiveSelector showAddButton />
      </Scrollable>
    </ListPage>
  );
};

export const ListPage = styled.div`
  width: 100%;
  font-size: 14px;
`;

export const Scrollable = styled(ScrollableBase)`
  padding: 1.6rem 3.2rem;
  width: 100%;
`;

export const PageHeader = styled.h3`
  text-align: left;
`;

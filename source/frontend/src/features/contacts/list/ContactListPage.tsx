import { FaPlus } from 'react-icons/fa';
import { useHistory } from 'react-router';
import styled from 'styled-components';

import ContactIcon from '@/assets/images/contact-icon.svg?react';
import * as CommonStyled from '@/components/common/styles';
import { PaddedScrollable, StyledAddButton } from '@/components/common/styles';
import ContactManagerView from '@/components/contact/ContactManagerView/ContactManagerView';
import Claims from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

/**
 * Page that displays a list of contacts.
 */
export const ContactListPage = () => {
  const history = useHistory();
  const { hasClaim } = useKeycloakWrapper();

  return (
    <CommonStyled.ListPage>
      <PaddedScrollable>
        <CommonStyled.H1>
          <FlexDiv>
            <div>
              <ContactIcon title="Contact manager icon" fill="currentColor" />
              <span className="ml-2">Contacts</span>
            </div>
            {hasClaim(Claims.CONTACT_ADD) && (
              <StyledAddButton onClick={() => history.push('/contact/new')}>
                <FaPlus />
                &nbsp;Add a New Contact
              </StyledAddButton>
            )}
          </FlexDiv>
        </CommonStyled.H1>
        <ContactManagerView showActiveSelector />
      </PaddedScrollable>
    </CommonStyled.ListPage>
  );
};

const FlexDiv = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.25rem;

  svg {
    vertical-align: baseline;
  }
`;

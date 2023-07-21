import React from 'react';
import { Col } from 'react-bootstrap';
import { MdEdit } from 'react-icons/md';
import { useHistory } from 'react-router-dom';
import styled from 'styled-components';

import { StyledIconButton } from '@/components/common/buttons';
import { ProtectedComponent } from '@/components/common/ProtectedComponent';
import { Claims } from '@/constants/claims';
import { useContactDetail } from '@/features/contacts/hooks/useContactDetail';

import { ContactBreadcrumb } from '../..';
import * as Styled from '../../styles';
import OrganizationView from './Organization';
import PersonView from './Person';

interface IContactViewContainerProps {
  match?: any;
}

const ContactViewContainer: React.FunctionComponent<
  React.PropsWithChildren<IContactViewContainerProps>
> = props => {
  const { contact } = useContactDetail(props?.match?.params?.id);
  const history = useHistory();
  return (
    <ContactLayout>
      <ContactBreadcrumb />
      <Styled.RowAligned>
        <Col>
          <Styled.H1>Contact</Styled.H1>
        </Col>
        <Col md="auto" className="ml-auto">
          <ProtectedComponent hideIfNotAuthorized claims={[Claims.CONTACT_EDIT]}>
            <StyledIconButton
              title="Edit Contact"
              variant="light"
              onClick={() => history.push(`/contact/${props?.match?.params?.id}/edit`)}
            >
              <MdEdit size={22} />
            </StyledIconButton>
          </ProtectedComponent>
        </Col>
      </Styled.RowAligned>

      {contact?.person && <PersonView person={contact?.person} />}
      {contact?.organization && <OrganizationView organization={contact?.organization} />}
    </ContactLayout>
  );
};

const ContactLayout = styled.div`
  height: 100%;
  width: 50%;
  min-width: 30rem;
  overflow: auto;
  padding: 1rem;
`;

export default ContactViewContainer;

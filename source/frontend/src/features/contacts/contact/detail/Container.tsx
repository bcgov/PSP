import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaWindowClose } from 'react-icons/fa';
import { MdContactMail } from 'react-icons/md';
import { useHistory } from 'react-router-dom';
import styled from 'styled-components';

import EditButton from '@/components/common/buttons/EditButton';
import { ProtectedComponent } from '@/components/common/ProtectedComponent';
import { H1 } from '@/components/common/styles';
import { Claims } from '@/constants/claims';
import { useContactDetail } from '@/features/contacts/hooks/useContactDetail';

import * as Styled from '../../styles';
import OrganizationView from './Organization';
import PersonFormView from './Person';

interface IContactViewContainerProps {
  match?: any;
}

const ContactViewContainer: React.FunctionComponent<
  React.PropsWithChildren<IContactViewContainerProps>
> = props => {
  const { contact } = useContactDetail(props?.match?.params?.id);
  const history = useHistory();
  return (
    <>
      <ContactLayout>
        <Styled.RowAligned>
          <Col>
            <H1>
              <Row>
                <Col>
                  <MdContactMail size={24} className="mr-2" />
                  Contact
                </Col>
                <Col md={1} className="d-flex justify-content-end">
                  <div className="btn">
                    <FaWindowClose
                      size={28}
                      onClick={() => {
                        history.push('/contact/list');
                      }}
                    />
                  </div>
                </Col>
              </Row>
            </H1>
          </Col>
        </Styled.RowAligned>
        <Styled.RowAligned>
          <Col md="auto" className="ml-auto">
            <ProtectedComponent hideIfNotAuthorized claims={[Claims.CONTACT_EDIT]}>
              <EditButton
                title="Edit Contact"
                onClick={() => history.push(`/contact/${props?.match?.params?.id}/edit`)}
              />
            </ProtectedComponent>
          </Col>
        </Styled.RowAligned>

        {contact?.person && <PersonFormView person={contact?.person} />}
        {contact?.organization && <OrganizationView organization={contact?.organization} />}
      </ContactLayout>
    </>
  );
};

const ContactLayout = styled.div`
  height: 100%;
  width: 50%;
  @media only screen and (max-width: 1199px) {
    width: 100%;
  }
  min-width: 30rem;
  overflow: auto;
  padding: 1rem;
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;

export default ContactViewContainer;

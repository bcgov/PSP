import { InlineFlexDiv } from 'components/common/styles';
import { UserNameTooltip } from 'components/common/UserNameTooltip';
import { LeaseHeaderAddresses } from 'features/leases/detail/LeaseHeaderAddresses';
import { HeaderField } from 'features/mapSideBar/tabs/HeaderField';
import { ILease } from 'interfaces';
import moment from 'moment';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { AiOutlineExclamationCircle } from 'react-icons/ai';
import styled from 'styled-components';
import { prettyFormatDate } from 'utils';

import { LeaseHeaderTenants } from './LeaseHeaderTenants';

export interface ILeaseHeaderProps {
  lease?: ILease;
}

const LeaseHeader: React.FC<ILeaseHeaderProps> = ({ lease }) => {
  const isExpired = moment().isAfter(moment(lease?.expiryDate, 'YYYY-MM-DD'), 'day');

  return (
    <Container>
      <Row className="no-gutters">
        <Col xs="7">
          <Row className="no-gutters">
            <Col>
              <HeaderField label="Lease/License #:" labelWidth="3" contentWidth="9">
                <StyledInlineDiv>
                  <span>{lease?.lFileNo ?? ''}</span>
                  <span>{lease?.paymentReceivableType?.description ?? ''}</span>
                </StyledInlineDiv>
              </HeaderField>
            </Col>
          </Row>
          <Row className="no-gutters">
            <Col>
              <HeaderField label="Property:" labelWidth="3" contentWidth="9">
                <LeaseHeaderAddresses lease={lease} />
              </HeaderField>
            </Col>
          </Row>
          <Row className="no-gutters">
            <Col>
              <HeaderField label="Tenant:" labelWidth="3" contentWidth="9">
                <LeaseHeaderTenants lease={lease} />
              </HeaderField>
            </Col>
          </Row>
        </Col>
        <Col xs="5">
          <Row className="no-gutters">
            <Col className="text-right">
              <StyledSmallText>
                Created: <strong>{prettyFormatDate(lease?.appCreateTimestamp)}</strong> by{' '}
                <UserNameTooltip
                  userName={lease?.appCreateUserid}
                  userGuid={lease?.appCreateUserGuid}
                />
              </StyledSmallText>
            </Col>
          </Row>
          <Row className="no-gutters">
            <Col className="text-right">
              <StyledSmallText>
                Last updated: <strong>{prettyFormatDate(lease?.appLastUpdateTimestamp)}</strong> by{' '}
                <UserNameTooltip
                  userName={lease?.appLastUpdateUserid}
                  userGuid={lease?.appLastUpdateUserGuid}
                />
              </StyledSmallText>
            </Col>
          </Row>
          <Row className="no-gutters">
            <Col>
              <HeaderField className="justify-content-end" label="Status:">
                {lease?.statusType?.description}
              </HeaderField>
            </Col>
          </Row>
        </Col>
      </Row>
      <Row>
        <Col>
          <HeaderField label="Start date:" labelWidth="3" contentWidth="9">
            {prettyFormatDate(lease?.startDate)}
          </HeaderField>
        </Col>
        <Col>
          <HeaderField label="Expiry date:" labelWidth="3" contentWidth="9">
            {prettyFormatDate(lease?.expiryDate)}
          </HeaderField>
        </Col>
        <Col>
          {isExpired && (
            <ExpiredWarning>
              <AiOutlineExclamationCircle size={16} />
              &nbsp; EXPIRED
            </ExpiredWarning>
          )}
        </Col>
      </Row>
    </Container>
  );
};

export default LeaseHeader;

const Container = styled.div`
  margin-top: 0.5rem;
  margin-bottom: 1.5rem;
  border-bottom-style: solid;
  border-bottom-color: grey;
  border-bottom-width: 0.1rem;
`;

const StyledSmallText = styled.span`
  font-size: 0.87em;
  line-height: 1.9;
`;

const StyledInlineDiv = styled(InlineFlexDiv)`
  justify-content: space-between;
  width: 100%;
`;

export const ExpiredWarning = styled(InlineFlexDiv)`
  color: ${props => props.theme.css.dangerColor};
  background-color: ${props => props.theme.css.dangerBackgroundColor};
  border-radius: 0.4rem;
  letter-spacing: 0.1rem;
  padding: 0.2rem;
  margin-right: 0.5rem;
  font-family: 'BCSans-Bold';
  font-size: 1.4rem;
  align-items: center;
`;

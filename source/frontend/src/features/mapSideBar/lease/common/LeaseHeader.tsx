import moment from 'moment';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { AiOutlineExclamationCircle } from 'react-icons/ai';
import styled from 'styled-components';

import {
  HeaderContentCol,
  HeaderField,
  HeaderLabelCol,
} from '@/components/common/HeaderField/HeaderField';
import { InlineFlexDiv } from '@/components/common/styles';
import { UserNameTooltip } from '@/components/common/UserNameTooltip';
import { LeaseHeaderAddresses } from '@/features/leases/detail/LeaseHeaderAddresses';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { prettyFormatDate, prettyFormatUTCDate } from '@/utils';

import { LeaseHeaderTenants } from './LeaseHeaderTenants';

export interface ILeaseHeaderProps {
  lease?: ApiGen_Concepts_Lease;
  lastUpdatedBy: Api_LastUpdatedBy | null;
}

export const LeaseHeader: React.FC<ILeaseHeaderProps> = ({ lease, lastUpdatedBy }) => {
  const isExpired = moment().isAfter(moment(lease?.expiryDate, 'YYYY-MM-DD'), 'day');

  return (
    <Container>
      <Row className="no-gutters">
        <Col xs="7">
          <Row className="no-gutters">
            <Col>
              <HeaderField label="Lease/License #" labelWidth="3" contentWidth="9">
                <StyledInlineDiv>
                  <span>{lease?.lFileNo ?? ''}</span>
                  <StyledGreenText>
                    {lease?.paymentReceivableType?.description ?? ''}
                  </StyledGreenText>
                </StyledInlineDiv>
              </HeaderField>
            </Col>
          </Row>
          <Row className="no-gutters">
            <Col>
              <HeaderField label="Property:" labelWidth="3" contentWidth="9">
                <LeaseHeaderAddresses
                  propertyLeases={lease?.fileProperties ?? []}
                  maxCollapsedLength={1}
                  delimiter={<br />}
                />
              </HeaderField>
            </Col>
          </Row>
          <Row className="no-gutters">
            <Col>
              <HeaderField label="Tenant:" labelWidth="3" contentWidth="9">
                <LeaseHeaderTenants
                  tenants={lease?.tenants ?? []}
                  maxCollapsedLength={1}
                  delimiter={<br />}
                />
              </HeaderField>
            </Col>
          </Row>
        </Col>
        <Col xs="5">
          <Row className="no-gutters">
            <Col className="text-right">
              <StyledSmallText>
                Created: <strong>{prettyFormatUTCDate(lease?.appCreateTimestamp)}</strong> by{' '}
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
                Last updated:{' '}
                <strong>{prettyFormatUTCDate(lastUpdatedBy?.appLastUpdateTimestamp)}</strong> by{' '}
                <UserNameTooltip
                  userName={lastUpdatedBy?.appLastUpdateUserid}
                  userGuid={lastUpdatedBy?.appLastUpdateUserGuid}
                />
              </StyledSmallText>
            </Col>
          </Row>
          <Row className="no-gutters">
            <Col>
              <HeaderField className="justify-content-end" label="Status:">
                {lease?.fileStatusTypeCode?.description}
              </HeaderField>
            </Col>
          </Row>
        </Col>
      </Row>
      <Row className="no-gutters">
        <Col xs="7">
          <Row className="no-gutters">
            <Col>
              <Row>
                <HeaderLabelCol label="Start date:" labelWidth="3" />
                <HeaderContentCol contentWidth="3">
                  {prettyFormatDate(lease?.startDate)}
                </HeaderContentCol>
                <HeaderLabelCol label="Expiry date:" />
                <HeaderContentCol>
                  <span className="pl-2">{prettyFormatDate(lease?.expiryDate)}</span>
                </HeaderContentCol>
              </Row>
            </Col>
          </Row>
        </Col>
        <Col xs="5">
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

const StyledGreenText = styled.span`
  color: ${props => props.theme.css.completedColor};
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
  width: fit-content;
`;

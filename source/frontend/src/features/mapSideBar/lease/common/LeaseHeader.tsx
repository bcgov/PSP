import moment from 'moment';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { AiOutlineExclamationCircle } from 'react-icons/ai';
import styled from 'styled-components';

import AuditSection from '@/components/common/HeaderField/AuditSection';
import {
  HeaderContentCol,
  HeaderField,
  HeaderLabelCol,
} from '@/components/common/HeaderField/HeaderField';
import StatusField from '@/components/common/HeaderField/StatusField';
import { StyledFiller } from '@/components/common/HeaderField/styles';
import { InlineFlexDiv } from '@/components/common/styles';
import { LeaseHeaderAddresses } from '@/features/leases/detail/LeaseHeaderAddresses';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { exists, prettyFormatDate } from '@/utils';

import HistoricalNumbersContainer from '../../shared/header/HistoricalNumberContainer';
import { HistoricalNumberSectionView } from '../../shared/header/HistoricalNumberSectionView';
import { LeaseHeaderTenants } from './LeaseHeaderTenants';

export interface ILeaseHeaderProps {
  lease?: ApiGen_Concepts_Lease;
  lastUpdatedBy: Api_LastUpdatedBy | null;
}

export const LeaseHeader: React.FC<ILeaseHeaderProps> = ({ lease, lastUpdatedBy }) => {
  const isExpired = moment().isAfter(moment(lease?.expiryDate, 'YYYY-MM-DD'), 'day');

  const propertyIds = lease?.fileProperties?.map(fp => fp.propertyId) ?? [];

  return (
    <Container>
      <Row className="no-gutters">
        <Col xs="8">
          <HeaderField label="Lease/Licence #" labelWidth="3" contentWidth="9">
            <span className="pr-4">{lease?.lFileNo ?? ''}</span>
            <StyledGreenText>{lease?.paymentReceivableType?.description ?? ''}</StyledGreenText>
          </HeaderField>
          <HeaderField label="Property:" labelWidth="3" contentWidth="9">
            <LeaseHeaderAddresses
              propertyLeases={lease?.fileProperties ?? []}
              maxCollapsedLength={1}
              delimiter={<br />}
            />
          </HeaderField>
          <HeaderField label="Tenant:" labelWidth="3" contentWidth="9">
            <LeaseHeaderTenants
              tenants={lease?.tenants ?? []}
              maxCollapsedLength={1}
              delimiter={<br />}
            />
          </HeaderField>
          <Row>
            <HeaderLabelCol label="Lease Start:" labelWidth="3" />
            <HeaderContentCol contentWidth="3">
              {prettyFormatDate(lease?.startDate)}
            </HeaderContentCol>
            <HeaderLabelCol label="Expiry:" />
            <HeaderContentCol>
              <span className="pl-2">{prettyFormatDate(lease?.expiryDate)}</span>
            </HeaderContentCol>
            <HeaderContentCol>
              {isExpired && (
                <ExpiredWarning className="ml-auto">
                  <AiOutlineExclamationCircle size={16} />
                  &nbsp; EXPIRED
                </ExpiredWarning>
              )}
            </HeaderContentCol>
          </Row>
          <HistoricalNumbersContainer
            propertyIds={propertyIds}
            View={HistoricalNumberSectionView}
          />
        </Col>

        <Col>
          <StyledFiller>
            <AuditSection lastUpdatedBy={lastUpdatedBy} baseAudit={lease} />
            {exists(lease?.fileStatusTypeCode) && (
              <StatusField preText="File:" statusCodeType={lease.fileStatusTypeCode} />
            )}
          </StyledFiller>
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

const StyledGreenText = styled.span`
  font-weight: bold;
  color: ${props => props.theme.bcTokens.iconsColorSuccess};
`;

export const ExpiredWarning = styled(InlineFlexDiv)`
  color: ${props => props.theme.bcTokens.surfaceColorPrimaryDangerButtonDefault};
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

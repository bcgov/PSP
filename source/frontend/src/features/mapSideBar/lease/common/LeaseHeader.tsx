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
import { getCalculatedExpiry } from '@/features/leases/leaseUtils';
import { Api_LastUpdatedBy } from '@/models/api/File';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { exists, prettyFormatDate } from '@/utils';

import HistoricalNumbersContainer from '../../shared/header/HistoricalNumberContainer';
import { HistoricalNumberSectionView } from '../../shared/header/HistoricalNumberSectionView';
import { LeaseHeaderStakeholders } from './LeaseHeaderTenants';

export interface ILeaseHeaderProps {
  lease?: ApiGen_Concepts_Lease;
  lastUpdatedBy: Api_LastUpdatedBy | null;
}

export const LeaseHeader: React.FC<ILeaseHeaderProps> = ({ lease, lastUpdatedBy }) => {
  const propertyIds = lease?.fileProperties?.map(fp => fp.propertyId) ?? [];

  const calculatedExpiry = exists(lease) ? getCalculatedExpiry(lease, lease.renewals || []) : '';

  const isExpired = moment().isAfter(moment(calculatedExpiry, 'YYYY-MM-DD'), 'day');

  return (
    <Container>
      <Row className="no-gutters">
        <Col xs="8">
          <HeaderField label="Lease/Licence #" labelWidth="4" contentWidth="8">
            <span className="pr-4">{lease?.lFileNo ?? ''}</span>
            <StyledGreenText>{lease?.paymentReceivableType?.description ?? ''}</StyledGreenText>
          </HeaderField>
          <HeaderField label="Property:" labelWidth="4" contentWidth="8">
            <LeaseHeaderAddresses
              propertyLeases={lease?.fileProperties ?? []}
              maxCollapsedLength={1}
              delimiter={<br />}
            />
          </HeaderField>
          <HeaderField label="Tenant:" labelWidth="4" contentWidth="8">
            <LeaseHeaderStakeholders
              stakeholders={lease?.stakeholders ?? []}
              maxCollapsedLength={1}
              delimiter={<br />}
            />
          </HeaderField>
          <Row className="flex-nowrap">
            <HeaderLabelCol
              label="Commencement:"
              labelWidth="4"
              tooltip="The start date defined in the agreement"
            />
            <HeaderContentCol contentWidth="3">
              {prettyFormatDate(lease?.startDate)}
            </HeaderContentCol>
            <HeaderLabelCol label="Expiry:" tooltip="The end date specified in the agreement" />
            <HeaderContentCol>
              <span className="pl-2">{prettyFormatDate(calculatedExpiry)}</span>
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
            labelWidth="4"
            contentWidth="8"
            View={HistoricalNumberSectionView}
          />
        </Col>

        <Col>
          <StyledFiller>
            <AuditSection lastUpdatedBy={lastUpdatedBy} baseAudit={lease} />
            {exists(lease?.fileStatusTypeCode) && (
              <StatusField
                preText="File:"
                statusCodeType={lease.fileStatusTypeCode}
                statusCodeDate={
                  lease?.fileStatusTypeCode?.id === ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED
                    ? lease?.terminationDate
                    : undefined
                }
              />
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

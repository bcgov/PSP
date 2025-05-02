import React, { useMemo } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaExternalLinkAlt, FaFileContract } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import styled from 'styled-components';

import EditButton from '@/components/common/buttons/EditButton';
import { HeaderField } from '@/components/common/HeaderField/HeaderField';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { StyledAddButton } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import { Claims, Roles } from '@/constants';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_CompReqAcqPayee } from '@/models/api/generated/ApiGen_Concepts_CompReqAcqPayee';
import { ApiGen_Concepts_CompReqLeasePayee } from '@/models/api/generated/ApiGen_Concepts_CompReqLeasePayee';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_Product } from '@/models/api/generated/ApiGen_Concepts_Product';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import {
  exists,
  formatMoney,
  getFilePropertyName,
  isValidId,
  isValidString,
  prettyFormatDate,
} from '@/utils';
import { formatMinistryProject } from '@/utils/formUtils';

import { cannotEditMessage } from '../../acquisition/common/constants';
import { PayeeDetail } from './PayeeDetail';

export interface CompensationRequisitionDetailViewProps {
  fileType: ApiGen_CodeTypes_FileTypes;
  fileProduct: ApiGen_Concepts_Product | undefined;
  fileProject: ApiGen_Concepts_Project | undefined;
  compensation: ApiGen_Concepts_CompensationRequisition;
  compensationProperties: ApiGen_Concepts_FileProperty[];
  compensationAcqPayees: ApiGen_Concepts_CompReqAcqPayee[];
  compensationLeasePayees: ApiGen_Concepts_CompReqLeasePayee[];
  clientConstant: string;
  loading: boolean;
  isFileFinalStatus?: boolean;
  setEditMode: (editMode: boolean) => void;
  onGenerate: (
    fileType: ApiGen_CodeTypes_FileTypes,
    compensation: ApiGen_Concepts_CompensationRequisition,
  ) => void;
}

export const CompensationRequisitionDetailView: React.FunctionComponent<
  CompensationRequisitionDetailViewProps
> = ({
  fileType,
  fileProduct,
  fileProject,
  compensation,
  compensationProperties,
  compensationAcqPayees,
  compensationLeasePayees,
  clientConstant,
  loading,
  isFileFinalStatus,
  setEditMode,
  onGenerate,
}) => {
  const { hasClaim, hasRole } = useKeycloakWrapper();

  const alternateProjectName = exists(compensation?.alternateProject)
    ? formatMinistryProject(
        compensation?.alternateProject?.code,
        compensation?.alternateProject?.description,
      )
    : '';

  const results =
    compensation.financials?.filter(el => {
      return el.isGstRequired === true;
    }) || [];

  const isGstApplicable = results.length > 0;

  const payeeDetails = useMemo<PayeeDetail[]>(() => {
    const tempPayeeDetails: PayeeDetail[] = [];
    if (!compensation) {
      return;
    }

    if (compensationAcqPayees?.length > 0 && fileType === ApiGen_CodeTypes_FileTypes.Acquisition) {
      compensationAcqPayees.forEach((currentPayee: ApiGen_Concepts_CompReqAcqPayee) => {
        let currentPayeeDetail = new PayeeDetail();
        if (currentPayee.acquisitionOwner) {
          currentPayeeDetail = PayeeDetail.createFromOwner(currentPayee.acquisitionOwner);
        } else if (isValidId(currentPayee.interestHolderId)) {
          if (exists(currentPayee?.interestHolder?.personId)) {
            currentPayeeDetail = PayeeDetail.createFromPerson(currentPayee.interestHolder.person);
          } else if (currentPayee?.interestHolder?.organizationId) {
            currentPayeeDetail = PayeeDetail.createFromOrganization(
              currentPayee.interestHolder.organization,
            );
          }
        } else if (isValidId(currentPayee?.acquisitionFileTeamId)) {
          if (isValidId(currentPayee?.acquisitionFileTeam?.personId)) {
            currentPayeeDetail = PayeeDetail.createFromPerson(
              currentPayee.acquisitionFileTeam.person,
            );
          } else if (isValidId(currentPayee?.acquisitionFileTeam?.organizationId)) {
            currentPayeeDetail = PayeeDetail.createFromOrganization(
              currentPayee.acquisitionFileTeam.organization,
            );
          }
        } else if (isValidString(currentPayee?.legacyPayee)) {
          currentPayeeDetail = PayeeDetail.createFromLegacyPayee(currentPayee.legacyPayee);
        }
        currentPayeeDetail.compReqPayeeId = currentPayee.compReqAcqPayeeId;
        currentPayeeDetail.isPaymentInTrust = compensation.isPaymentInTrust;
        tempPayeeDetails.push(currentPayeeDetail);
      });
    } else if (
      compensationLeasePayees?.length > 0 &&
      fileType === ApiGen_CodeTypes_FileTypes.Lease
    ) {
      compensationLeasePayees.forEach((leasePayee: ApiGen_Concepts_CompReqLeasePayee) => {
        let payeeDetail = new PayeeDetail();

        if (isValidId(leasePayee?.leaseStakeholder?.personId)) {
          payeeDetail = PayeeDetail.createFromPerson(leasePayee?.leaseStakeholder?.person);
        } else if (isValidId(leasePayee?.leaseStakeholder?.organizationId)) {
          payeeDetail = PayeeDetail.createFromOrganization(
            leasePayee?.leaseStakeholder?.organization,
          );
        }
        payeeDetail.isPaymentInTrust = compensation.isPaymentInTrust;
        payeeDetail.compReqPayeeId = leasePayee.compReqLeasePayeeId;
        tempPayeeDetails.push(payeeDetail);
      });
    }

    return tempPayeeDetails;
  }, [compensation, compensationAcqPayees, compensationLeasePayees, fileType]);

  const compPretaxAmount = compensation?.financials
    ?.map(f => f.pretaxAmount ?? 0)
    .reduce((prev, next) => prev + next, 0);

  const compTaxAmount = compensation?.financials
    ?.map(f => f.taxAmount ?? 0)
    .reduce((prev, next) => prev + next, 0);

  const compTotalAmount = compensation?.financials
    ?.map(f => f.totalAmount ?? 0)
    .reduce((prev, next) => prev + next, 0);

  const userCanEditCompensationReq = (): boolean => {
    if (isFileFinalStatus) {
      return false;
    } else if (
      (compensation.isDraft && hasClaim(Claims.COMPENSATION_REQUISITION_EDIT)) ||
      hasRole(Roles.SYSTEM_ADMINISTRATOR)
    ) {
      return true;
    }

    return false;
  };

  const editButtonBlock = (
    <EditButton
      title="Edit compensation requisition"
      onClick={() => {
        setEditMode(true);
      }}
      style={{ float: 'right' }}
    />
  );

  return (
    <StyledSummarySection>
      <LoadingBackdrop show={loading} parentScreen={true} />
      <Section>
        <StyledRow className="no-gutters">
          <Col xl="6" xs="12">
            <HeaderField label="Client:" labelWidth={{ xs: 8 }} valueTestId="compensation-client">
              {clientConstant}
            </HeaderField>
            <HeaderField
              label="Requisition number:"
              labelWidth={{ xs: 8 }}
              valueTestId="compensation-number"
            >
              {compensation.isDraft ? 'Draft' : compensation.id}
            </HeaderField>
          </Col>
          <Col xl="6" xs="12">
            <HeaderField
              label="Compensation amount:"
              labelWidth={{ xs: 8 }}
              contentWidth={{ xs: 4 }}
              valueTestId="header-pretax-amount"
            >
              <p className="mb-0 text-right">{formatMoney(compPretaxAmount ?? 0)}</p>
            </HeaderField>
            <HeaderField
              label="Applicable GST:"
              labelWidth={{ xs: 8 }}
              contentWidth={{ xs: 4 }}
              valueTestId="header-tax-amount"
            >
              <p className="mb-0 text-right">{formatMoney(compTaxAmount ?? 0)}</p>
            </HeaderField>
            <HeaderField
              label="Total cheque amount:"
              labelWidth={{ xs: 8 }}
              contentWidth={{ xs: 4 }}
              valueTestId="header-total-amount"
            >
              <p className="mb-0 text-right">{formatMoney(compTotalAmount ?? 0)}</p>
            </HeaderField>
          </Col>
        </StyledRow>
      </Section>

      <Section
        header={
          <FlexDiv>
            Requisition Details
            <RightFlexDiv>
              {setEditMode !== undefined && userCanEditCompensationReq() && editButtonBlock}
              {!userCanEditCompensationReq() && (
                <TooltipIcon
                  toolTipId={`${compensation?.id || 0}-compensation-cannot-edit-tooltip`}
                  toolTip={cannotEditMessage}
                />
              )}
              <StyledAddButton
                title="Download File"
                onClick={() => {
                  onGenerate(fileType, compensation);
                }}
              >
                <FaFileContract size={28} className="mr-2" />
                Generate H-120
              </StyledAddButton>
            </RightFlexDiv>
          </FlexDiv>
        }
      >
        <SectionField label="Status" labelWidth={{ xl: '4' }} valueTestId="compensation-status">
          {compensation.isDraft ? 'Draft' : 'Final'}
        </SectionField>
        <SectionField label="Alternate project" labelWidth={{ xl: '4' }}>
          {alternateProjectName}
        </SectionField>
        <SectionField
          label="Final date"
          labelWidth={{ xl: '4' }}
          valueTestId="compensation-finalized-date"
        >
          {prettyFormatDate(compensation.finalizedDate)}
        </SectionField>
        <SectionField label="Agreement date" labelWidth={{ xl: '4' }}>
          {prettyFormatDate(compensation.agreementDate)}
        </SectionField>
        <SectionField
          label="Special instructions"
          labelWidth={{ xl: '12' }}
          valueClassName="pre-wrap"
        >
          <p style={{ whiteSpace: 'pre-wrap' }}>{compensation.specialInstruction}</p>
        </SectionField>
      </Section>

      <Section
        header={
          <div className="d-flex align-items-center">
            <span>Selected File Properties</span>
            <TooltipIcon
              toolTipId="selected-properties"
              innerClassName="ml-4 mb-1"
              toolTip="Select the properties that will be displayed on the generated document"
            />
          </div>
        }
      >
        <SectionField label="Properties" labelWidth={{ xl: '4' }}>
          {compensationProperties.map(x => {
            const propertyName = getFilePropertyName(x);
            return (
              <p key={x.id}>
                <label>{propertyName.value}</label>
              </p>
            );
          })}
        </SectionField>
      </Section>

      <Section header="Financial Coding" isCollapsable initiallyExpanded>
        <SectionField label="Product" labelWidth={{ xl: '4' }} valueTestId="file-product">
          {fileProduct?.code ?? ''}
        </SectionField>
        <SectionField label="Business function" labelWidth={{ xl: '4' }}>
          {fileProject?.businessFunctionCode?.code ?? ''}
        </SectionField>
        <SectionField label="Work activity" labelWidth={{ xl: '4' }}>
          {fileProject?.workActivityCode?.code ?? ''}
        </SectionField>
        <SectionField label="Cost type" labelWidth={{ xl: '4' }}>
          {fileProject?.costTypeCode?.code ?? ''}
        </SectionField>
        <SectionField label="Fiscal year" labelWidth={{ xl: '4' }}>
          {compensation.fiscalYear ?? ''}
        </SectionField>
        <SectionField label="STOB" labelWidth={{ xl: '4' }}>
          {compensation.yearlyFinancial && (
            <label>
              {compensation.yearlyFinancial?.code} - {compensation.yearlyFinancial?.description}
            </label>
          )}
        </SectionField>
        <SectionField label="Service line" labelWidth={{ xl: '4' }}>
          {compensation.chartOfAccounts && (
            <label>
              {compensation.chartOfAccounts?.code} - {compensation.chartOfAccounts?.description}
            </label>
          )}
        </SectionField>
        <SectionField label="Responsibility centre" labelWidth={{ xl: '4' }}>
          {compensation.responsibility && (
            <label>
              {compensation.responsibility?.code} - {compensation.responsibility?.description}
            </label>
          )}
        </SectionField>
      </Section>

      <Section header="Payment" isCollapsable initiallyExpanded>
        <SectionField label="Payee(s)" labelWidth={{ xl: '4' }} valueTestId="comp-req-payees">
          {payeeDetails.map(payeeDetail => (
            <StyledPayeeDisplayName key={`compensations-payee-${payeeDetail.compReqPayeeId}`}>
              {payeeDetail?.contactEnabled && payeeDetail?.contactString && (
                <StyledLink
                  target="_blank"
                  rel="noopener noreferrer"
                  to={`/contact/${payeeDetail.contactString}`}
                >
                  <span>{payeeDetail.displayName}</span>
                  <FaExternalLinkAlt className="ml-2" size="1rem" />
                </StyledLink>
              )}

              {!payeeDetail?.contactEnabled && <label>{payeeDetail?.displayName ?? ''}</label>}
              {payeeDetail?.isPaymentInTrust && <label>, in trust</label>}
            </StyledPayeeDisplayName>
          ))}
        </SectionField>
        <SectionField label="Amount (before tax)">
          {formatMoney(compPretaxAmount ?? 0)}
        </SectionField>
        <SectionField label="GST applicable?">{isGstApplicable ? 'Yes' : 'No'}</SectionField>
        {isGstApplicable && (
          <>
            <SectionField label="GST amount">{formatMoney(compTaxAmount ?? 0)}</SectionField>
            <SectionField label="GST number">{compensation.gstNumber}</SectionField>
          </>
        )}
        <SectionField label="Total amount">{formatMoney(compTotalAmount ?? 0)}</SectionField>
      </Section>

      <Section header="Financial Activities" isCollapsable initiallyExpanded>
        {compensation.financials?.map((item, index) => (
          <React.Fragment
            key={`${item.financialActivityCode?.code}-${item.financialActivityCode?.description}`}
          >
            <StyledSubHeader>
              <label>Activity {index + 1}</label>
            </StyledSubHeader>
            <SectionField label="Code & Description" labelWidth={{ xl: '4' }}>
              {item.financialActivityCode?.code} - {item.financialActivityCode?.description}
            </SectionField>
            <SectionField label="Amount (before tax)" labelWidth={{ xl: '4' }}>
              {formatMoney(item.pretaxAmount ?? 0)}
            </SectionField>
            <SectionField label="GST applicable?" labelWidth={{ xl: '4' }}>
              {item.isGstRequired ? 'Yes' : 'No'}
            </SectionField>
            <SectionField label="GST amount" labelWidth={{ xl: '4' }}>
              {formatMoney(item.taxAmount ?? 0)}
            </SectionField>
            <SectionField label="Total amount" labelWidth={{ xl: '4' }}>
              {formatMoney(item.totalAmount ?? 0)}
            </SectionField>
          </React.Fragment>
        ))}
      </Section>

      <Section>
        <SectionField label="Detailed remarks" labelWidth={{ xl: '12' }}>
          <p style={{ whiteSpace: 'pre-wrap' }}>{compensation.detailedRemarks}</p>
        </SectionField>
      </Section>

      <Section>
        <StyledCompensationFooter>
          <Row>
            <Col className="pr-0 text-right">
              <label>Compensation amount:</label>
            </Col>
            <Col xs="3" className="pl-1 text-right">
              <span>{formatMoney(compPretaxAmount ?? 0)}</span>
            </Col>
          </Row>
          <Row>
            <Col className="pr-0 text-right">
              <label>Applicable GST:</label>
            </Col>
            <Col xs="3" className="pl-1 text-right">
              <span>{formatMoney(compTaxAmount ?? 0)}</span>
            </Col>
          </Row>
          <Row>
            <Col className="pr-0 text-right">
              <label>Total cheque amount:</label>
            </Col>
            <Col xs="3" className="pl-1 text-right">
              <span>{formatMoney(compTotalAmount ?? 0)}</span>
            </Col>
          </Row>
        </StyledCompensationFooter>
      </Section>
    </StyledSummarySection>
  );
};

export default CompensationRequisitionDetailView;

const StyledRow = styled(Row)`
  margin-top: 0.5rem;
  margin-bottom: 0.5rem;
  font-weight: bold;
`;

const StyledLink = styled(Link)`
  display: flex;
  align-items: center;
`;

const FlexDiv = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.25rem;
`;

const RightFlexDiv = styled.div`
  display: flex;
  flex-direction: row-reverse;
`;

const StyledCompensationFooter = styled.div`
  font-size: 16px;
  font-weight: 600;
`;

const StyledPayeeDisplayName = styled.div`
  display: flex;
  flex-direction: row;
  flex-grow: 1;
  text-align: left;
  overflow-y: auto;
  padding-bottom: 1rem;
`;

const StyledSubHeader = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  border-bottom: solid 0.1rem ${props => props.theme.css.headerBorderColor};
  margin-bottom: 2rem;

  label {
    color: ${props => props.theme.css.headerTextColor};
    font-family: 'BCSans-Bold';
    font-size: 1.75rem;
    width: 100%;
    text-align: left;
  }

  button {
    margin-bottom: 1rem;
  }
`;

import { Col, Row } from 'react-bootstrap';
import { FaExternalLinkAlt, FaMoneyCheck } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import styled from 'styled-components';

import EditButton from '@/components/common/EditButton';
import { HeaderField } from '@/components/common/HeaderField/HeaderField';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { StyledAddButton } from '@/components/common/styles';
import Claims from '@/constants/claims';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { Api_CompensationPayee } from '@/models/api/CompensationPayee';
import { Api_CompensationRequisition } from '@/models/api/CompensationRequisition';
import { Api_Product, Api_Project } from '@/models/api/Project';
import { formatMoney, prettyFormatDate } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import { DetailAcquisitionFileOwner } from '../../acquisition/detail/models';

export interface CompensationRequisitionDetailViewProps {
  compensation: Api_CompensationRequisition;
  compensationPayee?: Api_CompensationPayee;
  acqFileProject?: Api_Project;
  acqFileProduct?: Api_Product | undefined;
  clientConstant: string;
  gstConstant: number | undefined;
  loading: boolean;
  setEditMode: (editMode: boolean) => void;
  onGenerate: (compensation: Api_CompensationRequisition) => void;
}

interface PayeeViewDetails {
  displayName: string;
  isGstApplicable: boolean;
  gstNumber: string;
  preTaxAmount: number;
  taxAmount: number;
  totalAmount: number;
  goodTrust: boolean;
  contactEnabled: boolean;
  personId: number | null;
}

export const CompensationRequisitionDetailView: React.FunctionComponent<
  React.PropsWithChildren<CompensationRequisitionDetailViewProps>
> = ({
  compensation,
  compensationPayee,
  acqFileProject,
  acqFileProduct,
  clientConstant,
  loading,
  setEditMode,
  onGenerate,
}) => {
  const { hasClaim } = useKeycloakWrapper();
  const getPayeeDetails = (
    compensationPayee: Api_CompensationPayee | null | undefined,
  ): PayeeViewDetails | null => {
    if (!compensationPayee) {
      return null;
    }

    let payeeDetail = {
      contactEnabled: true,
      goodTrust: compensationPayee?.isPaymentInTrust,
      personId: null,
    } as PayeeViewDetails;

    if (compensationPayee.acquisitionOwnerId) {
      const ownerDetail = DetailAcquisitionFileOwner.fromApi(compensationPayee.acquisitionOwner!);
      payeeDetail.displayName = ownerDetail.ownerName ?? '';
      payeeDetail.contactEnabled = false;
    } else if (compensationPayee.interestHolderId) {
      payeeDetail.displayName = formatApiPersonNames(compensationPayee.interestHolder?.person);
      payeeDetail.personId = compensationPayee.interestHolder?.person?.id!;
    } else if (compensationPayee.ownerRepresentativeId) {
      payeeDetail.displayName = formatApiPersonNames(compensationPayee.ownerRepresentative?.person);
      payeeDetail.personId = compensationPayee.ownerRepresentative?.person?.id!;
    } else if (compensationPayee.ownerSolicitorId) {
      payeeDetail.displayName = formatApiPersonNames(compensationPayee.ownerSolicitor?.person);
      payeeDetail.personId = compensationPayee.ownerSolicitor?.person?.id!;
    } else if (compensationPayee.motiSolicitorId) {
      payeeDetail.displayName = formatApiPersonNames(compensationPayee.motiSolicitor);
      payeeDetail.personId = compensationPayee.motiSolicitor?.id!;
    }

    const payeePretaxAmount = compensation?.financials
      .map(f => f.pretaxAmount ?? 0)
      .reduce((prev, next) => prev + next, 0);

    const payeeTaxAmount = compensation?.financials
      .map(f => f.taxAmount ?? 0)
      .reduce((prev, next) => prev + next, 0);

    const payeeTotalAmount = compensation?.financials
      .map(f => f.totalAmount ?? 0)
      .reduce((prev, next) => prev + next, 0);

    payeeDetail.preTaxAmount = payeePretaxAmount;
    payeeDetail.taxAmount = payeeTaxAmount;
    payeeDetail.totalAmount = payeeTotalAmount;

    var results =
      compensation.financials?.filter(el => {
        return el.isGstRequired === true;
      }) || [];

    payeeDetail.isGstApplicable = results.length > 0;

    return payeeDetail;
  };

  const payeeDetails = getPayeeDetails(compensationPayee);

  return (
    <StyledSummarySection>
      <LoadingBackdrop show={loading} parentScreen={true} />
      <RightFlexDiv>
        {setEditMode !== undefined && hasClaim(Claims.COMPENSATION_REQUISITION_EDIT) && (
          <EditButton
            title="Edit compensation requisition"
            onClick={() => {
              setEditMode(true);
            }}
          />
        )}
        <StyledAddButton
          onClick={() => {
            onGenerate(compensation);
          }}
        >
          <FaMoneyCheck className="mr-2" />
          Generate H120
        </StyledAddButton>
      </RightFlexDiv>
      <Section>
        <StyledRow className="no-gutters">
          <Col xs="6">
            <HeaderField label="Client:" labelWidth="8" valueTestId={'compensation-client'}>
              {clientConstant}
            </HeaderField>
            <HeaderField label="Requisition number:" labelWidth="8">
              {compensation.isDraft ? 'Draft' : compensation.id}
            </HeaderField>
          </Col>
          <Col xs="6">
            <HeaderField label="Compensation amount:" labelWidth="8">
              {formatMoney(payeeDetails?.preTaxAmount ?? 0)}
            </HeaderField>
            <HeaderField label="Applicable GST:" labelWidth="8">
              {formatMoney(payeeDetails?.taxAmount ?? 0)}
            </HeaderField>
            <HeaderField label="Total cheque amount:" labelWidth="8">
              {formatMoney(payeeDetails?.totalAmount ?? 0)}
            </HeaderField>
          </Col>
        </StyledRow>
      </Section>

      <Section header="Requisition Details">
        <SectionField label="Status" labelWidth="4">
          {compensation.isDraft ? 'Draft' : 'Final'}
        </SectionField>
        <SectionField label="Agreement date" labelWidth="4">
          {prettyFormatDate(compensation.agreementDate)}
        </SectionField>
        <SectionField label="Expropriation notice server date" labelWidth="4">
          {prettyFormatDate(compensation.expropriationNoticeServedDate)}
        </SectionField>
        <SectionField label="Expropriation vesting date" labelWidth="4">
          {prettyFormatDate(compensation.expropriationVestingDate)}
        </SectionField>
        <SectionField label="Special instructions" labelWidth={'12'}>
          {compensation.specialInstruction}
        </SectionField>
      </Section>

      <Section header="Financial Coding" isCollapsable initiallyExpanded>
        <SectionField label="Product" labelWidth="4">
          {acqFileProduct?.code ?? ''}
        </SectionField>
        <SectionField label="Business function" labelWidth="4">
          {acqFileProject?.code ?? ''}
        </SectionField>
        <SectionField label="Work activity" labelWidth="4">
          {acqFileProject?.workActivityCode?.code ?? ''}
        </SectionField>
        <SectionField label="Cost type" labelWidth="4">
          {acqFileProject?.costTypeCode?.code ?? ''}
        </SectionField>
        <SectionField label="Fiscal year" labelWidth="4">
          {compensation.fiscalYear ?? ''}
        </SectionField>
        <SectionField label="STOB" labelWidth="4">
          {compensation.yearlyFinancial?.code ?? ''}
        </SectionField>
        <SectionField label="Service line" labelWidth="4">
          {compensation.chartOfAccounts && (
            <label>
              {compensation.chartOfAccounts?.code} - {compensation.chartOfAccounts?.description}
            </label>
          )}
        </SectionField>
        <SectionField label="Responsiblity centre" labelWidth="4">
          {compensation.responsibility && (
            <label>
              {compensation.responsibility?.code} - {compensation.responsibility?.description}
            </label>
          )}
        </SectionField>
      </Section>

      <Section header="Payment" isCollapsable initiallyExpanded>
        <SectionField label="Payee" labelWidth="4">
          <StyledPayeeDisplayName>
            {payeeDetails?.contactEnabled && payeeDetails?.personId && (
              <StyledLink
                target="_blank"
                rel="noopener noreferrer"
                to={`/contact/P${payeeDetails.personId}`}
              >
                <span>{payeeDetails.displayName}</span>
                <FaExternalLinkAlt className="ml-2" size="1rem" />
              </StyledLink>
            )}
            {!payeeDetails?.contactEnabled && <label>{payeeDetails?.displayName ?? ''}</label>}
            {payeeDetails?.goodTrust && <label>, in trust</label>}
          </StyledPayeeDisplayName>
        </SectionField>
        <SectionField label="Amount (before tax)">
          {formatMoney(payeeDetails?.preTaxAmount ?? 0)}
        </SectionField>
        <SectionField label="GST applicable?">
          {payeeDetails?.isGstApplicable ? 'Yes' : 'No'}
        </SectionField>
        {payeeDetails?.isGstApplicable && (
          <SectionField label="GST amount">
            {formatMoney(payeeDetails?.taxAmount ?? 0)}
          </SectionField>
        )}
        <SectionField label="Total amount">
          {formatMoney(payeeDetails?.totalAmount ?? 0)}
        </SectionField>
      </Section>

      <Section header="Financial Activities" isCollapsable initiallyExpanded>
        {compensation.financials.map((item, index) => (
          <>
            <StyledSubHeader>
              <label>Activity {index + 1}</label>
            </StyledSubHeader>
            <SectionField label="Code & Description" labelWidth="4">
              {item.financialActivityCode?.id} - {item.financialActivityCode?.description}
            </SectionField>
            <SectionField label="Amount (before tax)" labelWidth="4">
              {formatMoney(item.pretaxAmount ?? 0)}
            </SectionField>
            <SectionField label="GST applicable?" labelWidth="4">
              {item.isGstRequired ? 'Yes' : 'No'}
            </SectionField>
            <SectionField label="GST amount" labelWidth="4">
              {formatMoney(item.taxAmount ?? 0)}
            </SectionField>
            <SectionField label="Total amount" labelWidth="4">
              {formatMoney(item.totalAmount ?? 0)}
            </SectionField>
          </>
        ))}
      </Section>

      <Section>
        <SectionField label="Detailed remarks" labelWidth="12">
          {compensation.detailedRemarks}
        </SectionField>
      </Section>

      <Section>
        <StyledCompensationFooter>
          <div>
            <label>
              Compensation amount: <span>{formatMoney(payeeDetails?.preTaxAmount ?? 0)}</span>
            </label>
          </div>
          <div>
            <label>
              Applicable GST: <span>{formatMoney(payeeDetails?.taxAmount ?? 0)}</span>
            </label>
          </div>
          <div>
            <label>
              Total cheque amount: <span>{formatMoney(payeeDetails?.totalAmount ?? 0)}</span>
            </label>
          </div>
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

const RightFlexDiv = styled.div`
  display: flex;
  flex-direction: row-reverse;
`;

const StyledCompensationFooter = styled.div`
  display: flex;
  flex-direction: column;
  justify-content: flex-end;
  align-items: flex-end;
  font-size: 16px;
  font-weight: 600;
  span {
    margin-left: 1.25rem;
  }
`;

const StyledPayeeDisplayName = styled.div`
  display: flex;
  flex-direction: row;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-bottom: 1rem;
`;

const StyledSubHeader = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  border-bottom: solid 0.1rem ${props => props.theme.css.primaryColor};
  margin-bottom: 2rem;

  label {
    color: ${props => props.theme.css.primaryColor};
    font-family: 'BCSans-Bold';
    font-size: 1.75rem;
    width: 100%;
    text-align: left;
  }

  button {
    margin-bottom: 1rem;
  }
`;

import EditButton from 'components/common/EditButton';
import { StyledAddButton } from 'components/common/styles';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import Claims from 'constants/claims';
import { HeaderField } from 'features/mapSideBar/tabs/HeaderField';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { StyledSummarySection } from 'features/mapSideBar/tabs/SectionStyles';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_CompensationRequisition } from 'models/api/CompensationRequisition';
import { Api_Product, Api_Project } from 'models/api/Project';
import { Col, Row } from 'react-bootstrap';
import { FaMoneyCheck } from 'react-icons/fa';
import styled from 'styled-components';
import { formatMoney, prettyFormatDate } from 'utils';

export interface CompensationRequisitionDetailViewProps {
  compensation: Api_CompensationRequisition;
  acqFileProject?: Api_Project;
  acqFileProduct?: Api_Product | undefined;
  clientConstant: string;
  gstConstant: number | undefined;
  loading: boolean;
  setEditMode: (editMode: boolean) => void;
  onGenerate: (compensation: Api_CompensationRequisition) => void;
}

export const CompensationRequisitionDetailView: React.FunctionComponent<
  React.PropsWithChildren<CompensationRequisitionDetailViewProps>
> = ({
  compensation,
  acqFileProject,
  acqFileProduct,
  clientConstant,
  loading,
  setEditMode,
  onGenerate,
}) => {
  const { hasClaim } = useKeycloakWrapper();

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
              {formatMoney(0)}
            </HeaderField>
            <HeaderField label="Applicable GST:" labelWidth="8">
              {formatMoney(0)}
            </HeaderField>
            <HeaderField label="Total cheque amount:" labelWidth="8">
              {formatMoney(0)}
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
      </Section>
      <Section>
        <SectionField label="Detailed remarks" labelWidth={'12'}>
          {compensation.detailedRemarks}
        </SectionField>
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

const RightFlexDiv = styled.div`
  display: flex;
  flex-direction: row-reverse;
`;

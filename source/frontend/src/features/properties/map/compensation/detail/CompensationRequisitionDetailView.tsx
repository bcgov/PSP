import EditButton from 'components/common/EditButton';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import Claims from 'constants/claims';
import { HeaderField } from 'features/mapSideBar/tabs/HeaderField';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { StyledEditWrapper, StyledSummarySection } from 'features/mapSideBar/tabs/SectionStyles';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { Api_Compensation } from 'models/api/Compensation';
import { Api_Product, Api_Project } from 'models/api/Project';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';
import { formatMoney, prettyFormatDate } from 'utils';

export interface CompensationRequisitionDetailViewProps {
  compensation: Api_Compensation;
  acqFileProject?: Api_Project;
  acqFileProduct?: Api_Product;
  clientConstant: string;
  gstConstant: number | undefined;
  loading: boolean;
  setEditMode: (editMode: boolean) => void;
}

export const CompensationRequisitionDetailView: React.FunctionComponent<
  React.PropsWithChildren<CompensationRequisitionDetailViewProps>
> = ({ compensation, acqFileProject, acqFileProduct, clientConstant, loading, setEditMode }) => {
  const { hasClaim } = useKeycloakWrapper();

  return (
    <StyledSummarySection>
      <LoadingBackdrop show={loading} parentScreen={true} />
      <StyledEditWrapper>
        {setEditMode !== undefined && hasClaim(Claims.COMPENSATION_REQUISITION_EDIT) && (
          <EditButton
            title="Edit compensation requisition"
            onClick={() => {
              setEditMode(true);
            }}
          />
        )}
      </StyledEditWrapper>
      <Section>
        <StyledRow className="no-gutters">
          <Col xs="7">
            <Row className="no-gutters">
              <Col>
                <HeaderField label="Client:" contentWidth="4" valueTestId={'compensation-client'}>
                  {clientConstant}
                </HeaderField>
              </Col>
            </Row>
            <Row className="no-gutters">
              <Col>
                <HeaderField label="Requisition number:" contentWidth="4">
                  {compensation.isDraft ? 'Draft' : compensation.id}
                </HeaderField>
              </Col>
            </Row>
          </Col>
          <Col xs="5">
            <Row className="no-gutters">
              <Col>
                <HeaderField label="Compensation amount:" contentWidth="4">
                  {formatMoney(0)}
                </HeaderField>
              </Col>
            </Row>
            <Row className="no-gutters">
              <Col>
                <HeaderField label="Applicable GST:" contentWidth="4">
                  {formatMoney(0)}
                </HeaderField>
              </Col>
            </Row>
            <Row className="no-gutters">
              <Col>
                <HeaderField label="Total cheque amount:" contentWidth="4">
                  {formatMoney(0)}
                </HeaderField>
              </Col>
            </Row>
          </Col>
        </StyledRow>
      </Section>
      <Section header="Requisition Details">
        <SectionField label="Status" labelWidth={'4'}>
          {compensation.isDraft ? 'Draft' : 'Final'}
        </SectionField>
        <SectionField label="Agreement date" labelWidth={'4'}>
          {prettyFormatDate(compensation.agreementDate)}
        </SectionField>
        <SectionField label="Expropriation notice server date" labelWidth={'4'}>
          {prettyFormatDate(compensation.expropriationNoticeServedDate)}
        </SectionField>
        <SectionField label="Expropriation vesting date" labelWidth={'4'}>
          {prettyFormatDate(compensation.expropriationVestingDate)}
        </SectionField>
        <SectionField label="Special instructions" labelWidth={'12'}>
          {compensation.specialInstruction}
        </SectionField>
      </Section>
      <Section header="Financial Coding" isCollapsable initiallyExpanded>
        <SectionField label="Product" labelWidth={'4'}>
          {acqFileProduct?.code ?? ''}
        </SectionField>
        <SectionField label="Business function" labelWidth={'4'}>
          {acqFileProject?.code ?? ''}
        </SectionField>
        <SectionField label="Work activity" labelWidth={'4'}>
          {acqFileProject?.workActivityCode?.code ?? ''}
        </SectionField>
        <SectionField label="Cost type" labelWidth={'4'}>
          {acqFileProject?.costTypeCode?.code ?? ''}
        </SectionField>
        <SectionField label="Fiscal year" labelWidth={'4'}>
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

import moment from 'moment';
import { Col, Row } from 'react-bootstrap';
import { useHistory } from 'react-router-dom';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons';
import EditButton from '@/components/common/buttons/EditButton';
import { FormSection } from '@/components/common/form/styles';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import {
  InlineMessage,
  StyledEditWrapper,
  StyledInlineMessageSection,
} from '@/components/common/Section/SectionStyles';
import { Claims } from '@/constants';
import { useQuery } from '@/hooks/use-query';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { IBcAssessmentSummary } from '@/models/layers/bcAssesment';
import { exists, formatBcaAddress, formatMoney } from '@/utils';
import { pidFormatter } from '@/utils/propertyUtils';

import AssessedValuesTable from './AssessedValuesTable';
import SalesTable from './SalesTable';

export interface IBcAssessmentTabViewProps {
  property?: ApiGen_Concepts_Property;
  summaryData?: IBcAssessmentSummary;
  requestedOn?: Date;
  loading: boolean;
  pid?: string;
}

export const BcAssessmentTabView: React.FunctionComponent<IBcAssessmentTabViewProps> = ({
  property,
  summaryData,
  requestedOn,
  loading,
  pid,
}) => {
  const address = summaryData?.ADDRESSES?.find(a => a.PRIMARY_IND === 'true');
  const { hasClaim, obj } = useKeycloakWrapper();
  const logout = obj.logout;
  const query = useQuery();
  const history = useHistory();

  return (
    <>
      <LoadingBackdrop show={loading} parentScreen={true} />

      {!exists(pid) ? (
        <FormSection>
          <b>
            This property does not have a valid PID.
            <br />
            <br /> Only properties that are associated to a valid PID can display corresponding data
            from LTSA.
          </b>
        </FormSection>
      ) : !loading && !exists(summaryData) ? (
        <FormSection>
          <b>
            Failed to load data from BC Assessment.
            <br />
            <br /> Refresh this page to try again, your SITEMINDER credentials may have expired. You
            can refresh your credentials by logging out and logging back into the application. If
            this error persists past the logout/login, contact a site administrator.
          </b>
          <Button className="m-auto" onClick={() => logout()}>
            Log Out
          </Button>
        </FormSection>
      ) : (
        <StyledForm>
          {requestedOn && (
            <StyledInlineMessageSection>
              <InlineMessage>
                This data was retrieved from BC Assessment on{' '}
                {moment(requestedOn).format('DD-MMM-YYYY h:mm A')}
              </InlineMessage>
            </StyledInlineMessageSection>
          )}
          {exists(property) && (
            <Section
              header={
                <Row>
                  <Col md="10">Net Book</Col>
                  <Col md="2" className="d-flex align-items-center justify-content-end pr-0">
                    <StyledEditWrapper>
                      {hasClaim(Claims.DISPOSITION_EDIT) && (
                        <EditButton
                          title="Edit net book value"
                          onClick={() => {
                            query.set('edit', 'true');
                            history.push({ search: query.toString() });
                          }}
                        />
                      )}
                    </StyledEditWrapper>
                  </Col>
                </Row>
              }
            >
              <SectionField label="Net book value">
                {formatMoney(property.netBookAmount)}
              </SectionField>
              <SectionField label="Notes">{property.netBookNote}</SectionField>
            </Section>
          )}
          <Section header="Assessment Overview">
            <SectionField label="PID">{pidFormatter(pid)}</SectionField>
            <SectionField label="Jurisdiction">
              {summaryData?.FOLIO_DESCRIPTION?.JURISDICTION_CODE
                ? `${summaryData?.FOLIO_DESCRIPTION.JURISDICTION_CODE} - ${summaryData?.FOLIO_DESCRIPTION.JURISDICTION}`
                : ''}
            </SectionField>
            <SectionField label="Neighbourhood">
              {summaryData?.FOLIO_DESCRIPTION?.NEIGHBOURHOOD_CODE
                ? `${summaryData?.FOLIO_DESCRIPTION.NEIGHBOURHOOD_CODE} - ${summaryData?.FOLIO_DESCRIPTION.NEIGHBOURHOOD}`
                : ''}
            </SectionField>
            <SectionField label="Ownership year">Not available</SectionField>
            <SectionField label="Roll number">
              {summaryData?.FOLIO_DESCRIPTION?.ROLL_NUMBER ?? ''}
            </SectionField>
            <SectionField label="Roll year">Not available</SectionField>
            <SectionField label="Document number">Not available</SectionField>
          </Section>
          <Section header="Property Address">
            <StyledSubtleText>
              This is the property address as per BC Assessment (for reference).
            </StyledSubtleText>

            {address !== undefined ? (
              <SectionField label="Address">{formatBcaAddress(address)}</SectionField>
            ) : (
              <b>Unable to determine address from BC Assessment</b>
            )}
            <SectionField label="City">{address?.CITY ?? ''}</SectionField>
            <SectionField label="Province">{address?.PROVINCE ?? ''}</SectionField>
            <SectionField label="Postal code">{address?.POSTAL_CODE ?? ''}</SectionField>
          </Section>
          <Section header="Assessed Value">
            <AssessedValuesTable valuesData={summaryData?.VALUES ?? []} />
          </Section>
          <Section header="Assessment Details">
            <SectionField label="Manual class">
              {summaryData?.FOLIO_DESCRIPTION?.MANUAL_CLASS_CODE
                ? `${summaryData?.FOLIO_DESCRIPTION.MANUAL_CLASS_CODE} - ${summaryData?.FOLIO_DESCRIPTION.MANUAL_CLASS_DESCRIPTION}`
                : ''}
            </SectionField>
            <SectionField label="Actual use">
              {summaryData?.FOLIO_DESCRIPTION?.ACTUAL_USE_CODE
                ? `${summaryData?.FOLIO_DESCRIPTION.ACTUAL_USE_CODE} - ${summaryData?.FOLIO_DESCRIPTION.ACTUAL_USE_DESCRIPTION}`
                : ''}
            </SectionField>
            <SectionField label="ALR">
              {summaryData?.FOLIO_DESCRIPTION?.ALR_CODE
                ? `${summaryData?.FOLIO_DESCRIPTION.ALR_CODE} - ${summaryData?.FOLIO_DESCRIPTION.ALR_DESCRIPTION}`
                : ''}
            </SectionField>
            <SectionField label="Land dimension">
              {`${summaryData?.FOLIO_DESCRIPTION?.LAND_SIZE ?? ''} ${
                summaryData?.FOLIO_DESCRIPTION?.LAND_UNITS ?? ''
              } `}
            </SectionField>
          </Section>
          <Section header="Sales">
            <SalesTable salesData={summaryData?.SALES} />
          </Section>
        </StyledForm>
      )}
    </>
  );
};

export const StyledForm = styled.div`
  position: relative;
  &&& {
    input,
    select,
    textarea {
      background: none;
      border: none;
      resize: none;
      height: fit-content;
      padding: 0;
    }
    .form-label {
      font-weight: bold;
    }
  }
`;

const StyledSubtleText = styled.p`
  color: ${props => props.theme.bcTokens.iconsColorDisabled};
  text-align: left;
`;

export default BcAssessmentTabView;

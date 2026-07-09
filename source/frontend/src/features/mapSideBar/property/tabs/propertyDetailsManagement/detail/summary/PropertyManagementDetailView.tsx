import { Col, Row } from 'react-bootstrap';
import { useHistory } from 'react-router-dom';

import { EditButton } from '@/components/common/buttons/EditButton';
import { PrimaryContactSelectorDetails } from '@/components/common/form/PrimaryContactSelector/PrimaryContactSelectorView';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { MultiselectTextList } from '@/components/common/MultiselectTextList';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledEditWrapper } from '@/components/common/Section/SectionStyles';
import { Claims } from '@/constants/index';
import { useQuery } from '@/hooks/use-query';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { ApiGen_Concepts_PropertyManagement } from '@/models/api/generated/ApiGen_Concepts_PropertyManagement';
import { exists, formatApiPropertyManagementLease } from '@/utils';
import { booleanToYesNoUnknownString } from '@/utils/formUtils';
import { formatApiPersonNames } from '@/utils/personUtils';

export interface IPropertyManagementDetailViewProps {
  isLoading: boolean;
  propertyManagement: ApiGen_Concepts_PropertyManagement | null;
  responsiblePayerPerson: ApiGen_Concepts_Person | null;
  responsiblePayerOrganization: ApiGen_Concepts_Organization | null;
  primaryContact: ApiGen_Concepts_Person | null;
}

export const PropertyManagementDetailView: React.FC<IPropertyManagementDetailViewProps> = ({
  isLoading,
  propertyManagement,
  responsiblePayerPerson,
  responsiblePayerOrganization,
  primaryContact,
}) => {
  const { hasClaim } = useKeycloakWrapper();
  const query = useQuery();
  const history = useHistory();

  return (
    <Section
      header={
        <Row>
          <Col md="10">Summary</Col>
          <Col md="2" className="d-flex align-items-center justify-content-end pr-0">
            <StyledEditWrapper>
              {hasClaim(Claims.MANAGEMENT_EDIT) && (
                <EditButton
                  title="Edit property management information"
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
      isCollapsable
      initiallyExpanded
    >
      <LoadingBackdrop show={isLoading} />

      <SectionField label="Property purpose">
        <MultiselectTextList
          values={
            propertyManagement?.managementPurposes?.map(mp => mp.propertyPurposeTypeCode) ?? []
          }
        />
      </SectionField>
      <SectionField label="Active Lease/Licence" valueTestId="active-lease-information">
        {formatApiPropertyManagementLease(propertyManagement)}
      </SectionField>
      <SectionField label="Utilities payable">
        {booleanToYesNoUnknownString(propertyManagement?.isUtilitiesPayable)}
      </SectionField>
      <SectionField label="Taxes payable">
        {booleanToYesNoUnknownString(propertyManagement?.isTaxesPayable)}
      </SectionField>

      <PrimaryContactSelectorDetails
        label={'Responsible payer'}
        teamMemberName={
          exists(responsiblePayerOrganization)
            ? responsiblePayerOrganization.name
            : exists(responsiblePayerPerson)
            ? formatApiPersonNames(responsiblePayerPerson)
            : ''
        }
        teamMemberUrl={
          exists(propertyManagement?.responsiblePayerOrganizationId)
            ? `/contact/O${propertyManagement?.responsiblePayerOrganizationId}`
            : `/contact/P${propertyManagement?.responsiblePayerPersonId}`
        }
        primaryContactName={exists(primaryContact) ? formatApiPersonNames(primaryContact) : ''}
        primaryContactUrl={`/contact/P${propertyManagement?.responsiblePayerPrimaryContactId}`}
        showPrimaryContact={!!propertyManagement?.responsiblePayerOrganizationId}
        index={0}
      ></PrimaryContactSelectorDetails>

      <SectionField
        label="Additional details"
        contentWidth={{ xs: 12 }}
        tooltip="Describe the purpose of the property for the Ministry"
      >
        {propertyManagement?.additionalDetails}
      </SectionField>
    </Section>
  );
};

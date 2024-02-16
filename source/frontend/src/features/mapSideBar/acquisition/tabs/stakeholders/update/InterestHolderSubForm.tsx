import { FieldArrayRenderProps, FormikErrors, getIn, useFormikContext } from 'formik';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaTrash } from 'react-icons/fa';

import { StyledRemoveLinkButton } from '@/components/common/buttons';
import { DisplayError, Select } from '@/components/common/form';
import { SelectOption } from '@/components/common/form';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import { SectionField } from '@/components/common/Section/SectionField';
import FilePropertiesTable from '@/components/filePropertiesTable/FilePropertiesTable';
import * as API from '@/constants/API';
import { useOrganizationRepository } from '@/features/contacts/repositories/useOrganizationRepository';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_OrganizationPerson } from '@/models/api/generated/ApiGen_Concepts_OrganizationPerson';
import { exists } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import { InterestHolderForm, StakeHolderForm } from './models';

export interface IInterestHolderProps {
  index: number;
  errors: FormikErrors<StakeHolderForm>;
  arrayHelpers: FieldArrayRenderProps;
  file: ApiGen_Concepts_AcquisitionFile;
  interestHolder: InterestHolderForm;
}

export const InterestHolderSubForm: React.FunctionComponent<IInterestHolderProps> = ({
  index,
  errors,
  arrayHelpers,
  file,
  interestHolder,
}) => {
  const { values, setFieldValue } = useFormikContext<StakeHolderForm>();
  const { getOptionsByType } = useLookupCodeHelpers();
  const interestHolderInterestTypes = getOptionsByType(API.INTEREST_HOLDER_TYPES);
  const interestHolderContact = values.interestHolders[index].contact;

  const {
    getOrganizationDetail: { execute: fetchOrganization, response: organization },
  } = useOrganizationRepository();

  React.useEffect(() => {
    if (interestHolder.contact?.organizationId) {
      fetchOrganization(interestHolder.contact?.organizationId);
    }
  }, [interestHolder.contact?.organizationId, fetchOrganization]);

  const orgPersons = organization?.organizationPersons;

  React.useEffect(() => {
    if (orgPersons?.length === 0) {
      setFieldValue(`interestHolders.${index}.primaryContactId`, null);
    }
    if (orgPersons?.length === 1) {
      setFieldValue(`interestHolders.${index}.primaryContactId`, orgPersons[0].personId);
    }
  }, [orgPersons, setFieldValue, index]);

  const primaryContacts: SelectOption[] =
    orgPersons?.map((orgPerson: ApiGen_Concepts_OrganizationPerson) => {
      return {
        label: `${formatApiPersonNames(orgPerson.person)}`,
        value: orgPerson.personId ?? '',
      };
    }) ?? [];

  return (
    <React.Fragment
      key={
        interestHolder?.interestHolderId
          ? `interest-holder-${interestHolder?.interestHolderId}`
          : `interest-holder-${index}`
      }
    >
      {''}
      <SectionField label="Interest holder">
        <Row className="pb-0">
          <Col>
            <ContactInputContainer
              field={`interestHolders.${index}.contact`}
              View={ContactInputView}
            ></ContactInputContainer>
          </Col>
          <Col xs="auto">
            <StyledRemoveLinkButton
              title="Remove Interest"
              variant="light"
              onClick={() => {
                arrayHelpers.remove(index);
              }}
            >
              <FaTrash size="2rem" />
            </StyledRemoveLinkButton>
          </Col>
          {getIn(errors, `interestHolders.${index}.contact`) && (
            <DisplayError field={`interestHolders.${index}.contact`} />
          )}
        </Row>
      </SectionField>
      {interestHolderContact?.organizationId && !interestHolderContact?.personId && (
        <SectionField label="Primary contact">
          {primaryContacts.length > 1 ? (
            <Select
              options={primaryContacts}
              field={`interestHolders.${index}.primaryContactId`}
              placeholder="Select a primary contact"
            />
          ) : primaryContacts.length > 0 ? (
            primaryContacts[0].label
          ) : (
            'No contacts available'
          )}
        </SectionField>
      )}
      <SectionField label="Interest type">
        <Select
          options={interestHolderInterestTypes}
          field={`interestHolders.${index}.propertyInterestTypeCode`}
          placeholder="Select an interest type"
        />
      </SectionField>
      <SectionField
        label="Impacted properties"
        tooltip="The interest holder will show on the Compensation Request form relevant to these properties."
        labelWidth="4"
        contentWidth="8"
      >
        <FilePropertiesTable
          fileProperties={file.fileProperties ?? []}
          selectedFileProperties={
            interestHolder.impactedProperties
              .map(ip => file.fileProperties?.find(fp => fp.id === ip.acquisitionFilePropertyId))
              .filter(exists) ?? []
          }
          setSelectedFileProperties={(fileProperties: ApiGen_Concepts_FileProperty[]) => {
            const interestHolderProperties = fileProperties.map(fileProperty => {
              const matchingProperty = interestHolder.impactedProperties.find(
                ip => ip.acquisitionFilePropertyId === fileProperty.id,
              );

              return matchingProperty
                ? matchingProperty
                : {
                    acquisitionFileProperty: fileProperty,
                    acquisitionFilePropertyId: fileProperty.id,
                  };
            });
            setFieldValue(`interestHolders.${index}.impactedProperties`, interestHolderProperties);
          }}
          disabledSelection={false}
        />
        {getIn(errors, `interestHolders.${index}.impactedProperties`) && (
          <DisplayError field={`interestHolders.${index}.impactedProperties`} />
        )}
      </SectionField>
    </React.Fragment>
  );
};

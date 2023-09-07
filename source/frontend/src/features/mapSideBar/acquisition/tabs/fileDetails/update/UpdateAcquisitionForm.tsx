import { Formik, FormikHelpers, FormikProps } from 'formik';
import React from 'react';
import styled from 'styled-components';

import {
  FastDatePicker,
  Input,
  ProjectSelector,
  Select,
  SelectOption,
  TextArea,
} from '@/components/common/form';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import { UserRegionSelectContainer } from '@/components/common/form/UserRegionSelect/UserRegionSelectContainer';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSectionParagraph } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import { RestrictContactType } from '@/components/contact/ContactManagerView/ContactFilterComponent/ContactFilterComponent';
import * as API from '@/constants/API';
import { useOrganizationRepository } from '@/features/contacts/repositories/useOrganizationRepository';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import { useLookupCodeHelpers } from '@/hooks/useLookupCodeHelpers';
import { IAutocompletePrediction } from '@/interfaces';
import { Api_OrganizationPerson } from '@/models/api/Organization';
import { Api_Product } from '@/models/api/Project';
import { formatApiPersonNames } from '@/utils/personUtils';

import UpdateAcquisitionOwnersSubForm from '../../../common/update/acquisitionOwners/UpdateAcquisitionOwnersSubForm';
import { UpdateAcquisitionTeamSubForm } from '../../../common/update/acquisitionTeam/UpdateAcquisitionTeamSubForm';
import { UpdateAcquisitionSummaryFormModel } from './models';
import StatusToolTip from './StatusToolTip';

export interface IUpdateAcquisitionFormProps {
  formikRef: React.Ref<FormikProps<UpdateAcquisitionSummaryFormModel>>;
  /** Initial values of the form */
  initialValues: UpdateAcquisitionSummaryFormModel;
  /** A Yup Schema or a function that returns a Yup schema */
  validationSchema?: any | (() => any);
  /** Submission handler */
  onSubmit: (
    values: UpdateAcquisitionSummaryFormModel,
    formikHelpers: FormikHelpers<UpdateAcquisitionSummaryFormModel>,
  ) => void | Promise<any>;
}

export const UpdateAcquisitionForm: React.FC<IUpdateAcquisitionFormProps> = props => {
  const { formikRef, initialValues, validationSchema, onSubmit } = props;

  return (
    <Formik<UpdateAcquisitionSummaryFormModel>
      enableReinitialize
      innerRef={formikRef}
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={onSubmit}
    >
      {formikProps => {
        return <AcquisitionDetailSubForm formikProps={formikProps}></AcquisitionDetailSubForm>;
      }}
    </Formik>
  );
};

export default UpdateAcquisitionForm;

const AcquisitionDetailSubForm: React.FC<{
  formikProps: FormikProps<UpdateAcquisitionSummaryFormModel>;
}> = ({ formikProps }) => {
  const {
    setFieldValue,
    initialValues,
    values: { fileStatusTypeCode },
  } = formikProps;

  const [projectProducts, setProjectProducts] = React.useState<Api_Product[] | undefined>(
    undefined,
  );
  const { retrieveProjectProducts } = useProjectProvider();
  const { getOptionsByType } = useLookupCodeHelpers();
  const regionTypes = getOptionsByType(API.REGION_TYPES);
  const acquisitionTypes = getOptionsByType(API.ACQUISITION_TYPES);
  const acquisitionPhysFileTypes = getOptionsByType(API.ACQUISITION_PHYSICAL_FILE_STATUS_TYPES);
  const fileStatusTypeCodes = getOptionsByType(API.ACQUISITION_FILE_STATUS_TYPES);
  const acquisitionFundingTypes = getOptionsByType(API.ACQUISITION_FUNDING_TYPES);
  const ownerSolicitorContact = formikProps.values.ownerSolicitor.contact;

  const onMinistryProjectSelected = React.useCallback(
    async (param: IAutocompletePrediction[]) => {
      if (param.length > 0) {
        if (param[0].id !== undefined) {
          const result = await retrieveProjectProducts(param[0].id);
          if (result !== undefined) {
            setProjectProducts(result);
          }
        }
      } else {
        setProjectProducts(undefined);
      }
    },
    [retrieveProjectProducts],
  );

  React.useEffect(() => {
    if (initialValues.project !== undefined) {
      onMinistryProjectSelected([initialValues.project]);
    }
  }, [initialValues, onMinistryProjectSelected]);

  // clear the associated 'Completion Date' field if the corresponding File Status has its value changed from COMPLETE to something else.
  React.useEffect(() => {
    if (!!fileStatusTypeCode && fileStatusTypeCode !== 'COMPLT') {
      setFieldValue('completionDate', '');
    }
  }, [fileStatusTypeCode, setFieldValue]);

  const {
    getOrganizationDetail: { execute: fetchOrganization, response: organization },
  } = useOrganizationRepository();

  React.useEffect(() => {
    if (ownerSolicitorContact?.organizationId) {
      fetchOrganization(ownerSolicitorContact?.organizationId);
    }
  }, [ownerSolicitorContact?.organizationId, fetchOrganization]);

  const orgPersons = organization?.organizationPersons;

  React.useEffect(() => {
    if (orgPersons?.length === 0) {
      setFieldValue('ownerSolicitor.primaryContactId', null);
    }
    if (orgPersons?.length === 1) {
      setFieldValue('ownerSolicitor.primaryContactId', orgPersons[0].personId);
    }
  }, [orgPersons, setFieldValue]);

  const primaryContacts: SelectOption[] =
    orgPersons?.map((orgPerson: Api_OrganizationPerson) => {
      return {
        label: `${formatApiPersonNames(orgPerson.person)}`,
        value: orgPerson.personId ?? ' ',
      };
    }) ?? [];

  return (
    <Container>
      <Section>
        <SectionField
          label="Status"
          tooltip={
            <TooltipIcon
              className="tooltip-light"
              toolTipId="status-field-tooltip"
              toolTip={<StatusToolTip />}
              placement="auto"
            />
          }
        >
          <Select
            field="fileStatusTypeCode"
            options={fileStatusTypeCodes}
            placeholder="Select..."
            required
          />
        </SectionField>
      </Section>

      <Section header="Project">
        <SectionField
          label="Ministry project"
          tooltip="Be sure to select a File project that is not the same as the Alternate Project on a Compensation Requisition."
        >
          <ProjectSelector
            field="project"
            onChange={(vals: IAutocompletePrediction[]) => {
              onMinistryProjectSelected(vals);
              if (vals.length === 0) {
                formikProps.setFieldValue('product', '');
              }
            }}
          />
        </SectionField>
        {projectProducts !== undefined && (
          <SectionField label="Product">
            <Select
              field="product"
              options={projectProducts.map<SelectOption>(x => {
                return { label: x.code + ' ' + x.description || '', value: x.id || 0 };
              })}
              placeholder="Select..."
            />
          </SectionField>
        )}
        <SectionField label="Funding">
          <Select
            field="fundingTypeCode"
            options={acquisitionFundingTypes}
            placeholder="Select..."
            onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
              const selectedValue = [].slice
                .call(e.target.selectedOptions)
                .map((option: HTMLOptionElement & number) => option.value)[0];
              if (!!selectedValue && selectedValue !== 'OTHER') {
                formikProps.setFieldValue('fundingTypeOtherDescription', '');
              }
            }}
          />
        </SectionField>
        {formikProps.values?.fundingTypeCode === 'OTHER' && (
          <SectionField label="Other funding">
            <Input field="fundingTypeOtherDescription" />
          </SectionField>
        )}
      </Section>

      <Section header="Schedule">
        <SectionField label="Assigned date">
          <FastDatePicker field="assignedDate" formikProps={formikProps} />
        </SectionField>
        <SectionField
          label="Delivery date"
          tooltip="Date for delivery of the property to the project"
        >
          <FastDatePicker field="deliveryDate" formikProps={formikProps} />
        </SectionField>
        <SectionField
          label="Acquisition completed date"
          tooltip={`This will be enabled when the file status is set to "Completed"`}
          required={formikProps.values?.fileStatusTypeCode === 'COMPLT'}
        >
          <FastDatePicker
            field="completionDate"
            formikProps={formikProps}
            disabled={formikProps.values?.fileStatusTypeCode !== 'COMPLT'}
          />
        </SectionField>
      </Section>

      <Section header="Acquisition Details">
        <SectionField label="Acquisition file name" required>
          <Input field="fileName" />
        </SectionField>
        <SectionField
          label="Historical file number"
          tooltip="Older file that this file represents (ex: those from the legacy system or other non-digital files.)"
        >
          <Input field="legacyFileNumber" />
        </SectionField>
        <SectionField label="Physical file status">
          <Select
            field="acquisitionPhysFileStatusType"
            options={acquisitionPhysFileTypes}
            placeholder="Select..."
          />
        </SectionField>
        <SectionField label="Acquisition type" required>
          <Select
            field="acquisitionType"
            options={acquisitionTypes}
            placeholder="Select..."
            required
          />
        </SectionField>
        <SectionField label="Ministry region" required>
          <UserRegionSelectContainer
            field="region"
            options={regionTypes}
            placeholder="Select region..."
            required
          />
        </SectionField>
      </Section>

      <Section header="Acquisition Team">
        <UpdateAcquisitionTeamSubForm />
        {formikProps.errors?.team && typeof formikProps.errors?.team === 'string' && (
          <div className="invalid-feedback" data-testid="team-profile-dup-error">
            {formikProps.errors.team.toString()}
          </div>
        )}
      </Section>

      <Section header="Owners">
        <StyledSectionParagraph>
          Each property in this file should be owned by the owner(s) in this section
        </StyledSectionParagraph>
        <UpdateAcquisitionOwnersSubForm />
        <SectionField label="Owner solicitor" className="mt-4">
          <ContactInputContainer
            field="ownerSolicitor.contact"
            View={ContactInputView}
          ></ContactInputContainer>
        </SectionField>
        {ownerSolicitorContact?.organizationId && !ownerSolicitorContact?.personId && (
          <SectionField label="Primary contact" className="mt-4">
            {primaryContacts.length > 1 ? (
              <Select
                field="ownerSolicitor.primaryContactId"
                options={primaryContacts}
                placeholder="Select a primary contact..."
              ></Select>
            ) : primaryContacts.length === 1 ? (
              primaryContacts[0].label
            ) : (
              'No contacts available'
            )}
          </SectionField>
        )}
        <SectionField label="Owner representative">
          <ContactInputContainer
            field="ownerRepresentative.contact"
            View={ContactInputView}
            restrictContactType={RestrictContactType.ONLY_INDIVIDUALS}
          ></ContactInputContainer>
        </SectionField>
        <SectionField label="Comment">
          <TextArea
            field="ownerRepresentative.comment"
            placeholder="Remarks or additional representative(s)"
          ></TextArea>
        </SectionField>
      </Section>
    </Container>
  );
};

const Container = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};

  [name='region'] {
    max-width: 25rem;
  }
`;

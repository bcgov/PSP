import { Formik, FormikHelpers, FormikProps } from 'formik';
import React from 'react';
import styled from 'styled-components';

import {
  FastDatePicker,
  Input,
  Multiselect,
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
import { ApiGen_CodeTypes_SubfileInterestTypes } from '@/models/api/generated/ApiGen_CodeTypes_SubfileInterestTypes';
import { ApiGen_Concepts_PersonOrganization } from '@/models/api/generated/ApiGen_Concepts_PersonOrganization';
import { ApiGen_Concepts_Product } from '@/models/api/generated/ApiGen_Concepts_Product';
import { exists, isValidId, isValidString } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import UpdateAcquisitionOwnersSubForm from '../../../common/update/acquisitionOwners/UpdateAcquisitionOwnersSubForm';
import { UpdateAcquisitionTeamSubForm } from '../../../common/update/acquisitionTeam/UpdateAcquisitionTeamSubForm';
import { ProgressStatusModel } from '../../../models/ProgressStatusModel';
import { TakingTypeStatusModel } from '../../../models/TakingTypeStatusModel';
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
  const { setFieldValue, initialValues, values } = formikProps;

  const [projectProducts, setProjectProducts] = React.useState<
    ApiGen_Concepts_Product[] | undefined
  >(undefined);
  const { retrieveProjectProducts } = useProjectProvider();
  const { getOptionsByType, getByType } = useLookupCodeHelpers();
  const regionTypes = getOptionsByType(API.REGION_TYPES);
  const acquisitionTypes = getOptionsByType(API.ACQUISITION_TYPES);
  const acquisitionPhysFileTypes = getOptionsByType(API.ACQUISITION_PHYSICAL_FILE_STATUS_TYPES);
  const fileStatusTypeCodes = getOptionsByType(API.ACQUISITION_FILE_STATUS_TYPES);
  const acquisitionFundingTypes = getOptionsByType(API.ACQUISITION_FUNDING_TYPES);
  const ownerSolicitorContact = values.ownerSolicitor.contact;
  const subfileInterestTypes = getOptionsByType(API.SUBFILE_INTEREST_TYPES);

  const acquisitionProgressStatusTypesOptions = getByType(
    API.ACQUISITION_PROGRESS_STATUS_TYPES,
  ).map(x => ProgressStatusModel.fromLookup(x));
  const acquisitionAppraisalStatusTypes = getOptionsByType(API.ACQUISITION_APPRAISAL_STATUS_TYPES);
  const acquisitionLegalSurveyStatusTypes = getOptionsByType(
    API.ACQUISITION_LEGALSURVEY_STATUS_TYPES,
  );
  const acquisitionTakingStatusTypesOptions = getByType(API.ACQUISITION_TAKING_STATUS_TYPES).map(
    x => TakingTypeStatusModel.fromLookup(x),
  );
  const acquisitionExpropiationRiskStatusTypes = getOptionsByType(
    API.ACQUISITION_EXPROPIATIONRISK_STATUS_TYPES,
  );

  const onMinistryProjectSelected = React.useCallback(
    async (param: IAutocompletePrediction[]) => {
      if (param.length > 0) {
        if (isValidId(param[0].id)) {
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
    orgPersons?.map((orgPerson: ApiGen_Concepts_PersonOrganization) => {
      return {
        label: `${formatApiPersonNames(orgPerson.person)}`,
        value: orgPerson.personId ?? ' ',
      };
    }) ?? [];

  const isSubFile =
    exists(initialValues.parentAcquisitionFileId) &&
    isValidId(initialValues.parentAcquisitionFileId);

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
        {isSubFile ? (
          <>
            <SectionField
              label="Ministry project"
              tooltip="Sub-file has the same project as the main file and it can only be updated from the main file"
            >
              {values?.formattedProject ?? ''}
            </SectionField>
            <SectionField
              label="Product"
              tooltip="Sub-file has the same product as the main file and it can only be updated from the main file"
            >
              {values?.formatterProduct ?? ''}
            </SectionField>
          </>
        ) : (
          <>
            <SectionField
              label="Ministry project"
              tooltip="Be sure to select a File project that is not the same as the Alternate Project on a Compensation Requisition"
            >
              <ProjectSelector
                field="project"
                onChange={(vals: IAutocompletePrediction[]) => {
                  onMinistryProjectSelected(vals);
                  if (vals.length === 0) {
                    setFieldValue('product', '');
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
          </>
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
              if (isValidString(selectedValue) && selectedValue !== 'OTHER') {
                setFieldValue('fundingTypeOtherDescription', '');
              }
            }}
          />
        </SectionField>
        {values?.fundingTypeCode === 'OTHER' && (
          <SectionField label="Other funding">
            <Input field="fundingTypeOtherDescription" />
          </SectionField>
        )}
      </Section>

      <Section header="Progress Statuses">
        <SectionField label="File progress">
          <Multiselect
            field="progressStatuses"
            displayValue="progressTypeCodeDescription"
            placeholder=""
            options={acquisitionProgressStatusTypesOptions}
            hidePlaceholder
          />
        </SectionField>
        <SectionField label="Appraisal">
          <Select
            field="appraisalStatusType"
            options={acquisitionAppraisalStatusTypes}
            placeholder="Select appraisal"
          />
        </SectionField>
        <SectionField label="Legal survey">
          <Select
            field="legalSurveyStatusType"
            options={acquisitionLegalSurveyStatusTypes}
            placeholder="Select legal survey"
          />
        </SectionField>
        <SectionField label="Type of taking">
          <Multiselect
            field="takingStatuses"
            displayValue="takingTypeCodeDescription"
            placeholder=""
            options={acquisitionTakingStatusTypesOptions}
            hidePlaceholder
          />
        </SectionField>
        <SectionField label="Expropriation risk">
          <Select
            field="expropiationRiskStatusType"
            options={acquisitionExpropiationRiskStatusTypes}
            placeholder="Select expropiation risk"
          />
        </SectionField>
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
          label="Estimated date"
          tooltip="Estimated date by which the acquisition would be completed"
        >
          <FastDatePicker field="estimatedCompletionDate" formikProps={formikProps} />
        </SectionField>
        <SectionField label="Possession date">
          <FastDatePicker field="possessionDate" formikProps={formikProps} />
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

        {isSubFile && (
          <SectionField label="Sub-file interest" required>
            <Select
              field="subfileInterestTypeCode"
              options={subfileInterestTypes}
              placeholder="Select..."
              onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
                const selectedValue = [].slice
                  .call(e.target.selectedOptions)
                  .map((option: HTMLOptionElement & number) => option.value)[0];
                if (
                  !!selectedValue &&
                  selectedValue !== ApiGen_CodeTypes_SubfileInterestTypes.OTHER
                ) {
                  formikProps.setFieldValue('otherSubfileInterestType', null);
                } else {
                  formikProps.setFieldValue('otherSubfileInterestType', '');
                }
              }}
              required
              data-testid="subfileInterestTypeCode"
            />
          </SectionField>
        )}
        {isSubFile &&
          values?.subfileInterestTypeCode === ApiGen_CodeTypes_SubfileInterestTypes.OTHER && (
            <SectionField label="" required>
              <Input field="otherSubfileInterestType" placeholder="Describe other" required />
            </SectionField>
          )}

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

      <Section header={isSubFile ? 'Sub-Interest' : 'Owners'}>
        {isSubFile ? (
          <StyledSectionParagraph>
            Each property in this sub-file should be impacted by the sub-interest(s) in this section
          </StyledSectionParagraph>
        ) : (
          <StyledSectionParagraph>
            Each property in this file should be owned by the owner(s) in this section
          </StyledSectionParagraph>
        )}
        <UpdateAcquisitionOwnersSubForm isSubFile={isSubFile} />
        <SectionField
          label={isSubFile ? 'Sub-interest solicitor' : 'Owner solicitor'}
          className="mt-4"
        >
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
        <SectionField label={isSubFile ? 'Sub-interest representative' : 'Owner representative'}>
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
  background-color: ${props => props.theme.css.highlightBackgroundColor};

  [name='region'] {
    max-width: 25rem;
  }
`;

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
} from '@/components/common/form/';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import { UserRegionSelectContainer } from '@/components/common/form/UserRegionSelect/UserRegionSelectContainer';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSectionParagraph } from '@/components/common/styles';
import { RestrictContactType } from '@/components/contact/ContactManagerView/ContactFilterComponent/ContactFilterComponent';
import * as API from '@/constants/API';
import { useOrganizationRepository } from '@/features/contacts/repositories/useOrganizationRepository';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import { useLookupCodeHelpers } from '@/hooks/useLookupCodeHelpers';
import { IAutocompletePrediction } from '@/interfaces/IAutocomplete';
import { ApiGen_CodeTypes_SubfileInterestTypes } from '@/models/api/generated/ApiGen_CodeTypes_SubfileInterestTypes';
import { ApiGen_Concepts_PersonOrganization } from '@/models/api/generated/ApiGen_Concepts_PersonOrganization';
import { ApiGen_Concepts_Product } from '@/models/api/generated/ApiGen_Concepts_Product';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { exists, isValidId, isValidString } from '@/utils';
import { formatApiPersonNames } from '@/utils/personUtils';

import { PropertyForm } from '../../shared/models';
import { TeamMemberFormModal } from '../common/modals/AcquisitionFormModal';
import UpdateAcquisitionOwnersSubForm from '../common/update/acquisitionOwners/UpdateAcquisitionOwnersSubForm';
import { UpdateAcquisitionTeamSubForm } from '../common/update/acquisitionTeam/UpdateAcquisitionTeamSubForm';
import { ProgressStatusModel } from '../models/ProgressStatusModel';
import { TakingTypeStatusModel } from '../models/TakingTypeStatusModel';
import { AcquisitionPropertiesSubForm } from './AcquisitionPropertiesSubForm';
import { AcquisitionForm } from './models';

export interface IAddAcquisitionFormProps {
  formikRef: React.RefObject<FormikProps<AcquisitionForm>>;
  /** The parent acquisition file id - only applies to sub-files */
  parentId?: number;
  /** Initial values of the form */
  initialValues: AcquisitionForm;
  /** A Yup Schema or a function that returns a Yup schema */
  validationSchema?: any | (() => any);
  /** Submission handler */
  onSubmit: (
    values: AcquisitionForm,
    formikHelpers: FormikHelpers<AcquisitionForm>,
    userOverrides: UserOverrideCode[],
  ) => void | Promise<any>;
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
}

export const AddAcquisitionForm: React.FunctionComponent<IAddAcquisitionFormProps> = ({
  parentId,
  initialValues,
  validationSchema,
  onSubmit,
  confirmBeforeAdd,
  formikRef,
}) => {
  const [showDiffMinistryRegionModal, setShowDiffMinistryRegionModal] =
    React.useState<boolean>(false);

  const isMinistryRegionDiff = (values: AcquisitionForm): boolean => {
    const selectedPropRegions = values.properties.map(x => x.region);
    return (
      (selectedPropRegions.length > 0 &&
        (selectedPropRegions.indexOf(Number(values.region)) === -1 ||
          !selectedPropRegions.every(e => e === selectedPropRegions[0]))) ||
      false
    );
  };

  const handleSubmit = (values: AcquisitionForm, formikHelpers: FormikHelpers<AcquisitionForm>) => {
    if (isMinistryRegionDiff(values)) {
      setShowDiffMinistryRegionModal(true);
    } else {
      onSubmit(values, formikHelpers, []);
    }
  };

  return (
    <Formik<AcquisitionForm>
      innerRef={formikRef}
      initialValues={initialValues}
      validationSchema={validationSchema}
      validateOnChange={false}
      validateOnBlur={true}
      onSubmit={handleSubmit}
      enableReinitialize
    >
      {formikProps => {
        return (
          <AddAcquisitionDetailSubForm
            parentId={parentId}
            formikProps={formikProps}
            onSubmit={onSubmit}
            showDiffMinistryRegionModal={showDiffMinistryRegionModal}
            setShowDiffMinistryRegionModal={setShowDiffMinistryRegionModal}
            confirmBeforeAdd={confirmBeforeAdd}
          ></AddAcquisitionDetailSubForm>
        );
      }}
    </Formik>
  );
};

const AddAcquisitionDetailSubForm: React.FC<{
  parentId?: number;
  formikProps: FormikProps<AcquisitionForm>;
  onSubmit: (
    values: AcquisitionForm,
    formikHelpers: FormikHelpers<AcquisitionForm>,
    userOverrides: UserOverrideCode[],
  ) => void | Promise<any>;
  showDiffMinistryRegionModal: boolean;
  setShowDiffMinistryRegionModal: React.Dispatch<React.SetStateAction<boolean>>;
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
}> = ({
  parentId,
  formikProps,
  onSubmit,
  showDiffMinistryRegionModal,
  setShowDiffMinistryRegionModal,
  confirmBeforeAdd,
}) => {
  const [projectProducts, setProjectProducts] = React.useState<
    ApiGen_Concepts_Product[] | undefined
  >(undefined);

  const { values, setFieldValue } = formikProps;

  const ownerSolicitorContact = values?.ownerSolicitor.contact;

  const { retrieveProjectProducts } = useProjectProvider();
  const { getOptionsByType, getByType } = useLookupCodeHelpers();
  const acquisitionTypes = getOptionsByType(API.ACQUISITION_TYPES);
  const acquisitionPhysFileTypes = getOptionsByType(API.ACQUISITION_PHYSICAL_FILE_STATUS_TYPES);
  const acquisitionFundingTypes = getOptionsByType(API.ACQUISITION_FUNDING_TYPES);
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

  const isSubFile = exists(parentId) && isValidId(parentId);

  const {
    getOrganizationDetail: { execute: fetchOrganization, response: organization },
  } = useOrganizationRepository();

  const orgPersons = organization?.organizationPersons;
  const primaryContacts: SelectOption[] =
    orgPersons?.map((orgPerson: ApiGen_Concepts_PersonOrganization) => {
      return {
        label: `${formatApiPersonNames(orgPerson.person)}`,
        value: orgPerson.personId ?? ' ',
      };
    }) ?? [];

  const onMinistryProjectSelected = async (param: IAutocompletePrediction[]) => {
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
  };

  React.useEffect(() => {
    if (orgPersons?.length === 0) {
      setFieldValue('ownerSolicitor.primaryContactId', null);
    }
    if (orgPersons?.length === 1) {
      setFieldValue('ownerSolicitor.primaryContactId', orgPersons[0].personId);
    }
  }, [orgPersons, setFieldValue]);

  React.useEffect(() => {
    if (ownerSolicitorContact?.organizationId) {
      fetchOrganization(ownerSolicitorContact?.organizationId);
    }
  }, [ownerSolicitorContact?.organizationId, fetchOrganization]);

  return (
    <>
      <Container>
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
                {values?.formattedProduct ?? ''}
              </SectionField>
            </>
          ) : (
            <>
              <SectionField label="Ministry project">
                <ProjectSelector
                  field="project"
                  onChange={(vals: IAutocompletePrediction[]) => {
                    onMinistryProjectSelected(vals);
                    if (vals.length === 0) {
                      formikProps.setFieldValue('product', 0);
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
              <LargeInput field="fundingTypeOtherDescription" />
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

        <Section
          header={
            isSubFile
              ? 'Properties to include in this sub-file:'
              : 'Properties to include in this file:'
          }
        >
          <AcquisitionPropertiesSubForm
            formikProps={formikProps}
            confirmBeforeAdd={confirmBeforeAdd}
          />
        </Section>

        <Section header="Acquisition Details">
          <SectionField label="Acquisition file name" required>
            <LargeInput field="fileName" />
          </SectionField>
          <SectionField
            label="Historical file number"
            tooltip="Older file that this file represents (ex: those from the legacy system or other non-digital files.)"
          >
            <LargeInput field="legacyFileNumber" />
          </SectionField>
          <SectionField label="Physical file status">
            <Select
              field="acquisitionPhysFileStatusType"
              options={acquisitionPhysFileTypes}
              placeholder="Select..."
            />
          </SectionField>
          <SectionField
            label="Physical file details"
            tooltip="Location, the lawyer involved, which office it's with, and who currently has it."
          >
            <TextArea field="physicalFileDetails"></TextArea>
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
                <LargeInput
                  field="otherSubfileInterestType"
                  placeholder="Describe other"
                  required
                />
              </SectionField>
            )}

          <SectionField label="Ministry region" required>
            <UserRegionSelectContainer field="region" placeholder="Select region..." required />
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
              Each property in this sub-file should be impacted by the sub-interest(s) in this
              section
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

      <TeamMemberFormModal
        message={
          <>
            <p>
              The selected Ministry region is different from that associated to one or more selected
              properties.
            </p>
            <p>Do you want to proceed?</p>
          </>
        }
        title="Different Ministry region"
        display={showDiffMinistryRegionModal}
        handleOk={() => {
          setShowDiffMinistryRegionModal(false);
          onSubmit(formikProps.values, formikProps, [UserOverrideCode.UPDATE_REGION]);
        }}
        handleCancel={() => {
          setShowDiffMinistryRegionModal(false);
        }}
      ></TeamMemberFormModal>
    </>
  );
};

export default AddAcquisitionForm;

const LargeInput = styled(Input)`
  input.form-control {
    min-width: 50rem;
    max-width: 100%;
  }
`;

const Container = styled.div`
  .form-section {
    margin: 0;
    padding-left: 0;
  }

  .tab-pane {
    .form-section {
      margin: 1.5rem;
      padding-left: 1.5rem;
    }
  }

  [name='region'] {
    max-width: 25rem;
  }
`;

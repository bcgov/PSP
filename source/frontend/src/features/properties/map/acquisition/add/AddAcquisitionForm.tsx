import {
  FastDatePicker,
  Input,
  ProjectSelector,
  Select,
  SelectOption,
} from 'components/common/form/';
import { UserRegionSelectContainer } from 'components/common/form/UserRegionSelect/UserRegionSelectContainer';
import * as API from 'constants/API';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { Formik, FormikHelpers, FormikProps } from 'formik';
import { useProjectProvider } from 'hooks/repositories/useProjectProvider';
import { useLookupCodeHelpers } from 'hooks/useLookupCodeHelpers';
import { IAutocompletePrediction } from 'interfaces/IAutocomplete';
import { Api_Product } from 'models/api/Project';
import React from 'react';
import { Prompt } from 'react-router-dom';
import styled from 'styled-components';

import UpdateAcquisitionOwnersSubForm from '../common/update/acquisitionOwners/UpdateAcquisitionOwnersSubForm';
import { UpdateAcquisitionTeamSubForm } from '../common/update/acquisitionTeam/UpdateAcquisitionTeamSubForm';
import { AcquisitionFormModal } from '../modals/AcquisitionFormModal';
import { AcquisitionPropertiesSubForm } from './AcquisitionPropertiesSubForm';
import { AcquisitionForm } from './models';

export interface IAddAcquisitionFormProps {
  /** Initial values of the form */
  initialValues: AcquisitionForm;
  /** A Yup Schema or a function that returns a Yup schema */
  validationSchema?: any | (() => any);
  /** Submission handler */
  onSubmit: (
    values: AcquisitionForm,
    formikHelpers: FormikHelpers<AcquisitionForm>,
  ) => void | Promise<any>;
}

export const AddAcquisitionForm = React.forwardRef<
  FormikProps<AcquisitionForm>,
  IAddAcquisitionFormProps
>((props, ref) => {
  const { initialValues, validationSchema, onSubmit } = props;
  const [projectProducts, setProjectProducts] = React.useState<Api_Product[] | undefined>(
    undefined,
  );
  const { retrieveProjectProducts } = useProjectProvider();
  const { getOptionsByType } = useLookupCodeHelpers();
  const acquisitionTypes = getOptionsByType(API.ACQUISITION_TYPES);
  const acquisitionPhysFileTypes = getOptionsByType(API.ACQUISITION_PHYSICAL_FILE_STATUS_TYPES);
  const acquisitionFundingTypes = getOptionsByType(API.ACQUISITION_FUNDING_TYPES);
  const [showDiffMinistryRegionModal, setShowDiffMinistryRegionModal] =
    React.useState<boolean>(false);

  const isMinistryRegionDiff = (values: AcquisitionForm): boolean => {
    const selectedPropRegions = values.properties.map(x => x.region);
    return (
      (selectedPropRegions.length &&
        (selectedPropRegions.indexOf(Number(values.region)) === -1 ||
          !selectedPropRegions.every(e => e === selectedPropRegions[0]))) ||
      false
    );
  };

  const handleSubmit = (values: AcquisitionForm, formikHelpers: FormikHelpers<AcquisitionForm>) => {
    if (isMinistryRegionDiff(values)) {
      setShowDiffMinistryRegionModal(true);
    } else {
      onSubmit(values, formikHelpers);
    }
  };

  const onMinistryProjectSelected = async (param: IAutocompletePrediction[]) => {
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
  };

  return (
    <Formik<AcquisitionForm>
      enableReinitialize
      innerRef={ref}
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={handleSubmit}
    >
      {formikProps => (
        <>
          <Container>
            <Section header="Project">
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
                  <LargeInput field="fundingTypeOtherDescription" />
                </SectionField>
              )}
            </Section>

            <Section header="Schedule">
              <SectionField label="Assigned date">
                <FastDatePicker field="assignedDate" formikProps={formikProps} />
              </SectionField>
              <SectionField label="Delivery date">
                <FastDatePicker field="deliveryDate" formikProps={formikProps} />
              </SectionField>
            </Section>
            <Section header="Properties to include in this file:">
              <AcquisitionPropertiesSubForm formikProps={formikProps} />
            </Section>

            <Section header="Acquisition Details">
              <SectionField label="Acquisition file name">
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
              <SectionField label="Acquisition type">
                <Select
                  field="acquisitionType"
                  options={acquisitionTypes}
                  placeholder="Select..."
                  required
                />
              </SectionField>
              <SectionField label="Ministry region">
                <UserRegionSelectContainer field="region" placeholder="Select region..." required />
              </SectionField>
            </Section>

            <Section header="Acquisition Team">
              <UpdateAcquisitionTeamSubForm />
            </Section>
            <Section header="Owners">
              <UpdateAcquisitionOwnersSubForm />
            </Section>
          </Container>

          <Prompt
            when={
              (formikProps.dirty ||
                (formikProps.values.properties !== initialValues.properties &&
                  formikProps.submitCount === 0) ||
                (!formikProps.values.id && formikProps.values.properties.length > 0)) &&
              !formikProps.isSubmitting
            }
            message="You have made changes on this form. Do you wish to leave without saving?"
          />

          <AcquisitionFormModal
            message="Selected Ministry region is different from that of one or more selected properties. Do you wish to continue?"
            title="Different Ministry region"
            display={showDiffMinistryRegionModal}
            handleOk={() => {
              setShowDiffMinistryRegionModal(false);
              onSubmit(formikProps.values, formikProps);
            }}
            handleCancel={() => {
              setShowDiffMinistryRegionModal(false);
            }}
          ></AcquisitionFormModal>
        </>
      )}
    </Formik>
  );
});

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

  .react-datepicker-wrapper {
    max-width: 14rem;
  }

  [name='region'] {
    max-width: 25rem;
  }
`;

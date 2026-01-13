import { Formik, FormikProps } from 'formik';
import React from 'react';
import styled from 'styled-components';

import { FastCurrencyInput, TextArea } from '@/components/common/form';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { StyledFormWrapper } from '@/features/mapSideBar/shared/styles';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';

import { PropertyNetBookFormModel } from './models';

export interface IPropertyNetBookUpdateFormProps {
  isLoading: boolean;
  property: ApiGen_Concepts_Property | null;
  onSave: (apiModel: ApiGen_Concepts_Property) => Promise<void>;
}

export const PropertyNetBookUpdateForm = React.forwardRef<
  FormikProps<PropertyNetBookFormModel>,
  IPropertyNetBookUpdateFormProps
>(({ isLoading, property, onSave }, formikRef) => {
  const savePropertyNetBook = async (values: PropertyNetBookFormModel) => {
    await onSave(values.toApi());
  };

  return (
    <StyledFormWrapper>
      <StyledSummarySection>
        <LoadingBackdrop show={isLoading} />
        <Formik<PropertyNetBookFormModel>
          enableReinitialize
          innerRef={formikRef}
          initialValues={PropertyNetBookFormModel.fromApi(property)}
          onSubmit={savePropertyNetBook}
        >
          {formikProps => (
            <Section header="Net Book">
              <SectionField label="Net book value" contentWidth={{ xs: 4 }}>
                <FastCurrencyInput formikProps={formikProps} field="netBookAmount" />
              </SectionField>
              <SectionField label="Notes" labelWidth={{ xs: 12 }}>
                <MediumTextArea field="netBookNote" />
              </SectionField>
            </Section>
          )}
        </Formik>
      </StyledSummarySection>
    </StyledFormWrapper>
  );
});

const MediumTextArea = styled(TextArea)`
  textarea.form-control {
    min-width: 100%;
    height: 7rem;
    resize: none;
  }
`;

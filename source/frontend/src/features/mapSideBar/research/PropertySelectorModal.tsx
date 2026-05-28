import { FieldArray, Formik, FormikHelpers, FormikProps } from 'formik';
import { isEmpty } from 'lodash';
import React, { useRef } from 'react';
import styled from 'styled-components';

import { Form } from '@/components/common/form/Form';
import GenericModal from '@/components/common/GenericModal';
import OverflowTip from '@/components/common/OverflowTip';
import { StyledNoData } from '@/features/documents/commonStyles';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { getFilePropertyName } from '@/utils';
import { withNameSpace } from '@/utils/formUtils';

export interface IPropertySelectorModalProps {
  isOpened: boolean;
  availableProperties: ApiGen_Concepts_FileProperty[];
  onSelectOk: (selectedProperties: ApiGen_Concepts_FileProperty[]) => void;
  onCancelClick: () => void;
  isSingleSelect?: boolean;
  title?: string;
}

interface PropertySelectorForm {
  propertyIds: string[];
}

const PropertySelectorModal: React.FunctionComponent<
  React.PropsWithChildren<IPropertySelectorModalProps>
> = props => {
  const { isOpened, onCancelClick, availableProperties, onSelectOk, isSingleSelect } = props;

  const formikRef = useRef<FormikProps<PropertySelectorForm>>(null);

  const initialValues: PropertySelectorForm = {
    propertyIds: [],
  };

  const handleSelectProperties = (
    values: PropertySelectorForm,
    formikHelpers: FormikHelpers<PropertySelectorForm>,
  ) => {
    const ids = values.propertyIds;
    const selectedProperties = availableProperties.filter(x => ids.includes(x.id.toString()));

    onSelectOk(selectedProperties);
    formikHelpers.resetForm();
    formikHelpers.setSubmitting(false);
  };

  const selectType = isSingleSelect ? 'radio' : 'checkbox';

  return (
    <Formik<PropertySelectorForm>
      enableReinitialize
      innerRef={formikRef}
      initialValues={initialValues}
      onSubmit={handleSelectProperties}
    >
      {formikProps => (
        <StyledModal
          variant="info"
          display={isOpened}
          title={props.title ?? 'Generate Form 12'}
          message={
            <>
              <p>Select the properties that should be included in this form.</p>
              <p>
                <strong>Available properties in this file:</strong>
              </p>

              <StyledDiv>
                <FieldArray
                  name={withNameSpace('propertyIds')}
                  render={() => (
                    <Form.Group>
                      {availableProperties.map(
                        (propertyFile: ApiGen_Concepts_FileProperty, index: number) => (
                          <Form.Check
                            id={`propertyFile-${index}`}
                            type={selectType}
                            name="propertyIds"
                            key={propertyFile.id}
                          >
                            <Form.Check.Input
                              id={`propertyFile-${index}`}
                              type={selectType}
                              name="propertyIds"
                              value={propertyFile.id}
                              onChange={formikProps.handleChange}
                            />
                            <Form.Check.Label className="w-100" htmlFor={'recipient-' + index}>
                              <OverflowTip fullText={getFilePropertyName(propertyFile).value} />
                            </Form.Check.Label>
                          </Form.Check>
                        ),
                      )}
                    </Form.Group>
                  )}
                />
                {isEmpty(availableProperties) && (
                  <StyledNoData className="m-4">No Properties available</StyledNoData>
                )}
              </StyledDiv>
            </>
          }
          okButtonText="Continue"
          cancelButtonText="Cancel"
          handleOk={() => formikProps.submitForm()}
          handleCancel={() => {
            formikProps.resetForm();
            onCancelClick();
          }}
        ></StyledModal>
      )}
    </Formik>
  );
};

export default PropertySelectorModal;

const StyledModal = styled(GenericModal)`
  min-width: 70rem;

  .modal-body {
    padding-left: 2rem;
    padding-right: 2rem;
  }

  .modal-footer {
    padding-left: 2rem;
    padding-right: 2rem;
  }
`;

const StyledDiv = styled.div`
  border: 0.1rem solid ${props => props.theme.css.borderOutlineColor};
  border-radius: 0.5rem;
  max-height: 180px;
  overflow-y: auto;
  overflow-x: hidden;
  padding: 0.5rem 1.5rem;

  .form-check {
    input {
      margin-top: 0.6rem;
    }
  }

  .form-group {
    label {
      font-family: BcSans-Bold;
      line-height: 1.5rem;
      color: ${props => props.theme.bcTokens.typographyColorSecondary};

      span.type {
        font-size: 1.5rem;
        font-family: BCSans-Italic;
        font-style: italic;
        margin-left: 0.5rem;
        width: 100%;
      }
    }
  }
`;

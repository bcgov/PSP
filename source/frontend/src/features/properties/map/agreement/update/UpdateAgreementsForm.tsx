import { AxiosError } from 'axios';
import { Button, StyledRemoveLinkButton } from 'components/common/buttons';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import * as API from 'constants/API';
import { Section } from 'features/mapSideBar/tabs/Section';
import { FieldArray, Form, Formik, FormikProps } from 'formik';
import { useLookupCodeHelpers } from 'hooks/useLookupCodeHelpers';
import { IApiError } from 'interfaces/IApiError';
import { Api_Agreement } from 'models/api/Agreement';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaTrash } from 'react-icons/fa';
import { ILookupCode } from 'store/slices/lookupCodes';
import styled from 'styled-components';

import AgreementSubForm from './AgreementSubForm';
import { AgreementFormModelITEM, AgreementsFormModel } from './models';

export interface IUpdateAgreementsFormProps {
  isLoading: boolean;
  formikRef: React.Ref<FormikProps<AgreementsFormModel>>;
  initialValues: AgreementsFormModel;
  agreementTypes: ILookupCode[];
  onSave: (apiAcquisitionFile: AgreementsFormModel) => Promise<Api_Agreement[] | undefined>;
  onSuccess: (apiAcquisitionFile: Api_Agreement[]) => Promise<void>;
  onError: (e: AxiosError<IApiError>) => void;
}

export const UpdateAgreementsForm: React.FC<IUpdateAgreementsFormProps> = ({
  isLoading,
  formikRef,
  initialValues,
  agreementTypes,
  onSave,
  onSuccess,
  onError,
}) => {
  const { getByType, getOptionsByType } = useLookupCodeHelpers();
  const statusTypes = getOptionsByType(API.ACQUISITION_CHECKLIST_ITEM_STATUS_TYPES);

  //const arrayHelpersRef = useRef<FieldArrayRenderProps | null>(null);

  //const { values } = useFormikContext<AgreementsFormModel>();

  //const products: string[] = []; //values.products || [];

  /*const handleRemove = async (index: number) => {
    arrayHelpersRef.current?.remove(index);
  };*/

  const field = 'agreements';

  return (
    <StyledFormWrapper>
      <Formik<AgreementsFormModel>
        enableReinitialize
        innerRef={formikRef}
        initialValues={initialValues}
        onSubmit={async (values, formikHelpers) => {
          console.log(values);
          console.log(values.toApi());
          await onSave(values);
          /*try {
          const updatedFile = await onSave(values.toApi());
          /*if (!!updatedFile?.id) {
            formikHelpers.resetForm({
              values: AgreementsFormModel.fromApi(updatedFile, sectionTypes),
            });
            await onSuccess(updatedFile);
          }
        } catch (e) {
          if (axios.isAxiosError(e)) {
            const axiosError = e as AxiosError<IApiError>;
            onError && onError(axiosError);
          }
        } finally {
          formikHelpers.setSubmitting(false);
        }
      }*/
        }}
      >
        {formikProps => (
          <Form>
            <LoadingBackdrop show={isLoading}></LoadingBackdrop>

            <FieldArray
              name={field}
              render={arrayHelpers => {
                return (
                  <>
                    <Button
                      className="m-4"
                      onClick={() => arrayHelpers.push(new AgreementFormModelITEM())}
                    >
                      + Create new agreement
                    </Button>
                    {formikProps.values.agreements.map((product, index, array) => (
                      <Section
                        key={`agreement-edit-${index}`}
                        header={`Agreement ${index + 1}`}
                        isCollapsable
                        initiallyExpanded
                      >
                        <Row className="align-items-end pb-4">
                          <Col />
                          <Col xs="auto">
                            <StyledRemoveLinkButton
                              title="Delete Agreement"
                              variant="light"
                              onClick={() => arrayHelpers.remove(index)}
                            >
                              <FaTrash size="2rem" />
                            </StyledRemoveLinkButton>
                          </Col>
                        </Row>
                        <AgreementSubForm
                          index={index}
                          nameSpace={`${field}.${index}`}
                          formikProps={formikProps}
                          agreementTypes={agreementTypes}
                        />
                      </Section>
                    ))}
                  </>
                );
              }}
            ></FieldArray>
          </Form>
        )}
      </Formik>
    </StyledFormWrapper>
  );
};

const StyledFormWrapper = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-right: 1rem;
  padding-bottom: 1rem;
`;

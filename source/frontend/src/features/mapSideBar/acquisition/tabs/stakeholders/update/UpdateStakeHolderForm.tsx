import { FieldArray, Formik, FormikHelpers, FormikProps, getIn } from 'formik';
import { Fragment } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaTrash } from 'react-icons/fa';

import { Button, StyledRemoveLinkButton } from '@/components/common/buttons';
import { DisplayError } from '@/components/common/form';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import FilePropertiesTable from '@/components/filePropertiesTable/FilePropertiesTable';
import { StyledLink } from '@/components/maps/leaflet/LayerPopup/styles';
import { InterestHolderType } from '@/constants/interestHolderTypes';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';
import { exists } from '@/utils';

import { InterestHolderSubForm } from './InterestHolderSubForm';
import { InterestHolderForm, StakeHolderForm } from './models';
import { UpdateStakeHolderYupSchema } from './UpdateStakeHolderYupSchema';

export interface IUpdateStakeHolderFormProps {
  formikRef: React.Ref<FormikProps<StakeHolderForm>>;
  file: ApiGen_Concepts_AcquisitionFile;
  onSubmit: (
    interestHolders: StakeHolderForm,
    formikHelpers: FormikHelpers<StakeHolderForm>,
  ) => Promise<ApiGen_Concepts_InterestHolder[] | undefined>;
  interestHolders: StakeHolderForm;
  loading: boolean;
}

export const UpdateStakeHolderForm: React.FunctionComponent<IUpdateStakeHolderFormProps> = ({
  formikRef,
  file,
  onSubmit,
  interestHolders,
  loading,
}) => {
  const legacyStakeHolders: string[] = file.legacyStakeholders ?? [];

  return (
    <Formik<StakeHolderForm>
      enableReinitialize
      innerRef={formikRef}
      initialValues={interestHolders}
      validationSchema={UpdateStakeHolderYupSchema}
      onSubmit={async (values, formikHelpers) => {
        return onSubmit(values, formikHelpers);
      }}
    >
      {({ values, errors, setFieldValue }) => (
        <>
          <LoadingBackdrop parentScreen show={loading} />
          <StyledSummarySection>
            <Section header="Interests">
              <p>
                Interests will need to be in the{' '}
                <StyledLink to="/contact/list" target="_blank" rel="noopener noreferrer">
                  Contact Manager
                </StyledLink>{' '}
                in order to select them here.
              </p>
              <hr />
              <FieldArray
                name="interestHolders"
                render={arrayHelpers => (
                  <>
                    {values.interestHolders.length === 0 && legacyStakeHolders.length === 0 && (
                      <i>No Interest holders to display</i>
                    )}
                    {values.interestHolders.map((interestHolder, index) => (
                      <InterestHolderSubForm
                        key={`interest-holder-${interestHolder?.interestHolderId ?? index}`}
                        index={index}
                        errors={errors}
                        arrayHelpers={arrayHelpers}
                        file={file}
                        interestHolder={interestHolder}
                      ></InterestHolderSubForm>
                    ))}

                    {legacyStakeHolders.length > 0 && (
                      <Fragment>
                        <hr />
                        <SectionField
                          label="Legacy interest holders"
                          tooltip="This is read-only field to display legacy information"
                          labelWidth="4"
                          contentWidth="8"
                          valueTestId="acq-file-legacy-stakeholders"
                        >
                          {legacyStakeHolders.map((stakeholder, index) => (
                            <div key={index}>
                              <label key={index}>{stakeholder}</label>
                              {index < legacyStakeHolders.length - 1 && <br />}
                            </div>
                          ))}
                        </SectionField>
                      </Fragment>
                    )}
                    <hr />

                    <Button
                      variant="link"
                      onClick={() =>
                        arrayHelpers.push(
                          new InterestHolderForm(InterestHolderType.INTEREST_HOLDER, file.id),
                        )
                      }
                    >
                      + Add an Interest holder
                    </Button>
                  </>
                )}
              />
            </Section>
            <Section header="Non-interest Payees">
              <p>
                These are additional payees for the file who are not interest holders. (ex:
                construction for fences). Payees will need to be in the{' '}
                <StyledLink to="/contact/list" target="_blank" rel="noopener noreferrer">
                  Contact Manager
                </StyledLink>{' '}
                in order to select them here.
              </p>
              <hr />
              <FieldArray
                name="nonInterestPayees"
                render={arrayHelpers => (
                  <>
                    {values.nonInterestPayees.length === 0 && (
                      <i>No Non-interest payees to display</i>
                    )}
                    {values.nonInterestPayees.map((interestHolder, index) => (
                      <Fragment
                        key={
                          interestHolder?.interestHolderId
                            ? `non-interest-holder-${interestHolder?.interestHolderId}`
                            : `non-interest-holder-${index}`
                        }
                      >
                        <SectionField label="Payee name">
                          <Row>
                            <Col>
                              <ContactInputContainer
                                field={`nonInterestPayees.${index}.contact`}
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
                            {getIn(errors, `nonInterestPayees.${index}.contact`) && (
                              <DisplayError field={`nonInterestPayees.${index}.contact`} />
                            )}
                          </Row>
                        </SectionField>

                        <SectionField
                          label="Impacted properties"
                          tooltip="The non-interest payee will show on the Compensation Request form relevant to these properties."
                        >
                          <FilePropertiesTable
                            fileProperties={file.fileProperties ?? []}
                            selectedFileProperties={
                              interestHolder.impactedProperties
                                .map(ip =>
                                  file.fileProperties?.find(
                                    fp => fp.id === ip.acquisitionFilePropertyId,
                                  ),
                                )
                                .filter(exists) ?? []
                            }
                            setSelectedFileProperties={(
                              fileProperties: ApiGen_Concepts_FileProperty[],
                            ) => {
                              const interestHolderProperties = fileProperties.map(fileProperty => {
                                const matchingProperty = interestHolder.impactedProperties.find(
                                  ip => ip.acquisitionFilePropertyId === fileProperty.id,
                                );

                                return matchingProperty
                                  ? matchingProperty
                                  : {
                                      acquisitionFileProperty: fileProperty,
                                      acquisitionFilePropertyId: fileProperty.id,
                                      interestTypeCode: { id: 'NIP' },
                                    };
                              });
                              setFieldValue(
                                `nonInterestPayees.${index}.impactedProperties`,
                                interestHolderProperties,
                              );
                            }}
                            disabledSelection={false}
                          />
                        </SectionField>
                        {getIn(errors, `nonInterestPayees.${index}.impactedProperties`) && (
                          <DisplayError field={`nonInterestPayees.${index}.impactedProperties`} />
                        )}
                      </Fragment>
                    ))}
                    <hr />
                    <Button
                      variant="link"
                      onClick={() => {
                        const interestHolderForm = new InterestHolderForm(
                          InterestHolderType.INTEREST_HOLDER,
                          file.id,
                        );
                        interestHolderForm.propertyInterestTypeCode = 'NIP';
                        arrayHelpers.push(interestHolderForm);
                      }}
                    >
                      + Add a Non-interest payee
                    </Button>
                  </>
                )}
              />
            </Section>
          </StyledSummarySection>
        </>
      )}
    </Formik>
  );
};

export default UpdateStakeHolderForm;

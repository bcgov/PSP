import { Button } from 'components/common/buttons';
import { StyledRemoveLinkButton } from 'components/common/buttons';
import { DisplayError, Select } from 'components/common/form';
import { ContactInputContainer } from 'components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from 'components/common/form/ContactInput/ContactInputView';
import { StyledLink } from 'components/maps/leaflet/LayerPopup/styles';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import * as API from 'constants/API';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { StyledSummarySection } from 'features/mapSideBar/tabs/SectionStyles';
import { FieldArray, Formik, FormikProps, getIn } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { Api_InterestHolder } from 'models/api/InterestHolder';
import { Api_PropertyFile } from 'models/api/PropertyFile';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaTrash } from 'react-icons/fa';

import InterestHolderPropertiesTable from './InterestHolderPropertiesTable';
import { InterestHolderForm, StakeHolderForm } from './models';
import { UpdateStakeHolderYupSchema } from './UpdateStakeHolderYupSchema';

export interface IUpdateStakeHolderFormProps {
  formikRef: React.Ref<FormikProps<StakeHolderForm>>;
  file: Api_AcquisitionFile;
  onSubmit: (interestHolders: StakeHolderForm) => Promise<Api_InterestHolder[] | undefined>;
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
  const { getOptionsByType } = useLookupCodeHelpers();
  const interestHolderTypes = getOptionsByType(API.INTEREST_HOLDER_TYPES);
  return (
    <Formik<StakeHolderForm>
      enableReinitialize
      innerRef={formikRef}
      initialValues={interestHolders}
      validationSchema={UpdateStakeHolderYupSchema}
      onSubmit={async (values, formikHelpers) => {
        return onSubmit(values);
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
                    {values.interestHolders.length === 0 && <i>No Interest holders to display</i>}
                    {values.interestHolders.map((interestHolder, index) => (
                      <React.Fragment
                        key={
                          interestHolder?.interestHolderId
                            ? `interest-holder-${interestHolder?.interestHolderId}`
                            : `interest-holder-${index}`
                        }
                      >
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
                        <SectionField label="Interest type">
                          <Select
                            options={interestHolderTypes}
                            field={`interestHolders.${index}.interestTypeCode`}
                            placeholder="Select an interest type"
                          />
                        </SectionField>
                        <SectionField
                          label="Impacted properties"
                          tooltip="The interest holder will show on the Compensation Request form relevant to these properties."
                        >
                          <InterestHolderPropertiesTable
                            fileProperties={file.fileProperties ?? []}
                            selectedFileProperties={
                              (interestHolder.impactedProperties
                                .map(ip =>
                                  file.fileProperties?.find(
                                    fp => fp.id === ip.acquisitionFilePropertyId,
                                  ),
                                )
                                .filter(fp => !!fp) as Api_PropertyFile[]) ?? []
                            }
                            setSelectedFileProperties={(fileProperties: Api_PropertyFile[]) => {
                              const interestHolderProperties = fileProperties.map(fileProperty => {
                                const matchingProperty = interestHolder.impactedProperties.find(
                                  ip => ip.acquisitionFileProperty?.id === fileProperty.id,
                                );

                                return matchingProperty
                                  ? matchingProperty
                                  : {
                                      acquisitionFileProperty: fileProperty,
                                      acquisitionFilePropertyId: fileProperty.id,
                                    };
                              });
                              setFieldValue(
                                `interestHolders.${index}.impactedProperties`,
                                interestHolderProperties,
                              );
                            }}
                            disabledSelection={false}
                          />
                          {getIn(errors, `interestHolders.${index}.impactedProperties`) && (
                            <DisplayError field={`interestHolders.${index}.impactedProperties`} />
                          )}
                        </SectionField>
                      </React.Fragment>
                    ))}
                    <hr />
                    <Button
                      variant="link"
                      onClick={() => arrayHelpers.push(new InterestHolderForm(file.id))}
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
                      <React.Fragment
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
                          <InterestHolderPropertiesTable
                            fileProperties={file.fileProperties ?? []}
                            selectedFileProperties={
                              (interestHolder.impactedProperties
                                .map(ip =>
                                  file.fileProperties?.find(
                                    fp => fp.id === ip.acquisitionFilePropertyId,
                                  ),
                                )
                                .filter(fp => !!fp) as Api_PropertyFile[]) ?? []
                            }
                            setSelectedFileProperties={(fileProperties: Api_PropertyFile[]) => {
                              const interestHolderProperties = fileProperties.map(fileProperty => {
                                const matchingProperty = interestHolder.impactedProperties.find(
                                  ip => ip.acquisitionFileProperty?.id === fileProperty.id,
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
                      </React.Fragment>
                    ))}
                    <hr />
                    <Button
                      variant="link"
                      onClick={() => {
                        const interestHolderForm = new InterestHolderForm(file.id);
                        interestHolderForm.interestTypeCode = 'NIP';
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

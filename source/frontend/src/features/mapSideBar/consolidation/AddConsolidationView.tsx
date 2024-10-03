import { FieldArray, Formik, FormikHelpers, FormikProps } from 'formik';
import noop from 'lodash/noop';
import { useCallback } from 'react';
import { Tab } from 'react-bootstrap';
import { FaInfoCircle } from 'react-icons/fa';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';
import styled from 'styled-components';
import * as Yup from 'yup';

import SubdivisionIcon from '@/assets/images/subdivision-icon.svg?react';
import ConfirmNavigation from '@/components/common/ConfirmNavigation';
import { Form } from '@/components/common/form';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { H2 } from '@/components/common/styles';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { IMapSelectorContainerProps } from '@/components/propertySelector/MapSelectorContainer';
import { StyledTabView } from '@/components/propertySelector/PropertySelectorTabsView';
import { PropertySelectorPidSearchContainerProps } from '@/components/propertySelector/search/PropertySelectorPidSearchContainer';
import PropertySearchSelectorPidFormView from '@/components/propertySelector/search/PropertySelectorPidSearchView';
import { AreaUnitTypes } from '@/constants/areaUnitTypes';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { convertArea, exists } from '@/utils';

import MapSideBarLayout from '../layout/MapSideBarLayout';
import { AddressForm, PropertyForm } from '../shared/models';
import SelectedOperationProperty from '../shared/operations/SelectedOperationProperty';
import SelectedOperationPropertyHeader from '../shared/operations/SelectedOperationPropertyHeader';
import SidebarFooter from '../shared/SidebarFooter';
import AddConsolidationMarkerSynchronizer from './AddConsolidationMarkerSynchronizer';
import { ConsolidationFormModel } from './AddConsolidationModel';

export const AddConsolidationYupSchema = Yup.object().shape({
  pid: Yup.string()
    .nullable()
    .matches(/^\d{0,3}-\d{3}-\d{3}$|^\d{0,9}$/, 'Invalid PID'),
  sourceProperties: Yup.array().test({
    message: 'You must select at least two parent properties',
    test: arr => !!arr?.length && arr.length >= 2,
  }),
  destinationProperty: Yup.object().nullable().required('You must select a child property'),
});

export interface IAddConsolidationViewProps {
  formikRef: React.RefObject<FormikProps<ConsolidationFormModel>>;
  consolidationInitialValues: ConsolidationFormModel;
  loading: boolean;
  displayFormInvalid: boolean;
  onSubmit: (
    values: ConsolidationFormModel,
    formikHelpers: FormikHelpers<ConsolidationFormModel>,
  ) => void | Promise<any>;
  onCancel: () => void;
  onSave: () => void;
  getPrimaryAddressByPid: (pid: string) => Promise<AddressForm | undefined>;
  MapSelectorComponent: React.FunctionComponent<IMapSelectorContainerProps>;
  PropertySelectorPidSearchComponent: React.FunctionComponent<
    React.PropsWithChildren<PropertySelectorPidSearchContainerProps>
  >;
}

const AddConsolidationView: React.FunctionComponent<
  React.PropsWithChildren<IAddConsolidationViewProps>
> = ({
  formikRef,
  consolidationInitialValues,
  loading,
  displayFormInvalid,
  onSubmit,
  onSave,
  onCancel,
  getPrimaryAddressByPid,
  MapSelectorComponent,
  PropertySelectorPidSearchComponent,
}) => {
  const history = useHistory();

  const checkState = useCallback(() => {
    return formikRef?.current?.dirty && !formikRef?.current?.isSubmitting;
  }, [formikRef]);

  return (
    <MapSideBarLayout
      showCloseButton
      title="Create a Consolidation"
      icon={<StyledSubdivideConsolidateIcon />}
      onClose={onCancel}
      footer={
        <SidebarFooter
          isOkDisabled={loading}
          onSave={onSave}
          onCancel={onCancel}
          displayRequiredFieldError={displayFormInvalid}
          saveButtonLabel="Create Consolidation"
        />
      }
    >
      <StyledFormWrapper>
        <LoadingBackdrop show={loading} />
        <Formik<ConsolidationFormModel>
          onSubmit={onSubmit}
          initialValues={consolidationInitialValues}
          innerRef={formikRef}
          validationSchema={AddConsolidationYupSchema}
          validateOnBlur={false}
          validateOnChange={false}
        >
          {({ values, setFieldValue, errors }) => (
            <Form>
              <Section>
                <H2>
                  Properties in Consolidation
                  <TooltipWrapper
                    tooltipId="pims-only-consolidation-tooltip"
                    tooltip="Only properties that are in the PIMS inventory can be consolidated"
                  >
                    <FaInfoCircle className="tooltip-icon h-20" size="1rem" />
                  </TooltipWrapper>
                </H2>
                <AddConsolidationMarkerSynchronizer values={values} />
                <p>Select two or more parent properties that were consolidated:</p>
                <FieldArray name="sourceProperties">
                  {({ remove, push }) => (
                    <>
                      <StyledTabView activeKey="parent-property">
                        <Tab eventKey="parent-property" title="Parent Property Search">
                          <PropertySelectorPidSearchComponent
                            setSelectProperty={selectedProperty => push(selectedProperty)}
                            PropertySelectorPidSearchView={PropertySearchSelectorPidFormView}
                          />
                        </Tab>
                      </StyledTabView>
                      <Section header="Selected Parents" noPadding className="pt-4">
                        <SelectedOperationPropertyHeader />
                        {values.sourceProperties.map((property, index) => (
                          <SelectedOperationProperty
                            property={property}
                            onRemove={() => remove(index)}
                            nameSpace={`sourceProperties.${index}`}
                            getMarkerIndex={property => getDraftMarkerIndex(property, values)}
                            key={`destination-property-${property.pid}-${property.latitude}-${property.longitude}`}
                          />
                        ))}
                        {errors.sourceProperties && (
                          <div className="invalid-feedback">
                            {errors.sourceProperties as string}
                          </div>
                        )}
                      </Section>
                    </>
                  )}
                </FieldArray>
              </Section>
              <Section>
                <p>Select the child property to which parent properties were consolidated:</p>
                <MapSelectorComponent
                  addSelectedProperties={async properties => {
                    const allProperties: ApiGen_Concepts_Property[] = [];
                    await properties.reduce(async (promise, property) => {
                      return promise.then(async () => {
                        const formProperty = PropertyForm.fromFeatureDataset(property);
                        formProperty.landArea =
                          formProperty.landArea && formProperty.areaUnit
                            ? convertArea(
                                formProperty.landArea,
                                formProperty.areaUnit.toLocaleLowerCase(),
                                AreaUnitTypes.SquareMeters,
                              )
                            : 0;
                        formProperty.areaUnit = AreaUnitTypes.SquareMeters;
                        if (formProperty.pid) {
                          formProperty.address = await getPrimaryAddressByPid(formProperty.pid);
                          allProperties.push(formProperty.toApi());
                        } else {
                          toast.error('Selected property must have a PID');
                        }
                      });
                    }, Promise.resolve());
                    setFieldValue('destinationProperty', allProperties[0]);
                  }}
                  selectedComponentId="destination-property-selector"
                  modifiedProperties={[]}
                  repositionSelectedProperty={noop}
                />
                <Section header="Selected Child" noPadding className="pt-4">
                  <SelectedOperationPropertyHeader />
                  {exists(values.destinationProperty) && (
                    <SelectedOperationProperty
                      property={values.destinationProperty}
                      onRemove={() => setFieldValue('destinationProperty', undefined)}
                      nameSpace="destinationProperty"
                      getMarkerIndex={() => values.sourceProperties.length}
                      isEditable
                    />
                  )}
                  {errors.destinationProperty && (
                    <div className="invalid-feedback">{errors.destinationProperty as string}</div>
                  )}
                </Section>
              </Section>
            </Form>
          )}
        </Formik>
      </StyledFormWrapper>
      <ConfirmNavigation navigate={history.push} shouldBlockNavigation={checkState} />
    </MapSideBarLayout>
  );
};

const getDraftMarkerIndex = (
  property: ApiGen_Concepts_Property,
  form: ConsolidationFormModel,
): number => {
  const index = form.sourceProperties.findIndex(
    p =>
      p.latitude === property.latitude &&
      p.longitude === property.longitude &&
      p.pid === property.pid,
  );
  return index;
};

export default AddConsolidationView;

const StyledFormWrapper = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-right: 1rem;
  padding-bottom: 1rem;
  background-color: ${props => props.theme.css.highlightBackgroundColor};
`;

const StyledSubdivideConsolidateIcon = styled(SubdivisionIcon)`
  width: 3rem;
  height: 3rem;
  margin-right: 1rem;
  fill: ${props => props.theme.bcTokens.typographyColorSecondary};
`;

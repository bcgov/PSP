import { FieldArray, Formik, FormikHelpers, FormikProps } from 'formik';
import { Tab } from 'react-bootstrap';
import { FaInfoCircle } from 'react-icons/fa';
import { toast } from 'react-toastify';
import styled from 'styled-components';
import * as Yup from 'yup';

import ConsolidateSubdivideIcon from '@/assets/images/subdivisionconsolidation.svg?react';
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
import { convertArea } from '@/utils/convertUtils';

import MapSideBarLayout from '../layout/MapSideBarLayout';
import { AddressForm, PropertyForm } from '../shared/models';
import SelectedOperationProperty from '../shared/operations/SelectedOperationProperty';
import SelectedOperationPropertyHeader from '../shared/operations/SelectedOperationPropertyHeader';
import SidebarFooter from '../shared/SidebarFooter';
import AddSubdivisionMarkerSynchronizer from './AddSubdivisionMarkerSynchronizer';
import { SubdivisionFormModel } from './AddSubdivisionModel';

export const AddSubdivisionYupSchema = Yup.object().shape({
  pid: Yup.string()
    .nullable()
    .matches(/^\d{0,3}-\d{3}-\d{3}$|^\d{0,9}$/, 'Invalid PID'),
  destinationProperties: Yup.array().test({
    message: 'You must select at least two child properties',
    test: arr => !!arr?.length && arr.length >= 2,
  }),
  sourceProperty: Yup.object().nullable().required('You must select a parent property'),
});

export interface IAddSubdivisionViewProps {
  formikRef: React.RefObject<FormikProps<SubdivisionFormModel>>;
  subdivisionInitialValues: SubdivisionFormModel;
  loading: boolean;
  displayFormInvalid: boolean;
  onSubmit: (
    values: SubdivisionFormModel,
    formikHelpers: FormikHelpers<SubdivisionFormModel>,
  ) => void | Promise<any>;
  onCancel: () => void;
  onSave: () => void;
  getPrimaryAddressByPid: (pid: string) => Promise<AddressForm | undefined>;
  MapSelectorComponent: React.FunctionComponent<IMapSelectorContainerProps>;
  PropertySelectorPidSearchComponent: React.FunctionComponent<
    React.PropsWithChildren<PropertySelectorPidSearchContainerProps>
  >;
}

const AddSubdivisionView: React.FunctionComponent<
  React.PropsWithChildren<IAddSubdivisionViewProps>
> = ({
  formikRef,
  subdivisionInitialValues,
  loading,
  displayFormInvalid,
  onSubmit,
  onSave,
  onCancel,
  getPrimaryAddressByPid,
  MapSelectorComponent,
  PropertySelectorPidSearchComponent,
}) => {
  const getAreaValue = (area: number, unit: string): number => {
    const sqm = convertArea(area, unit, AreaUnitTypes.SquareMeters);
    return Number(sqm.toFixed(3));
  };
  return (
    <MapSideBarLayout
      showCloseButton
      title="Create a Subdivision"
      icon={<StyledSubdivideConsolidateIcon />}
      onClose={onCancel}
      footer={
        <SidebarFooter
          isOkDisabled={loading}
          onSave={onSave}
          onCancel={onCancel}
          displayRequiredFieldError={displayFormInvalid}
          saveButtonLabel="Create Subdivision"
        />
      }
    >
      <StyledFormWrapper>
        <LoadingBackdrop show={loading} />

        <Formik<SubdivisionFormModel>
          onSubmit={onSubmit}
          initialValues={subdivisionInitialValues}
          innerRef={formikRef}
          validationSchema={AddSubdivisionYupSchema}
          validateOnBlur={false}
          validateOnChange={false}
        >
          {({ values, setFieldValue, errors }) => (
            <Form>
              <Section>
                <H2>
                  Properties in Subdivision &nbsp;
                  <TooltipWrapper
                    tooltipId="pims-only-subdivision-tooltip"
                    tooltip="Only a property that is in the PIMS inventory can be subdivided"
                  >
                    <FaInfoCircle className="tooltip-icon h-20" size="1rem" />
                  </TooltipWrapper>
                </H2>
                <AddSubdivisionMarkerSynchronizer values={values} />
                <p>Select the parent property that was subdivided:</p>
                <StyledTabView activeKey="parent-property">
                  <Tab eventKey="parent-property" title="Parent Property Search">
                    <PropertySelectorPidSearchComponent
                      setSelectProperty={selectedProperty =>
                        setFieldValue('sourceProperty', selectedProperty)
                      }
                      PropertySelectorPidSearchView={PropertySearchSelectorPidFormView}
                    />
                  </Tab>
                </StyledTabView>
                <Section header="Selected Parent" noPadding className="pt-4">
                  <SelectedOperationPropertyHeader />
                  {values.sourceProperty?.pid && (
                    <SelectedOperationProperty
                      property={values.sourceProperty}
                      onRemove={() => setFieldValue('sourceProperty', undefined)}
                      nameSpace="sourceProperty"
                      getMarkerIndex={() => 0}
                    />
                  )}
                  {errors.destinationProperties && (
                    <div className="invalid-feedback">{errors.sourceProperty as string}</div>
                  )}
                </Section>
              </Section>
              <Section>
                <p>Select the child properties to which parent property was subdivided:</p>
                <MapSelectorComponent
                  addSelectedProperties={async properties => {
                    const allProperties = [...values.destinationProperties];
                    await properties.reduce(async (promise, property) => {
                      return promise.then(async () => {
                        const formProperty = PropertyForm.fromMapProperty(property);
                        formProperty.landArea =
                          property.landArea && property.areaUnit
                            ? getAreaValue(property.landArea, property.areaUnit)
                            : 0;
                        formProperty.areaUnit = AreaUnitTypes.SquareMeters;
                        if (property.pid) {
                          formProperty.address = await getPrimaryAddressByPid(property.pid);
                          allProperties.push(formProperty.toApi());
                        } else {
                          toast.error('Selected property must have a PID');
                        }
                      });
                    }, Promise.resolve());
                    setFieldValue('destinationProperties', allProperties);
                  }}
                  selectedComponentId="destination-property-selector"
                  modifiedProperties={values.destinationProperties.map(dp =>
                    PropertyForm.fromPropertyApi(dp),
                  )}
                />
                <FieldArray name="destinationProperties">
                  {({ remove }) => (
                    <Section header="Selected Children" noPadding className="pt-4">
                      <SelectedOperationPropertyHeader />
                      {values.destinationProperties.map((property, index) => (
                        <SelectedOperationProperty
                          property={property}
                          onRemove={() => remove(index)}
                          nameSpace={`destinationProperties.${index}`}
                          getMarkerIndex={property => getDraftMarkerIndex(property, values)}
                          key={`destination-property-${property.pid}-${property.latitude}-${property.longitude}`}
                          isEditable
                        />
                      ))}
                      {errors.destinationProperties && (
                        <div className="invalid-feedback">
                          {errors.destinationProperties as string}
                        </div>
                      )}
                    </Section>
                  )}
                </FieldArray>
              </Section>
            </Form>
          )}
        </Formik>
      </StyledFormWrapper>
    </MapSideBarLayout>
  );
};

const getDraftMarkerIndex = (
  property: ApiGen_Concepts_Property,
  form: SubdivisionFormModel,
): number => {
  let index = form.destinationProperties.findIndex(
    p =>
      p.latitude === property.latitude &&
      p.longitude === property.longitude &&
      p.pid === property.pid,
  );
  if (form.sourceProperty) {
    index++;
  }
  return index;
};

export default AddSubdivisionView;

const StyledFormWrapper = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-right: 1rem;
  padding-bottom: 1rem;
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;

const StyledSubdivideConsolidateIcon = styled(ConsolidateSubdivideIcon)`
  width: 3rem;
  height: 3rem;
  margin-right: 1rem;
  fill: ${props => props.theme.css.textColor};
`;

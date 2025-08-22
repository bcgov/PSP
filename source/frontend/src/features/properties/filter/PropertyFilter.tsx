import { Formik } from 'formik';
import { Feature, Geometry } from 'geojson';
import React, { useMemo } from 'react';
import { Row } from 'react-bootstrap';
import Col from 'react-bootstrap/Col';
import { toast } from 'react-toastify';

import { ResetButton, SearchButton } from '@/components/common/buttons';
import { Form, Input, Select } from '@/components/common/form';
import { getFeatureLatLng } from '@/components/maps/leaflet/Layers/PointClusterer';
import { TableSort } from '@/components/Table/TableSort';
import { IGeographicNamesProperties } from '@/hooks/pims-api/interfaces/IGeographicNamesProperties';
import { useGeocoderRepository } from '@/hooks/useGeocoderRepository';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { useTenant } from '@/tenants';
import { exists, pidFormatter } from '@/utils';

import { GeocoderAutoComplete } from '../components/GeocoderAutoComplete';
import { CoordinateSearchForm } from './CoordinateSearch/CoordinateSearchForm';
import { DmsCoordinates } from './CoordinateSearch/models';
import { GeographicNameInput } from './GeographicNameInput';
import { defaultPropertyFilter, IPropertyFilter } from './IPropertyFilter';
import { SearchToggleOption } from './PropertySearchToggle';
import { PropertyFilterValidationSchema } from './validation';

/**
 * PropertyFilter component properties.
 */
export interface IPropertyFilterProps {
  /** The default filter to apply if a different one hasn't been set. */
  defaultFilter: IPropertyFilter;
  /** The current property filter */
  propertyFilter: IPropertyFilter;
  /** Callback event when the filter is changed during Mount. */
  onChange: (filter: IPropertyFilter) => void;
  /** Comma separated list of column names to sort by. */
  sort?: TableSort<ApiGen_Concepts_Property>;
  /** Event fire when sorting changes. */
  onSorting?: (sort: TableSort<ApiGen_Concepts_Property>) => void;
  /** Which toggle view is currently active */
  toggle?: SearchToggleOption;
  /** Which toggle view is currently active */
  useGeocoder?: boolean;
}

/**
 * Property filter bar to search for properties.
 * Applied filter will be added to the URL query parameters and stored in a redux store.
 */
export const PropertyFilter: React.FC<React.PropsWithChildren<IPropertyFilterProps>> = ({
  defaultFilter,
  propertyFilter,
  onChange,
  toggle = SearchToggleOption.Map,
  useGeocoder,
}) => {
  const { getSitePids } = useGeocoderRepository();
  const { landTitleDistricts } = useTenant();

  const initialValues = useMemo(() => {
    const values = { ...defaultFilter, ...propertyFilter };
    return values;
  }, [defaultFilter, propertyFilter]);

  const changeFilter = (values: IPropertyFilter) => {
    onChange?.({ ...values });
  };

  const resetFilter = () => {
    changeFilter(defaultPropertyFilter);
  };

  const searchOptions = [
    { label: 'PID', value: 'pid' },
    { label: 'PIN', value: 'pin' },
    { label: 'Address', value: 'address' },
    { label: 'Plan #', value: 'planNumber' },
    {
      label: 'Historical File #',
      value: 'historical',
    },
  ];

  if (toggle === SearchToggleOption.Map) {
    searchOptions.push({
      label: 'POI Name',
      value: 'name',
    });
    searchOptions.push({
      label: 'Lat/Long',
      value: 'coordinates',
    });
    searchOptions.push({
      label: 'Survey Parcel',
      value: 'surveyParcel',
    });
  }

  return (
    <Formik<IPropertyFilter>
      initialValues={{ ...initialValues }}
      enableReinitialize
      validationSchema={PropertyFilterValidationSchema}
      onSubmit={(values, { setSubmitting }) => {
        setSubmitting(true);
        changeFilter(values);
        setSubmitting(false);
      }}
    >
      {({ isSubmitting, setFieldValue, values, resetForm, isValid }) => (
        <Form>
          <Row noGutters>
            <Col xs="12">
              <Select
                field="searchBy"
                options={searchOptions}
                className="idir-input-group"
                onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
                  setFieldValue('latitude', null);
                  setFieldValue('longitude', null);
                  setFieldValue('pid', null);
                  setFieldValue('pin', null);
                  setFieldValue('planNumber', null);
                  setFieldValue('historical', null);
                  setFieldValue('name', null);
                  setFieldValue('section', null);
                  setFieldValue('township', null);
                  setFieldValue('range', null);
                  setFieldValue('district', null);
                  if (e.target.value === 'coordinates') {
                    setFieldValue('coordinates', new DmsCoordinates());
                  } else {
                    setFieldValue('coordinates', null);
                  }
                }}
              />
            </Col>
            <Col xs="12">
              {values.searchBy === 'pid' && (
                <Input field="pid" placeholder="Enter a PID" displayErrorTooltips></Input>
              )}
              {values.searchBy === 'pin' && (
                <Input field="pin" placeholder="Enter a PIN" displayErrorTooltips></Input>
              )}
              {values.searchBy === 'address' && useGeocoder && (
                <GeocoderAutoComplete
                  data-testid="geocoder-mapview"
                  field="address"
                  placeholder="Enter an address"
                  onSelectionChanged={async val => {
                    const geocoderPidResponse = await getSitePids(val.siteId);
                    if (
                      geocoderPidResponse?.pids?.length === 1 &&
                      geocoderPidResponse?.pids[0] !== ''
                    ) {
                      setFieldValue('pid', geocoderPidResponse?.pids[0]);
                    } else {
                      if (geocoderPidResponse?.pids?.length > 1) {
                        toast.warn(
                          `Warning, multiple PIDs found for this address:\n ${geocoderPidResponse?.pids
                            .map(x => pidFormatter(x))
                            .join(
                              ',',
                            )} PIMS will search for the lat/lng of the property provided by geocoder instead of the PID.`,
                        );
                      } else {
                        toast.warn('No valid PIDs found for this address, using lat/long instead.');
                      }
                    }
                    setFieldValue('latitude', val.latitude);
                    setFieldValue('longitude', val.longitude);
                  }}
                  value={values.address}
                  autoSetting="off"
                ></GeocoderAutoComplete>
              )}
              {values.searchBy === 'address' && !useGeocoder && (
                <Input field="address" placeholder="Enter address"></Input>
              )}
              {values.searchBy === 'planNumber' && (
                <Input field="planNumber" placeholder="Enter a plan number"></Input>
              )}
              {values.searchBy === 'historical' && (
                <Input
                  field="historical"
                  placeholder="Enter a historical file# (LIS, PS, etc.)"
                ></Input>
              )}
              {values.searchBy === 'name' && (
                <GeographicNameInput
                  field="name"
                  placeholder='Enter a POI name (e.g. "Langford Lake")'
                  onSelectionChanged={(
                    selection: Feature<Geometry, IGeographicNamesProperties>,
                  ) => {
                    if (exists(selection.geometry)) {
                      const lngLat = getFeatureLatLng(selection);
                      setFieldValue('latitude', lngLat[1]);
                      setFieldValue('longitude', lngLat[0]);
                    }
                  }}
                />
              )}
            </Col>
            {values.searchBy === 'coordinates' && (
              <Col xs="12">
                <CoordinateSearchForm field="coordinates" innerClassName="flex-nowrap" />
              </Col>
            )}
            {values.searchBy === 'surveyParcel' && (
              <>
                <Row noGutters>
                  <Col>
                    <Select
                      field="district"
                      options={landTitleDistricts.map(ltd => ({
                        value: ltd,
                        label: ltd,
                      }))}
                    />
                  </Col>
                </Row>
                <Row noGutters>
                  <Col>
                    <Input placeholder="Section" field="section" displayErrorTooltips></Input>
                  </Col>
                </Row>
                <Row noGutters>
                  <Col>
                    <Input placeholder="Township" field="township" displayErrorTooltips></Input>
                  </Col>
                </Row>
                <Row noGutters>
                  <Col>
                    <Input placeholder="Range" field="range" displayErrorTooltips></Input>
                  </Col>
                </Row>
              </>
            )}
          </Row>
          <Row className="pt-2">
            <Col xs="auto">
              <SearchButton
                disabled={
                  isSubmitting ||
                  !(
                    values.pid ||
                    values.pin ||
                    values.latitude ||
                    values.longitude ||
                    values.planNumber ||
                    values.address ||
                    values.historical ||
                    values.township ||
                    values.section ||
                    values.range ||
                    values.district ||
                    (values.searchBy === 'coordinates' && isValid)
                  )
                }
              >
                Search
              </SearchButton>
            </Col>
            <Col xs="auto">
              <ResetButton
                disabled={isSubmitting}
                onClick={() => {
                  resetForm();
                  resetFilter();
                }}
              >
                Clear
              </ResetButton>
            </Col>
          </Row>
        </Form>
      )}
    </Formik>
  );
};

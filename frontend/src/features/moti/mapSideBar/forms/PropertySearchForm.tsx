import { ReactComponent as ParcelDraftIcon } from 'assets/images/draft-parcel-icon.svg';
import { Input } from 'components/common/form';
import SearchButton from 'components/common/form/SearchButton';
import { Label } from 'components/common/Label';
import TooltipIcon from 'components/common/TooltipIcon';
import TooltipWrapper from 'components/common/TooltipWrapper';
import { pidFormatter } from 'features/properties/components/forms/subforms/PidPinForm';
import { GeocoderAutoComplete } from 'features/properties/components/GeocoderAutoComplete';
import { Formik, getIn } from 'formik';
import { IGeocoderResponse } from 'hooks/useApi';
import { useState } from 'react';
import { Col, Form, Row } from 'react-bootstrap';
import ClickAwayListener from 'react-click-away-listener';
import styled from 'styled-components';

import { geocoderTooltip, markerSearchTooltip } from '../strings';

const SearchMarkerButton = styled.button`
  top: 20px;
  right: 20px;
  border: 0px;
  background-color: white;
`;

const SearchForm = styled(Row)`
  padding-bottom: 20px;
  padding-left: 30px;
  .form-row {
    margin: 0.5rem;
    justify-content: flex-end;
  }
  .label {
    margin: 0;
  }
  .form-group {
    margin: 0 0.5rem;
  }
  .form-row {
    flex-wrap: nowrap;
  }
  .btn {
    min-height: 20px;
  }
  .GeocoderAutoComplete {
    width: auto;
    .suggestionList {
      position: absolute;
      top: 40px;
      left: 10px;
    }
  }
  max-height: 135px;
`;

const SearchHeader = styled.h5`
  text-align: left;
  font-size: 22px;
`;

interface ISearchFormProps {
  /** used for determining nameSpace of field */
  nameSpace?: string;
  /** handle the population of Geocoder information */
  handleGeocoderChange: (data: IGeocoderResponse, nameSpace?: string) => Promise<void>;
  /** help set the cursor type when click the add marker button */
  setMovingPinNameSpace: (nameSpace?: string) => void;
  /** handle the pid formatting on change */
  handlePidChange: (pid: string, nameSpace?: string) => void;
  /** inner ref to underlying formik search form */
  formikRef: any;
}

interface IPropertySearchFields {
  searchPid: string;
  searchAddress: string;
}

/**
 * Search component which displays a vertically stacked set of search fields, used to find matching parcels within pims or the parcel layer.
 * @param {ISearchFormProps} param0
 */
export const PropertySearchForm = ({
  nameSpace,
  handleGeocoderChange,
  setMovingPinNameSpace,
  handlePidChange,
  formikRef,
  ...props
}: ISearchFormProps) => {
  const defaultValues: IPropertySearchFields = {
    searchPid: '',
    searchAddress: '',
  };
  const [geocoderResponse, setGeocoderResponse] = useState<IGeocoderResponse | undefined>();
  return (
    <Formik initialValues={defaultValues} innerRef={formikRef} onSubmit={() => {}}>
      {({ values, setFieldValue, errors, touched }) => (
        <SearchForm noGutters className="section">
          <Col md={12}>
            <SearchHeader>Search for a Property</SearchHeader>
          </Col>
          <Col md={5}>
            <Form.Row>
              <Label>PID</Label>
              <Input
                displayErrorTooltips
                disabled={false}
                pattern={RegExp(/^[\d\- ]*$/)}
                onBlurFormatter={(pid: string) => {
                  if (pid?.length > 0) {
                    return pid.replace(pid, pidFormatter(pid));
                  }
                  return '';
                }}
                field={'searchPid'}
              />
              <SearchButton
                className=""
                data-testid="pid-search-button"
                onClick={(e: any) => {
                  e.preventDefault();
                  handlePidChange(values.searchPid, nameSpace);
                }}
              />
            </Form.Row>
            <Form.Row>
              <Label className="d-flex flex-nowrap">
                Address&nbsp;
                <TooltipIcon toolTipId="geocoder-tooltip" toolTip={geocoderTooltip}></TooltipIcon>
              </Label>
              <GeocoderAutoComplete
                value={values.searchAddress}
                field={'searchAddress'}
                onSelectionChanged={selection => {
                  setFieldValue('searchAddress', selection.fullAddress);
                  setGeocoderResponse(selection);
                }}
                onTextChange={value => {
                  if (value !== geocoderResponse?.address1) {
                    setGeocoderResponse(undefined);
                  }
                  setFieldValue('searchAddress', value);
                }}
                error={getIn(errors, 'searchAddress')}
                touch={getIn(touched, 'searchAddress')}
                displayErrorTooltips
                autoSetting="off"
              />
              <SearchButton
                className=""
                data-testid="address-search-button"
                disabled={!geocoderResponse || !values.searchAddress}
                onClick={(e: any) => {
                  e.preventDefault();
                  geocoderResponse && handleGeocoderChange(geocoderResponse, nameSpace);
                }}
              />
            </Form.Row>
          </Col>
          <Col md={2}>
            <h5>OR</h5>
          </Col>
          <Col md={5} className="instruction">
            <p>Locate the property on the map.</p>
            <Row>
              <Col className="marker-svg">
                <ClickAwayListener
                  onClickAway={() => {
                    setMovingPinNameSpace(undefined);
                  }}
                >
                  <SearchMarkerButton
                    data-testid="land-search-marker"
                    type="button"
                    onClick={(e: any) => {
                      setMovingPinNameSpace(nameSpace ?? '');
                      e.preventDefault();
                    }}
                  >
                    <TooltipWrapper
                      toolTipId="property-marker-search-tooltip"
                      toolTip={markerSearchTooltip}
                    >
                      <ParcelDraftIcon className="parcel-icon" />
                    </TooltipWrapper>
                  </SearchMarkerButton>
                </ClickAwayListener>
              </Col>
            </Row>
          </Col>
        </SearchForm>
      )}
    </Formik>
  );
};

export default PropertySearchForm;

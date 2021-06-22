import { FastInput, Select } from 'components/common/form';
import { TypeaheadField } from 'components/common/form/Typeahead';
import { Label } from 'components/common/Label';
import * as API from 'constants/API';
import { FormikProps, getIn } from 'formik';
import { IGeocoderResponse } from 'hooks/useApi';
import { IAddress } from 'interfaces';
import _ from 'lodash';
import { useCallback } from 'react';
import React from 'react';
import { Form } from 'react-bootstrap';
import { useAppSelector } from 'store/hooks';
import { ILookupCode } from 'store/slices/lookupCodes';
import { mapLookupCode, postalCodeFormatter } from 'utils';

import { GeocoderAutoComplete } from '../../GeocoderAutoComplete';
import { streetAddressTooltip } from '../strings';

interface AddressProps {
  nameSpace?: string;
  disabled?: boolean;
  onGeocoderChange?: (data: IGeocoderResponse) => void;
  toolTips?: boolean;
  hideStreetAddress?: boolean;
  disableStreetAddress?: boolean;
  /** disable the green checkmark that appears beside the input on valid entry */
  disableCheckmark?: boolean;
}

export const defaultAddressValues: IAddress = {
  id: 0,
  line1: '',
  line2: undefined,
  administrativeArea: '',
  province: undefined,
  provinceId: 'BC',
  postal: '',
};
const AddressForm = <T extends any>(props: AddressProps & FormikProps<T>) => {
  const lookupCodes = useAppSelector(state => state.lookupCode.lookupCodes);
  const provinces = _.filter(lookupCodes, (lookupCode: ILookupCode) => {
    return lookupCode.type === API.PROVINCE_CODE_SET_NAME;
  }).map(mapLookupCode);
  const administrativeAreas = _.filter(lookupCodes, (lookupCode: ILookupCode) => {
    return lookupCode.type === API.ADMINISTRATIVE_AREA_CODE_SET_NAME;
  }).map(mapLookupCode);

  const withNameSpace: Function = useCallback(
    (fieldName: string) => {
      return props.nameSpace ? `${props.nameSpace}.${fieldName}` : fieldName;
    },
    [props.nameSpace],
  );

  const handleGeocoderChanges = (data: IGeocoderResponse) => {
    if (data && props.onGeocoderChange) {
      props.onGeocoderChange(data);
    }
  };

  return (
    <>
      {props.hideStreetAddress !== true && (
        <Form.Row>
          <Label>Street Address</Label>
          <GeocoderAutoComplete
            tooltip={props.toolTips ? streetAddressTooltip : undefined}
            value={getIn(props.values, withNameSpace('line1'))}
            disabled={props.disableStreetAddress || props.disabled}
            field={withNameSpace('line1')}
            onSelectionChanged={handleGeocoderChanges}
            onTextChange={value => props.setFieldValue(withNameSpace('line1'), value)}
            error={getIn(props.errors, withNameSpace('line1'))}
            touch={getIn(props.touched, withNameSpace('line1'))}
            displayErrorTooltips
            required={true}
          />
        </Form.Row>
      )}
      <Form.Row>
        <Label>Location</Label>
        <TypeaheadField
          options={administrativeAreas.map(x => x.label)}
          name={withNameSpace('administrativeArea')}
          disabled={props.disabled}
          hideValidation={props.disableCheckmark}
          paginate={false}
          required
          displayErrorTooltips
        />
      </Form.Row>
      <Form.Row>
        <Label>Province</Label>
        <Select
          disabled={true}
          placeholder="Must Select One"
          field={withNameSpace('provinceId')}
          options={provinces}
        />
      </Form.Row>
      <Form.Row className="postal">
        <Label>Postal Code</Label>
        <FastInput
          className="input-small"
          formikProps={props}
          style={{ width: '120px' }}
          disabled={props.disabled}
          onBlurFormatter={(postal: string) => postal.replace(postal, postalCodeFormatter(postal))}
          field={withNameSpace('postal')}
          displayErrorTooltips
        />
      </Form.Row>
    </>
  );
};

export default AddressForm;

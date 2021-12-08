import { Input, Select } from 'components/common/form';
import { CountryCodes } from 'constants/countryCodes';
import { Dictionary } from 'interfaces/Dictionary';
import React, { useCallback, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { ILookupCode } from 'store/slices/lookupCodes';
import { mapLookupCode } from 'utils';
import { withNameSpace } from 'utils/formUtils';

export interface IAddressProps {
  countries: ILookupCode[];
  provinceStates: ILookupCode[];
  namespace?: string;
}

/**
 * Displays addresses directly associated with this Contact Person.
 * @param {IAddressProps} param0
 */
export const Address: React.FunctionComponent<IAddressProps> = ({
  countries,
  provinceStates,
  namespace,
}) => {
  const [selectedCountry, setSelectedCountry] = useState(CountryCodes.Canada);

  const countryOptions = countries.map(c => mapLookupCode(c));
  const provinceOptions = provinceStates.map(c => mapLookupCode(c));

  const onCountryChanged = useCallback(
    (e: React.ChangeEvent<HTMLSelectElement>) => {
      const countryId = e.target.value;
      const countryCode = countries.find(c => c.id === countryId)?.code as CountryCodes;
      setSelectedCountry(countryCode);
    },
    [countries],
  );

  return (
    <>
      <Row>
        <Col md={8}>
          <Input field={withNameSpace(namespace, 'streetAddress1')} label="Address (line 1)" />
        </Col>
      </Row>
      <Row>
        <Col md={4}>
          <Select
            field={withNameSpace(namespace, 'countryId')}
            options={countryOptions}
            onChange={onCountryChanged}
          />
        </Col>
      </Row>
      <Row>
        <Col md={4}>
          <Input field="municipality" label="City" />
        </Col>
      </Row>
      <Row>
        <Col md={4}>
          <Select
            field={withNameSpace(namespace, 'provinceId')}
            options={provinceOptions}
            label={getUiLabel('province', selectedCountry)}
          />
        </Col>
      </Row>
      <Row>
        <Col md={4}>
          <Input
            field={withNameSpace(namespace, 'postal')}
            label={getUiLabel('postal', selectedCountry)}
          />
        </Col>
      </Row>
    </>
  );
};

const uiLabels = new Map<string, Dictionary<string>>([
  [CountryCodes.Canada, { province: 'Province', postal: 'Postal Code' }],
  [CountryCodes.US, { province: 'State', postal: 'Zip Code' }],
  [CountryCodes.Other, { province: '', postal: 'Postal Code' }],
]);

function getUiLabel(label: string, countryCode = CountryCodes.Canada) {
  const entry = uiLabels.get(countryCode) || {};
  return entry[label] || '';
}

export default Address;

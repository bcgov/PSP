import { Input, Select, SelectOption } from 'components/common/form';
import { CountryCodes } from 'constants/countryCodes';
import useAddressHelpers from 'features/contacts/hooks/useAddressHelpers';
import React, { useCallback, useMemo, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { mapLookupCode } from 'utils';
import { withNameSpace } from 'utils/formUtils';

export interface IAddressProps {
  namespace?: string;
}

/**
 * Displays addresses directly associated with this Contact Person.
 * @param {IAddressProps} param0
 */
export const Address: React.FunctionComponent<IAddressProps> = ({ namespace }) => {
  const { countries, provinces, getFieldLabel } = useAddressHelpers();
  const [selectedCountry, setSelectedCountry] = useState(CountryCodes.Canada);

  const countryOptions = useMemo(() => countries.map(c => mapLookupCode(c)), [countries]);
  const provinceOptions = useMemo(() => provinces.map(c => mapLookupCode(c)), [provinces]);

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
          <ProvinceOrCountryName
            namespace={namespace}
            selectedCountry={selectedCountry}
            provinceOptions={provinceOptions}
            getFieldLabel={getFieldLabel}
          />
        </Col>
      </Row>
      <Row>
        <Col md={4}>
          <Input
            field={withNameSpace(namespace, 'postal')}
            label={getFieldLabel('postal', selectedCountry)}
          />
        </Col>
      </Row>
    </>
  );
};

interface IProvinceOrCountryName {
  selectedCountry: CountryCodes;
  provinceOptions: SelectOption[];
  getFieldLabel: Function;
  namespace?: string;
}

const ProvinceOrCountryName: React.FunctionComponent<IProvinceOrCountryName> = ({
  selectedCountry,
  provinceOptions,
  getFieldLabel,
  namespace,
}) => {
  if (selectedCountry === CountryCodes.Other) {
    return <Input field={withNameSpace(namespace, 'countryOther')} label="Country name" />;
  }

  return (
    <Select
      field={withNameSpace(namespace, 'provinceId')}
      options={provinceOptions}
      label={getFieldLabel('province', selectedCountry)}
    />
  );
};

export default Address;

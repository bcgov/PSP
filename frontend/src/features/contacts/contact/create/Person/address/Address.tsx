import { Input, Select, SelectOption } from 'components/common/form';
import { CountryCodes } from 'constants/countryCodes';
import { getIn, useFormikContext } from 'formik';
import { Dictionary } from 'interfaces/Dictionary';
import { ICreatePersonForm } from 'interfaces/ICreateContact';
import React, { useCallback, useEffect } from 'react';
import { Col, Row } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { withNameSpace } from 'utils/formUtils';

import useAddressHelpers from './useAddressHelpers';

export interface IAddressProps {
  namespace?: string;
}

/**
 * Displays addresses directly associated with this Contact Person.
 * @param {IAddressProps} param0
 */
export const Address: React.FunctionComponent<IAddressProps> = ({ namespace }) => {
  const {
    defaultCountryId,
    countries,
    provinces,
    formLabels,
    selectedCountryCode,
    setSelectedCountryId,
  } = useAddressHelpers();

  const { values, setFieldValue } = useFormikContext<ICreatePersonForm>();
  const countryId = getIn(values, withNameSpace(namespace, 'countryId'));

  // set country to CANADA if none selected
  useEffect(() => {
    if (countryId === '') {
      setFieldValue(withNameSpace(namespace, 'countryId'), defaultCountryId ?? '');
      setFieldValue(withNameSpace(namespace, 'provinceId'), '');
      setSelectedCountryId(defaultCountryId ?? '');
    }
  }, [countryId, defaultCountryId, namespace, setFieldValue, setSelectedCountryId]);

  // clear associated fields (province, other country name) whenever country value is changed
  const onCountryChanged = useCallback(
    (e: React.ChangeEvent<HTMLSelectElement>) => {
      setSelectedCountryId(e.target.value);
      setFieldValue(withNameSpace(namespace, 'provinceId'), '');
      setFieldValue(withNameSpace(namespace, 'countryOther'), '');
    },
    [namespace, setFieldValue, setSelectedCountryId],
  );

  return (
    <>
      <Row>
        <Col md={8}>
          <Input field={withNameSpace(namespace, 'streetAddress1')} label="Address (line 1)" />
          <Link to="" onClick={() => {}}>
            + Add an address line
          </Link>
        </Col>
      </Row>
      <Row>
        <Col md={4}>
          <Select
            label="Country"
            field={withNameSpace(namespace, 'countryId')}
            options={countries}
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
            selectedCountry={selectedCountryCode}
            provinces={provinces}
            formLabels={formLabels}
          />
        </Col>
      </Row>
      <Row>
        <Col md={4}>
          <Input field={withNameSpace(namespace, 'postal')} label={formLabels.postal} />
        </Col>
      </Row>
    </>
  );
};

interface IProvinceOrCountryName {
  selectedCountry: string;
  provinces: SelectOption[];
  formLabels: Dictionary<string>;
  namespace?: string;
}

const ProvinceOrCountryName: React.FunctionComponent<IProvinceOrCountryName> = ({
  selectedCountry,
  provinces,
  formLabels,
  namespace,
}) => {
  if (selectedCountry === CountryCodes.Other) {
    return <Input field={withNameSpace(namespace, 'countryOther')} label="Country name" />;
  }

  return (
    <Select
      field={withNameSpace(namespace, 'provinceId')}
      options={provinces}
      label={formLabels.province}
      placeholder={formLabels.provincePlaceholder}
    />
  );
};

export default Address;

import { Button, Input, Select, SelectOption } from 'components/common/form';
import { Stack } from 'components/common/Stack/Stack';
import { CountryCodes } from 'constants/countryCodes';
import { getIn, useFormikContext } from 'formik';
import { Dictionary } from 'interfaces/Dictionary';
import { ICreatePersonForm } from 'interfaces/ICreateContact';
import React, { useCallback, useEffect } from 'react';
import { Col, Row } from 'react-bootstrap';
import { MdClose } from 'react-icons/md';
import { withNameSpace } from 'utils/formUtils';

import * as Styled from '../styles';
import useAddressHelpers from './useAddressHelpers';
import useCounter from './useCounter';

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

  // this counter determines how many address lines we render in the form; e.g. street1, street2, etc
  const { count, increment, decrement } = useCounter({ initial: 1, min: 1, max: 3 });

  // clear extra address fields when they get removed from address form...
  useEffect(() => {
    if (count < 3) {
      setFieldValue(withNameSpace(namespace, 'streetAddress3'), '');
    }
    if (count < 2) {
      setFieldValue(withNameSpace(namespace, 'streetAddress2'), '');
    }
  }, [count, namespace, setFieldValue]);

  return (
    <>
      <Row>
        <Col md={8}>
          <Input field={withNameSpace(namespace, 'streetAddress1')} label="Address (line 1)" />
          {count > 1 && (
            <Input field={withNameSpace(namespace, 'streetAddress2')} label="Address (line 2)" />
          )}
          {count > 2 && (
            <Input field={withNameSpace(namespace, 'streetAddress3')} label="Address (line 3)" />
          )}
        </Col>
        <Col style={{ paddingLeft: 0, paddingBottom: '2rem' }}>
          {count > 1 && (
            <Stack justifyContent="flex-end" className="h-100">
              <Styled.RemoveButton onClick={decrement}>
                <MdClose size="2rem" /> <span className="text">Remove</span>
              </Styled.RemoveButton>
            </Stack>
          )}
        </Col>
      </Row>
      {count < 3 && (
        <Row style={{ marginTop: '-1rem', marginBottom: '1rem' }}>
          <Col>
            <Button variant="link" onClick={increment}>
              + Add an address line
            </Button>
          </Col>
        </Row>
      )}
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

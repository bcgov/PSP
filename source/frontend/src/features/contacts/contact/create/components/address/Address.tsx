import { getIn, useFormikContext } from 'formik';
import React, { useCallback, useEffect } from 'react';
import { Col, Row } from 'react-bootstrap';
import { MdClose } from 'react-icons/md';

import { LinkButton, RemoveButton } from '@/components/common/buttons';
import { Input, Select, SelectOption } from '@/components/common/form';
import { Stack } from '@/components/common/Stack/Stack';
import { CountryCodes } from '@/constants/countryCodes';
import useCounter from '@/hooks/useCounter';
import { Dictionary } from '@/interfaces/Dictionary';
import { withNameSpace } from '@/utils/formUtils';

import useAddressHelpers from './useAddressHelpers';

// re-export helper hooks
export { useAddressHelpers };

export interface IAddressProps {
  namespace?: string;
  disabled?: boolean;
  addressLines?: number;
}

/**
 * Displays addresses directly associated with this Contact Person.
 * @param {IAddressProps} param0
 */
export const Address: React.FunctionComponent<React.PropsWithChildren<IAddressProps>> = ({
  namespace,
  disabled = false,
  addressLines,
}) => {
  const { countries, provinces, formLabels, selectedCountryCode, setSelectedCountryId } =
    useAddressHelpers();

  const { setFieldValue, values } = useFormikContext();
  const countryId = getIn(values, withNameSpace(namespace, 'countryId'));

  const line1Count = !!getIn(values, withNameSpace(namespace, 'streetAddress1')) ? 1 : 0;
  const line2Count = !!getIn(values, withNameSpace(namespace, 'streetAddress2')) ? 1 : 0;
  const line3Count = !!getIn(values, withNameSpace(namespace, 'streetAddress3')) ? 1 : 0;

  addressLines = addressLines ?? line1Count + line2Count + line3Count;

  useEffect(() => {
    setSelectedCountryId(countryId);
  }, [countryId, namespace, setFieldValue, setSelectedCountryId]);

  // clear associated fields (province, other country name) whenever country value is changed
  const onCountryChanged = useCallback(
    (e: React.ChangeEvent<HTMLSelectElement>) => {
      setFieldValue(withNameSpace(namespace, 'provinceId'), '');
      setFieldValue(withNameSpace(namespace, 'countryOther'), '');
    },
    [namespace, setFieldValue],
  );

  // this counter determines how many address lines we render in the form; e.g. street1, street2, etc
  const { count, increment, decrement } = useCounter({
    initial: addressLines || 1,
    min: 1,
    max: 3,
  });
  // clear extra address fields when they get removed from address form...
  const decrementFunction = () => {
    if (count === 3) {
      setFieldValue(withNameSpace(namespace, 'streetAddress3'), '');
    }
    if (count === 2) {
      setFieldValue(withNameSpace(namespace, 'streetAddress2'), '');
    }
    decrement();
  };

  return (
    <>
      <Row>
        <Col md={8}>
          <Input
            disabled={disabled}
            field={withNameSpace(namespace, 'streetAddress1')}
            label="Address (line 1)"
          />
          {count > 1 && (
            <Input
              disabled={disabled}
              field={withNameSpace(namespace, 'streetAddress2')}
              label="Address (line 2)"
            />
          )}
          {count > 2 && (
            <Input
              disabled={disabled}
              field={withNameSpace(namespace, 'streetAddress3')}
              label="Address (line 3)"
            />
          )}
        </Col>
        <Col style={{ paddingLeft: 0, paddingBottom: '2rem' }}>
          {count > 1 && !disabled && (
            <Stack justifyContent="flex-end" className="h-100">
              <RemoveButton onRemove={decrementFunction}>
                <MdClose size="2rem" /> <span className="text">Remove</span>
              </RemoveButton>
            </Stack>
          )}
        </Col>
      </Row>
      {count < 3 && !disabled && (
        <Row style={{ marginTop: '-1rem', marginBottom: '1rem' }}>
          <Col>
            <LinkButton onClick={increment}>+ Add an address line</LinkButton>
          </Col>
        </Row>
      )}
      <Row>
        <Col md={4}>
          <Select
            disabled={disabled}
            label="Country"
            field={withNameSpace(namespace, 'countryId')}
            options={countries}
            onChange={onCountryChanged}
            placeholder="Select..."
          />
        </Col>
      </Row>
      <Row>
        <Col md={4}>
          <Input
            disabled={disabled}
            field={withNameSpace(namespace, 'municipality')}
            label="City"
          />
        </Col>
      </Row>
      <Row>
        <Col md={4}>
          <ProvinceOrCountryName
            disabled={disabled}
            namespace={namespace}
            selectedCountry={selectedCountryCode}
            provinces={provinces}
            formLabels={formLabels}
          />
        </Col>
      </Row>
      <Row>
        <Col md={4}>
          <Input
            disabled={disabled}
            field={withNameSpace(namespace, 'postal')}
            label={formLabels.postal}
          />
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
  disabled?: boolean;
}

const ProvinceOrCountryName: React.FunctionComponent<
  React.PropsWithChildren<IProvinceOrCountryName>
> = ({ selectedCountry, provinces, formLabels, namespace, disabled = false }) => {
  if (selectedCountry === CountryCodes.Other) {
    return (
      <Input
        disabled={disabled}
        field={withNameSpace(namespace, 'countryOther')}
        label="Country name"
      />
    );
  }

  return (
    <Select
      disabled={disabled}
      field={withNameSpace(namespace, 'provinceId')}
      options={provinces}
      label={formLabels.province}
      placeholder={formLabels.provincePlaceholder}
    />
  );
};

export default Address;

import { getIn, useFormikContext } from 'formik';
import React, { useCallback, useEffect } from 'react';
import { Col, Row } from 'react-bootstrap';

import { LinkButton, RemoveButton } from '@/components/common/buttons';
import { Input, Select } from '@/components/common/form';
import { SectionField } from '@/components/common/Section/SectionField';
import { CountryCodes } from '@/constants/countryCodes';
import useCounter from '@/hooks/useCounter';
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

  const line1Count = getIn(values, withNameSpace(namespace, 'streetAddress1')) ? 1 : 0;
  const line2Count = getIn(values, withNameSpace(namespace, 'streetAddress2')) ? 1 : 0;
  const line3Count = getIn(values, withNameSpace(namespace, 'streetAddress3')) ? 1 : 0;

  addressLines = addressLines ?? line1Count + line2Count + line3Count;

  useEffect(() => {
    setSelectedCountryId(countryId);
  }, [countryId, namespace, setFieldValue, setSelectedCountryId]);

  // clear associated fields (province, other country name) whenever country value is changed
  const onCountryChanged = useCallback(() => {
    setFieldValue(withNameSpace(namespace, 'provinceId'), '');
    setFieldValue(withNameSpace(namespace, 'countryOther'), '');
  }, [namespace, setFieldValue]);

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
      <SectionField label="Address (line 1)">
        <Row>
          <Col md={9}>
            <Input disabled={disabled} field={withNameSpace(namespace, 'streetAddress1')} />
          </Col>
        </Row>
      </SectionField>
      {count > 1 && (
        <SectionField label="Address (line 2)">
          <Row>
            <Col md={9}>
              <Input disabled={disabled} field={withNameSpace(namespace, 'streetAddress2')} />
            </Col>
            <Col className="pl-0 pt-2">
              {count === 2 && !disabled && (
                <RemoveButton fontSize="1.3rem" onRemove={decrementFunction} />
              )}
            </Col>
          </Row>
        </SectionField>
      )}
      {count > 2 && (
        <SectionField label="Address (line 3)">
          <Row>
            <Col md={9}>
              <Input disabled={disabled} field={withNameSpace(namespace, 'streetAddress3')} />
            </Col>
            <Col className="pl-0 pt-2">
              {count === 3 && !disabled && (
                <RemoveButton fontSize="1.3rem" onRemove={decrementFunction} />
              )}
            </Col>
          </Row>
        </SectionField>
      )}
      {count < 3 && !disabled && (
        <SectionField label={null}>
          <Row style={{ marginTop: '-1rem', marginBottom: '1rem' }}>
            <Col>
              <LinkButton onClick={increment}>+ Add an address line</LinkButton>
            </Col>
          </Row>
        </SectionField>
      )}
      <SectionField label="Country" contentWidth="4">
        <Select
          disabled={disabled}
          field={withNameSpace(namespace, 'countryId')}
          options={countries}
          onChange={onCountryChanged}
          placeholder="Select..."
        />
      </SectionField>
      <SectionField label="City" contentWidth="4">
        <Input disabled={disabled} field={withNameSpace(namespace, 'municipality')} />
      </SectionField>
      {selectedCountryCode !== CountryCodes.Other && (
        <SectionField label={formLabels.province ?? 'Province'} contentWidth="4">
          <Select
            disabled={disabled}
            field={withNameSpace(namespace, 'provinceId')}
            options={provinces}
            placeholder={formLabels.provincePlaceholder}
          />
        </SectionField>
      )}
      {selectedCountryCode === CountryCodes.Other && (
        <SectionField label="Country name" contentWidth="4">
          <Input disabled={disabled} field={withNameSpace(namespace, 'countryOther')} />
        </SectionField>
      )}
      <SectionField label={formLabels.postal ?? 'Postal Code'} contentWidth="4">
        <Input disabled={disabled} field={withNameSpace(namespace, 'postal')} />
      </SectionField>
    </>
  );
};

export default Address;

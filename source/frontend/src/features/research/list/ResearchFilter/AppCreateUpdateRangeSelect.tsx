import { useFormikContext } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { FastDatePicker, InputGroup, Select } from '@/components/common/form';

interface IAppCreatedUpdated {
  createOrUpdateRange: string;
  createdOnEndDate: string;
  createdOnStartDate: string;
  updatedOnEndDate: string;
  updatedOnStartDate: string;
}

interface IAppCreateUpdateRangeSelectProps {
  disabled?: boolean;
  options?: { label: string; value: string }[];
  placeholders?: Record<string, string>;
}

const createUpdateTimestampOptions = [
  { label: 'Created date', value: 'createdOnStartDate' },
  { label: 'Updated date', value: 'updatedOnStartDate' },
];

/**
 * Provides a dropdown with list of search options for properties.
 */
export const AppCreateUpdateRangeSelect: React.FC<
  React.PropsWithChildren<IAppCreateUpdateRangeSelectProps & React.HTMLAttributes<HTMLElement>>
> = ({ disabled, options, placeholders, ...rest }) => {
  const state: {
    options: { label: string; value: string }[];
    placeholders: Record<string, string>;
  } = {
    options: options ?? createUpdateTimestampOptions,
    placeholders: placeholders ?? {
      appCreateTimestamp: 'from date',
      appLastUpdateTimestamp: 'from date',
    },
  };

  // access the form context values, no need to pass props
  const formikProps = useFormikContext<IAppCreatedUpdated>();
  const {
    values: { createOrUpdateRange },
    setFieldValue,
  } = formikProps;
  const desc = state.placeholders[createOrUpdateRange] || '';

  const reset = () => {
    setFieldValue(createOrUpdateRange ? createOrUpdateRange : 'appLastUpdateTimestamp', '');
  };

  return (
    <Row>
      <Col lg={12} xl={8}>
        <SmallInputGroup
          prepend={
            <Select
              field="createOrUpdateRange"
              options={state.options}
              onChange={reset}
              disabled={disabled}
            />
          }
          content={
            <SmallDatePicker
              formikProps={formikProps}
              field={createOrUpdateRange}
              placeholderText="from date"
            />
          }
          field={createOrUpdateRange}
          placeholder={desc}
          disabled={disabled}
          {...rest}
        ></SmallInputGroup>
      </Col>
      <Col lg={12} xl={4}>
        <SmallDatePicker
          formikProps={formikProps}
          field={
            createOrUpdateRange === 'createdOnStartDate' ? 'createdOnEndDate' : 'updatedOnEndDate'
          }
          placeholderText="to date"
        />
      </Col>
    </Row>
  );
};

const SmallInputGroup = styled(InputGroup)`
  .input-group-prepend select {
    width: 16rem;
  }
`;

const SmallDatePicker = styled(FastDatePicker)`
  width: 18rem;
  max-width: 100%;
`;

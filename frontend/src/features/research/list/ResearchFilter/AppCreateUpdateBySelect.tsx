import { InputGroup, Select } from 'components/common/form';
import { useFormikContext } from 'formik';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

interface IAppCreateUpdateBySelectProps {
  disabled?: boolean;
  options?: { label: string; value: string }[];
  placeholders?: Record<string, string>;
}

interface IAppCreatedUpdatedBy {
  createOrUpdateBy: string;
  appCreateUserid: string;
  appLastUpdateUserid: string;
}

const createUpdateUserOptions = [
  {
    label: 'Created by',
    value: 'appCreateUserid',
  },
  {
    label: 'Updated by',
    value: 'appLastUpdateUserid',
  },
];

/**
 * Provides a dropdown with list of search options for the user who created/updated the row.
 */
export const AppCreateUpdateBySelect: React.FC<IAppCreateUpdateBySelectProps &
  React.HTMLAttributes<HTMLElement>> = ({ disabled, options, placeholders, ...rest }) => {
  const state: {
    options: { label: string; value: string }[];
    placeholders: Record<string, string>;
  } = {
    options: options ?? createUpdateUserOptions,
    placeholders: placeholders ?? {
      appLastUpdateUserid: `User's IDIR`,
      appCreateUserid: `User's IDIR`,
    },
  };

  // access the form context values, no need to pass props
  const formikProps = useFormikContext<IAppCreatedUpdatedBy>();
  const {
    values: { createOrUpdateBy },
    setFieldValue,
  } = formikProps;
  const desc = state.placeholders[createOrUpdateBy] || '';

  const reset = () => {
    setFieldValue(createOrUpdateBy ? createOrUpdateBy : 'appLastUpdateUserid', '');
  };

  return (
    <Row>
      <Col xl="12">
        <SmallInputGroup
          formikProps={formikProps}
          prepend={
            <Select
              field="createOrUpdateBy"
              options={state.options}
              onChange={reset}
              disabled={disabled}
            />
          }
          field={createOrUpdateBy}
          placeholder={desc}
          disabled={disabled}
          {...rest}
        />
      </Col>
    </Row>
  );
};

const SmallInputGroup = styled(InputGroup)`
  .input-group-prepend select {
    width: 16rem;
  }
  input {
    width: 18rem;
    max-width: 100%;
  }
`;
export default AppCreateUpdateBySelect;

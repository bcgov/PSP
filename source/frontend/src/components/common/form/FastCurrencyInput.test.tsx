import { render } from '@testing-library/react';
import { Form, Formik } from 'formik';
import { noop } from 'lodash';
import React from 'react';

import { FastCurrencyInput } from './FastCurrencyInput';

describe('FastCurrencyInput', () => {
  it('fast currency input renders correctly', () => {
    const { asFragment } = render(
      <Formik initialValues={{ assessedLand: '' }} onSubmit={noop}>
        {props => (
          <Form>
            <FastCurrencyInput formikProps={props} field={'assessedLand'} tooltip={'Tooltip'} />
          </Form>
        )}
      </Formik>,
    );
    expect(asFragment()).toMatchSnapshot();
  });

  it('fast currency input should not show tooltip', () => {
    const { container } = render(
      <Formik initialValues={{ assessedLand: '' }} onSubmit={noop}>
        {props => (
          <Form>
            <FastCurrencyInput formikProps={props} field={'assessedLand'} />
          </Form>
        )}
      </Formik>,
    );

    expect(container.querySelector('svg[class="tooltip-icon"]')).toBeFalsy();
  });

  it('fast currency input should show tooltip', () => {
    const { container } = render(
      <Formik initialValues={{ assessedLand: '' }} onSubmit={noop}>
        {props => (
          <Form>
            <FastCurrencyInput formikProps={props} field={'assessedLand'} tooltip="Test tooltip" />
          </Form>
        )}
      </Formik>,
    );
    expect(container.querySelector('svg[class="tooltip-icon"]')).toBeInTheDocument();
  });

  it('fast currency input custom placeholder', async () => {
    const { findByPlaceholderText } = render(
      <Formik initialValues={{ assessedLand: '' }} onSubmit={noop}>
        {props => (
          <Form>
            <FastCurrencyInput
              formikProps={props}
              field={'assessedLand'}
              tooltip="Test tooltip"
              placeholder="custom placeholder"
            />
          </Form>
        )}
      </Formik>,
    );
    expect(await findByPlaceholderText('custom placeholder')).toBeInTheDocument();
  });
});

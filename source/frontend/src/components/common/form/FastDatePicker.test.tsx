import { act } from '@testing-library/react';
import { Formik } from 'formik';
import noop from 'lodash/noop';

import { fillInput, render } from '@/utils/test-utils';

import { FastDatePicker } from './FastDatePicker';

const testRender = (props?: any, formikProps?: any) =>
  render(
    <Formik initialValues={formikProps?.initialValues ?? {}} onSubmit={noop}>
      {formikProps => (
        <FastDatePicker
          field="test"
          {...{ ...(props ?? {}) }}
          formikProps={{
            ...{
              ...formikProps,
              initialValues: { test: '2020-12-31', ...formikProps.initialValues },
              values: { test: '2020-12-31', ...formikProps.values },
            },
          }}
        ></FastDatePicker>
      )}
    </Formik>,
  );

describe('fast date picker', () => {
  it('handles an empty value', async () => {
    const { container } = testRender(
      { oldDateWarning: true },
      { initialValues: { test: '' }, values: { test: '' } },
    );
    await act(async () => {
      await fillInput(container, 'test', '12/29/2020', 'datepicker');
    });
  });
  it('handles identical starting values and current value', async () => {
    const { container } = testRender({ oldDateWarning: true }, { values: { test: '2020-12-31' } });
    await act(async () => {
      await fillInput(container, 'test', '12/31/2020', 'datepicker');
    });
  });
});

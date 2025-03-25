import { Formik, FormikProps } from 'formik';
import { createRef } from 'react';

import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { INumberInputProps, NumberInput } from './';

const onFocus = vi.fn();
const onChange = vi.fn();

const validateFn = vi.fn(() => {
  return {};
});

describe('NumberInput component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & { props?: Partial<INumberInputProps> } = {}) => {
    const formikRef = createRef<FormikProps<{ test: number }>>();
    const utils = render(
      <Formik
        enableReinitialize
        innerRef={formikRef}
        initialValues={{ test: 0 }}
        onSubmit={vi.fn()}
        validate={validateFn}
      >
        <NumberInput
          label="Test Label"
          field="test"
          {...renderOptions?.props}
          onChange={onChange}
          onFocus={onFocus}
        />
      </Formik>,
      {
        ...renderOptions,
      },
    );

    return { ...utils, formikRef };
  };

  beforeEach(() => {});

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays existing values if they exist', async () => {
    const { getByRole, formikRef } = setup();

    await act(async () => {
      formikRef.current?.resetForm({ values: { test: 125 } });
    });

    const input = getByRole('spinbutton');
    expect(input).toBeVisible();
    expect(input).toHaveValue(125);
    expect(input.tagName).toBe('INPUT');
  });

  it('supports disabled property as intended', () => {
    const { getByRole } = setup({ props: { disabled: true } });

    const input = getByRole('spinbutton');
    expect(input).toBeVisible();
    expect(input.tagName).toBe('INPUT');
    expect(input).toHaveAttribute('disabled');
  });

  it('supports placeholder property as intended', () => {
    const { getByPlaceholderText } = setup({ props: { placeholder: 'type number here' } });

    const input = getByPlaceholderText('type number here');
    expect(input).toBeVisible();
  });

  it('displays associated help text if provided', () => {
    const help = 'This field is case-sensitive';
    const { getByText } = setup({ props: { helpText: help } });

    expect(getByText(help)).toBeVisible();
  });

  it('displays error text in a tooltip', async () => {
    validateFn.mockReturnValueOnce({ test: 'error message' });
    const { getByLabelText, formikRef } = setup();

    await act(async () => {
      formikRef.current?.submitForm();
    });
    await act(async () => {
      userEvent.hover(getByLabelText('Test Label'));
    });

    expect(screen.getByText('error message')).toBeVisible();
  });

  it('calls onChange callback when value changes', async () => {
    const { getByRole, formikRef } = setup();
    await act(async () => userEvent.paste(getByRole('spinbutton'), '999.99'));

    expect(onChange).toHaveBeenCalled();
    expect(formikRef.current?.values?.test).toBe(999.99);
  });

  it('calls onFocus callback when the component gets focus', async () => {
    const { getByRole } = setup();
    await act(async () => userEvent.click(getByRole('spinbutton')));

    expect(onFocus).toHaveBeenCalled();
  });
});

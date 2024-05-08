import { Formik } from 'formik';
import noop from 'lodash/noop';

import { act, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import { Input, InputProps } from './';

const BASIC_PROPS: InputProps = { field: 'foo' };
const FORMIK_VALUES = { foo: '' };

describe('Input component', () => {
  // render component under test
  const setup = (
    props: InputProps = BASIC_PROPS,
    initialValues: { foo?: string } = FORMIK_VALUES,
    renderOptions: RenderOptions = {},
  ) => {
    const { label = 'Test Label', ...rest } = props;
    const utils = render(
      <Formik initialValues={initialValues ?? {}} onSubmit={noop}>
        <Input label={label} {...rest} />
      </Formik>,
      {
        ...renderOptions,
      },
    );

    return { ...utils };
  };

  beforeEach(() => {});

  afterEach(() => {
    vi.resetAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays existing values if they exist', () => {
    const { getByRole } = setup(BASIC_PROPS, { foo: 'bar' });
    const input = getByRole('textbox');

    expect(input).toBeVisible();
    expect(input).toHaveValue('bar');
    expect(input.tagName).toBe('INPUT');
  });

  it('supports disabled property as intended', () => {
    const { getByRole } = setup({ ...BASIC_PROPS, disabled: true }, { foo: 'bar' });
    const input = getByRole('textbox');

    expect(input).toBeVisible();
    expect(input).toHaveValue('bar');
    expect(input.tagName).toBe('INPUT');
    expect(input).toHaveAttribute('disabled');
  });

  it('displays associated help text if provided', () => {
    const help = 'This field is case-sensitive';
    const { getByText } = setup({ ...BASIC_PROPS, helpText: help });

    expect(getByText(help)).toBeVisible();
  });

  it(`renders as TEXTAREA when property 'as' equals 'textarea'`, () => {
    const { getByRole } = setup({ ...BASIC_PROPS, as: 'textarea' }, { foo: 'bar' });
    const input = getByRole('textbox');

    expect(input).toBeVisible();
    expect(input).toHaveValue('bar');
    expect(input.tagName).toBe('TEXTAREA');
  });

  it('calls onChange callback when value changes', async () => {
    const onChangeSpy = vi.fn();
    const { getByRole } = setup({ ...BASIC_PROPS, onChange: onChangeSpy });

    // type some random data into text input
    await act(async () => userEvent.type(getByRole('textbox'), 'Lorem ipsum'));

    // assert
    await waitFor(() => expect(onChangeSpy).toHaveBeenCalled());
  });
});

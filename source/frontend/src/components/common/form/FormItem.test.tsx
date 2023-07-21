import { Field, Formik } from 'formik';
import noop from 'lodash/noop';

import { Button } from '@/components/common/buttons';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { FormItem } from './';

type RenderType = RenderOptions & { props?: { validate?: Function } };

describe('FormItem component', () => {
  const setup = async (renderOptions: RenderType = { props: { validate: () => undefined } }) => {
    const utils = render(
      <Formik
        initialValues={{ test: '' }}
        onSubmit={noop}
        validate={renderOptions.props?.validate as any}
      >
        <FormItem field="test">
          <Field name="test" data-testid="input" />
          <Button type="submit" data-testid="submit" />
        </FormItem>
      </Formik>,
      {
        ...renderOptions,
      },
    );

    return { ...utils };
  };

  beforeEach(() => {});

  afterEach(() => {});

  it('displays validation result', async () => {
    const validate = () => ({ test: 'error' });
    const { getByTestId, queryByText } = await setup({ props: { validate } });

    expect(queryByText('error')).not.toBeInTheDocument();
    await act(() => userEvent.paste(getByTestId('input'), 'foo bar baz'));
    await act(() => userEvent.click(getByTestId('submit')));

    expect(queryByText('error')).toBeInTheDocument();
    const containerDiv = getByTestId('input').parentElement!;
    expect(containerDiv).toHaveClass('is-invalid');
  });

  it('should not display error if no display is required', async () => {
    const validate = () => undefined;
    const { getByTestId } = await setup({ props: { validate } });

    await act(() => userEvent.paste(getByTestId('input'), 'foo bar baz'));
    await act(() => userEvent.click(getByTestId('submit')));

    const errorElement = getByTestId('input').parentElement!.nextSibling;
    expect(errorElement).toBeNull();
    const containerDiv = getByTestId('input').parentElement!;
    expect(containerDiv).not.toHaveClass('is-invalid');
  });
});

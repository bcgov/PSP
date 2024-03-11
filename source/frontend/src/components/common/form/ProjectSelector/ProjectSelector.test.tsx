import { Formik } from 'formik';
import noop from 'lodash/noop';

import { useProjectTypeahead } from '@/hooks/useProjectTypeahead';
import { act, render, RenderOptions, screen, userEvent, waitFor } from '@/utils/test-utils';

import { ProjectSelector } from './ProjectSelector';

jest.mock('@/hooks/useProjectTypeahead');
const mockUseProjectTypeahead = useProjectTypeahead as jest.MockedFunction<
  typeof useProjectTypeahead
>;

const handleTypeaheadSearch = jest.fn();
const onChange = jest.fn();

describe('ProjectSelector component', () => {
  // render component under test
  const setup = (initialValues: { foo: string }, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <Formik initialValues={initialValues ?? {}} onSubmit={noop}>
        <ProjectSelector field="foo" onChange={onChange} />
      </Formik>,
      {
        ...renderOptions,
      },
    );

    return {
      ...utils,
      // Finding elements
      getInput: () => {
        return screen.queryByRole('combobox');
      },
      findItems: async () => {
        return screen.findAllByRole('option');
      },
    };
  };

  let testForm: { foo: string };

  beforeEach(() => {
    testForm = { foo: '' };
    mockUseProjectTypeahead.mockReturnValue({
      handleTypeaheadSearch,
      isTypeaheadLoading: false,
      matchedProjects: [
        {
          id: 1,
          text: 'MOCK TEST PROJECT',
        },
      ],
    });
  });

  afterEach(() => {
    jest.resetAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup(testForm);
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays existing values if they exist', () => {
    const { getInput } = setup({ foo: 'bar' });
    const input = getInput();

    expect(input).toBeVisible();
    expect(input).toHaveValue('bar');
    expect(input?.tagName).toBe('INPUT');
  });

  it('makes request for matching projects', async () => {
    const { getInput } = setup(testForm);

    await act(async () => userEvent.type(getInput()!, 'test'));
    await waitFor(() => expect(handleTypeaheadSearch).toHaveBeenCalled());
  });

  it('shows matching projects based on user input', async () => {
    const { getInput, findItems } = setup(testForm);
    await act(async () => userEvent.type(getInput()!, 'test'));
    await waitFor(() => expect(handleTypeaheadSearch).toHaveBeenCalled());

    const items = await findItems();
    expect(items).toHaveLength(1);
    expect(items[0]).toHaveTextContent(/MOCK TEST PROJECT/i);
  });

  it('calls onChange callback when a project is selected', async () => {
    const { getInput, findItems } = setup(testForm);
    await act(async () => userEvent.type(getInput()!, 'test'));
    await waitFor(() => expect(handleTypeaheadSearch).toHaveBeenCalled());

    const items = await findItems();
    expect(items).toHaveLength(1);
    act(() => items[0].focus());
    await act(async () => {
      userEvent.keyboard('{Enter}');
    });
    expect(onChange).toHaveBeenCalled();
  });
});

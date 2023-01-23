import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { mockLookups } from 'mocks/mockLookups';
import { createRef } from 'react';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fakeText, render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

import { AddProjectYupSchema } from './AddProjectFileYupSchema';
import AddProjectForm, { IAddProjectFormProps } from './AddProjectForm';
import { ProjectForm } from './models';

const history = createMemoryHistory();
const validationSchema = jest.fn().mockReturnValue(AddProjectYupSchema);
const onSubmit = jest.fn();

type TestProps = Pick<IAddProjectFormProps, 'initialValues'>;
jest.mock('@react-keycloak/web');

describe('AddProjectForm component', () => {
  // render component under test
  const setup = (props: TestProps, renderOptions: RenderOptions = {}) => {
    const ref = createRef<FormikProps<ProjectForm>>();
    const utils = render(
      <AddProjectForm
        ref={ref}
        initialValues={props.initialValues}
        validationSchema={validationSchema}
        onSubmit={onSubmit}
        projectStatusOptions={[]}
        projectRegionOptions={[]}
      />,
      {
        ...renderOptions,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        claims: [],
        history,
      },
    );

    return {
      ...utils,
      getFormikRef: () => ref,
      getNameTextbox: () =>
        utils.container.querySelector(`input[name="projectName"]`) as HTMLInputElement,
      getNumberTextbox: () =>
        utils.container.querySelector(`input[name="projectNumber"]`) as HTMLInputElement,
      getRegionDropdown: () =>
        utils.container.querySelector(`select[name="region"]`) as HTMLSelectElement,
      getStatusDropdown: () =>
        utils.container.querySelector(`select[name="projectStatusType"]`) as HTMLSelectElement,
      getSummaryTextbox: () =>
        utils.container.querySelector(`textarea[name="summary"]`) as HTMLInputElement,
    };
  };

  let initialValues: ProjectForm;

  beforeEach(() => {
    initialValues = new ProjectForm();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({ initialValues });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders form fields as expected', async () => {
    const {
      getNameTextbox,
      getNumberTextbox,
      getStatusDropdown,
      getRegionDropdown,
      getSummaryTextbox,
    } = setup({ initialValues });

    const input = getNameTextbox();
    const number = getNumberTextbox();
    const select = getRegionDropdown();
    const status = getStatusDropdown();
    const summary = getSummaryTextbox();

    expect(input).toBeVisible();
    expect(select).toBeVisible();
    expect(number).toBeVisible();
    expect(status).toBeVisible();
    expect(summary).toBeVisible();

    expect(input.tagName).toBe('INPUT');
    expect(number.tagName).toBe('INPUT');
    expect(summary.tagName).toBe('TEXTAREA');
    expect(select.tagName).toBe('SELECT');
    expect(status.tagName).toBe('SELECT');
  });

  it('should validate character limits', async () => {
    const { getFormikRef, getNameTextbox, getNumberTextbox, getSummaryTextbox, findByText } = setup(
      {
        initialValues,
      },
    );

    // name cannot exceed 500 characters
    const nameInput = getNameTextbox();
    const numberInput = getNumberTextbox();
    const summayInput = getSummaryTextbox();
    await waitFor(() => userEvent.paste(nameInput, fakeText(201)));
    await waitFor(() => userEvent.paste(numberInput, fakeText(21)));
    await waitFor(() => userEvent.paste(summayInput, fakeText(2001)));

    // submit form to trigger validation check
    await waitFor(() => getFormikRef().current?.submitForm());

    expect(validationSchema).toBeCalled();
    expect(await findByText(/Project name must be at most 200 characters/i)).toBeVisible();
    expect(await findByText(/Project number must be at most 20 characters/i)).toBeVisible();
    expect(await findByText(/Project summary must be at most 2000 characters/i)).toBeVisible();
  });
});

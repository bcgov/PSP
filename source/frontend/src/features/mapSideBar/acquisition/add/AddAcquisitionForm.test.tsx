import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';

import { mockLookups } from '@/mocks/lookups.mock';
import { mockProjects } from '@/mocks/projects.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { fakeText, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import { AddAcquisitionFileYupSchema } from './AddAcquisitionFileYupSchema';
import { AddAcquisitionForm, IAddAcquisitionFormProps } from './AddAcquisitionForm';
import { AcquisitionForm } from './models';

const history = createMemoryHistory();

const validationSchema = jest.fn().mockReturnValue(AddAcquisitionFileYupSchema);
const onSubmit = jest.fn();

type TestProps = Pick<IAddAcquisitionFormProps, 'initialValues'>;
jest.mock('@react-keycloak/web');

describe('AddAcquisitionForm component', () => {
  // render component under test
  const setup = (props: TestProps, renderOptions: RenderOptions = {}) => {
    const formikRef = createRef<FormikProps<AcquisitionForm>>();
    const utils = render(
      <AddAcquisitionForm
        ref={formikRef}
        initialValues={props.initialValues}
        validationSchema={validationSchema}
        onSubmit={onSubmit}
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
      formikRef,
      getFormikRef: () => formikRef,
      getNameTextbox: () =>
        utils.container.querySelector(`input[name="fileName"]`) as HTMLInputElement,
      getRegionDropdown: () =>
        utils.container.querySelector(`select[name="region"]`) as HTMLSelectElement,
      getAcquisitionTypeDropdown: () =>
        utils.container.querySelector(`select[name="acquisitionType"]`) as HTMLSelectElement,
    };
  };

  let initialValues: AcquisitionForm;

  beforeEach(() => {
    initialValues = new AcquisitionForm();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({ initialValues });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders form fields as expected', () => {
    const { getByText, getNameTextbox, getRegionDropdown } = setup({ initialValues });

    const formSection = getByText(/Acquisition Details/i);
    const input = getNameTextbox();
    const select = getRegionDropdown();

    expect(formSection).toBeVisible();
    expect(input).toBeVisible();
    expect(input.tagName).toBe('INPUT');
    expect(select).toBeVisible();
    expect(select.tagName).toBe('SELECT');
  });

  it('displays existing values if they exist', async () => {
    initialValues.fileName = 'foo bar baz';
    const apiProject = mockProjects()[0];
    initialValues.project = { id: apiProject.id || 0, text: apiProject.description || '' };
    const { getNameTextbox } = setup({ initialValues });
    const input = getNameTextbox();

    expect(input).toBeVisible();
    expect(input).toHaveValue('foo bar baz');
  });

  it('should validate character limits', async () => {
    const { getFormikRef, findByText, getNameTextbox } = setup({ initialValues });

    // name cannot exceed 500 characters
    const nameInput = getNameTextbox();
    await waitFor(() => userEvent.paste(nameInput, fakeText(550)));

    // submit form to trigger validation check
    await waitFor(() => getFormikRef().current?.submitForm());

    expect(validationSchema).toBeCalled();
    expect(await findByText(/Acquisition file name must be at most 500 characters/i)).toBeVisible();
  });

  it('should display historical field input', async () => {
    const { getByText } = setup({ initialValues });
    expect(getByText(/Historical file number/i)).toBeVisible();
  });

  it('should display owner solicitor input', async () => {
    const { getByText } = setup({ initialValues });
    expect(getByText(/Owner solicitor/i)).toBeVisible();
  });

  it('should display owner representative input', async () => {
    const { getByText } = setup({ initialValues });
    expect(getByText(/Owner representative/i)).toBeVisible();
  });
});

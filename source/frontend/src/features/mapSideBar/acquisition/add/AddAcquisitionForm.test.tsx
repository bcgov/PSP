import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';
import { vi } from 'vitest';

import { useApiUsers } from '@/hooks/pims-api/useApiUsers';
import { mockLookups } from '@/mocks/lookups.mock';
import { mockProjects } from '@/mocks/projects.mock';
import { getMockPagedUsers, getUserMock } from '@/mocks/user.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  cleanup,
  fakeText,
  render,
  RenderOptions,
  screen,
  userEvent,
  waitFor,
  within,
} from '@/utils/test-utils';

import { AddAcquisitionFileYupSchema } from './AddAcquisitionFileYupSchema';
import { AddAcquisitionForm, IAddAcquisitionFormProps } from './AddAcquisitionForm';
import { AcquisitionForm } from './models';

const history = createMemoryHistory();

const validationSchema = vi.fn().mockReturnValue(AddAcquisitionFileYupSchema);
const onSubmit = vi.fn();

type TestProps = Pick<IAddAcquisitionFormProps, 'initialValues' | 'confirmBeforeAdd' | 'parentId'>;

vi.mock('@/hooks/pims-api/useApiUsers');
vi.mocked(useApiUsers).mockReturnValue({
  activateUser: vi.fn(),
  getUser: vi.fn().mockResolvedValue({ data: getUserMock() }),
  getUserInfo: vi.fn().mockResolvedValue({ data: getUserMock() }),
  getUsersPaged: vi.fn().mockResolvedValue({ data: getMockPagedUsers() }),
  putUser: vi.fn(),
  exportUsers: vi.fn(),
});

describe('AddAcquisitionForm component', () => {
  // render component under test
  const setup = (props: TestProps, renderOptions: RenderOptions = {}) => {
    const formikRef = createRef<FormikProps<AcquisitionForm>>();
    const utils = render(
      <AddAcquisitionForm
        ref={formikRef}
        initialValues={props.initialValues ?? new AcquisitionForm()}
        confirmBeforeAdd={props.confirmBeforeAdd ?? vi.fn()}
        parentId={props.parentId ?? undefined}
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
    vi.clearAllMocks();
    cleanup();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({ initialValues, confirmBeforeAdd: vi.fn() });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders form fields as expected', () => {
    const { getByText, getNameTextbox, getRegionDropdown } = setup({
      initialValues,
      confirmBeforeAdd: vi.fn(),
    });

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
    const { getNameTextbox } = setup({ initialValues, confirmBeforeAdd: vi.fn() });
    const input = getNameTextbox();

    expect(input).toBeVisible();
    expect(input).toHaveValue('foo bar baz');
  });

  it('should validate character limits', async () => {
    const { getFormikRef, findByText, getNameTextbox } = setup({
      initialValues,
      confirmBeforeAdd: vi.fn(),
    });

    // name cannot exceed 500 characters
    const nameInput = getNameTextbox();
    await waitFor(() => userEvent.paste(nameInput, fakeText(550)));

    // submit form to trigger validation check
    await waitFor(() => getFormikRef().current?.submitForm());

    expect(validationSchema).toBeCalled();
    expect(await findByText(/Acquisition file name must be at most 500 characters/i)).toBeVisible();
  });

  it('should display historical field input', async () => {
    const { getByText } = setup({ initialValues, confirmBeforeAdd: vi.fn() });
    expect(getByText(/Historical file number/i)).toBeVisible();
  });

  it('should display owner solicitor input', async () => {
    const { getByText } = setup({ initialValues, confirmBeforeAdd: vi.fn() });
    expect(getByText(/Owner solicitor/i)).toBeVisible();
  });

  it('should display owner representative input', async () => {
    const { getByText } = setup({ initialValues, confirmBeforeAdd: vi.fn() });
    expect(getByText(/Owner representative/i)).toBeVisible();
  });

  describe('Sub-interest files', () => {
    let parentId: number;
    beforeEach(() => {
      parentId = 99;
      initialValues.parentAcquisitionFileId = parentId;
      initialValues.formattedProject = '1111 - Test Project';
      initialValues.formattedProduct = '9999 Test Product';
    });

    it('renders as expected', () => {
      const { asFragment } = setup({ initialValues, parentId, confirmBeforeAdd: vi.fn() });
      expect(asFragment()).toMatchSnapshot();
    });

    it('should display interest solicitor input', async () => {
      const { getByText } = setup({ initialValues, parentId, confirmBeforeAdd: vi.fn() });
      expect(getByText(/Sub-interest solicitor/i)).toBeVisible();
    });

    it('should display interest representative input', async () => {
      const { getByText } = setup({ initialValues, parentId, confirmBeforeAdd: vi.fn() });
      expect(getByText(/Sub-interest representative/i)).toBeVisible();
    });

    it('should display project and product as read-only (with tooltip explaining why)', async () => {
      setup({ initialValues, parentId, confirmBeforeAdd: vi.fn() });

      expect(screen.getByText('1111 - Test Project')).toBeVisible();
      expect(screen.getByText('9999 Test Product')).toBeVisible();

      const project = screen.getByText('Ministry project:');
      const tooltipIcon = within(project).getByTestId('tooltip-icon-section-field-tooltip');
      await act(async () => userEvent.hover(tooltipIcon));

      expect(
        screen.getByText(
          /Sub-file has the same project as the main file and it can only be updated from the main file/i,
        ),
      ).toBeInTheDocument();
    });
  });
});

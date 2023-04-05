import { SelectOption } from 'components/common/form';
import * as API from 'constants/API';
import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { useUserInfoRepository } from 'hooks/repositories/useUserInfoRepository';
import { getMockLookUpsByType, mockLookups } from 'mocks/mockLookups';
import { getUserMock } from 'mocks/userMock';
import { createRef } from 'react';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, fakeText, fillInput, render, RenderOptions, userEvent } from 'utils/test-utils';

import { ProjectForm } from '../models';
import { AddProjectYupSchema } from './AddProjectFileYupSchema';
import AddProjectForm, { IAddProjectFormProps } from './AddProjectForm';

const history = createMemoryHistory();
const validationSchema = jest.fn().mockReturnValue(AddProjectYupSchema);
const onSubmit = jest.fn();

type TestProps = Pick<IAddProjectFormProps, 'initialValues'>;
jest.mock('@react-keycloak/web');

jest.mock('hooks/repositories/useUserInfoRepository');
(useUserInfoRepository as jest.MockedFunction<typeof useUserInfoRepository>).mockReturnValue({
  retrieveUserInfo: jest.fn(),
  retrieveUserInfoLoading: true,
  retrieveUserInfoResponse: {
    ...getUserMock(),
    userRegions: [
      {
        id: 1,
        userId: 5,
        regionCode: 1,
        region: { id: 1 },
      },
      {
        id: 2,
        userId: 5,
        regionCode: 2,
        region: { id: 2 },
      },
    ],
  },
});

const mockStatusOptions: SelectOption[] = getMockLookUpsByType(API.PROJECT_STATUS_TYPES);

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
        projectStatusOptions={mockStatusOptions}
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
      getProductCodeTextBox: (index: number) =>
        utils.container.querySelector(`input[name="products.${index}.code"]`) as HTMLInputElement,
    };
  };

  let initialValues: ProjectForm;

  beforeEach(() => {
    initialValues = new ProjectForm();
    initialValues.projectStatusType = 'AC';
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
    const { container, getFormikRef, findByText } = setup({
      initialValues,
    });

    await act(async () => {
      await fillInput(container, 'projectName', fakeText(201));
      await fillInput(container, 'projectNumber', fakeText(21));
      await fillInput(container, 'summary', fakeText(2001), 'textarea');
    });

    // submit form to trigger validation check
    await act(() => getFormikRef().current?.submitForm());

    expect(validationSchema).toBeCalled();
    expect(await findByText(/Project name must be at most 200 characters/i)).toBeVisible();
    expect(await findByText(/Project number must be at most 20 characters/i)).toBeVisible();
    expect(await findByText(/Project summary must be at most 2000 characters/i)).toBeVisible();
  });

  it('should call onSubmit and save form data as expected', async () => {
    const { getFormikRef, getNameTextbox, getRegionDropdown } = setup({
      initialValues,
    });

    await act(() => userEvent.selectOptions(getRegionDropdown(), '1'));
    await act(() => userEvent.paste(getNameTextbox(), `TRANS-CANADA HWY - 10`));

    // submit form to trigger validation check
    await act(() => getFormikRef().current?.submitForm());

    expect(onSubmit).toHaveBeenCalled();
  });

  it('should add a product', async () => {
    const { getByText, getProductCodeTextBox } = setup({
      initialValues,
    });

    const addProductButton = getByText('+ Add another product');
    await act(() => {
      userEvent.click(addProductButton);
    });

    const productCodeTextBox = getProductCodeTextBox(0);
    expect(productCodeTextBox).toBeVisible();
  });
});

import { Formik, FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { forwardRef, RefObject } from 'react';

import { useFinancialCodeRepository } from '@/hooks/repositories/useFinancialCodeRepository';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import { mockLookups } from '@/mocks/lookups.mock';
import { mockProjectPostResponse } from '@/mocks/projects.mock';
import { getUserMock } from '@/mocks/user.mock';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { toTypeCodeNullable } from '@/utils/formUtils';
import {
  act,
  createAxiosError,
  getMockRepositoryObj,
  render,
  RenderOptions,
  screen,
  userEvent,
} from '@/utils/test-utils';

import { ProjectForm } from '../models';
import AddProjectContainer, { IAddProjectContainerProps } from './AddProjectContainer';
import { IAddProjectFormProps } from './AddProjectForm';

const history = createMemoryHistory();

// Need to mock this library for unit tests
vi.mock('react-visibility-sensor', () => {
  return {
    default: vi.fn().mockImplementation(({ children }) => {
      if (children instanceof Function) {
        return children({ isVisible: true });
      }
      return children;
    }),
  };
});

const mockAddProjectApi = getMockRepositoryObj();

vi.mock('@/hooks/repositories/useProjectProvider');
vi.mocked(useProjectProvider, { partial: true }).mockReturnValue({
  addProject: mockAddProjectApi,
});

vi.mock('@/hooks/repositories/useFinancialCodeRepository');
vi.mocked(useFinancialCodeRepository, { partial: true }).mockReturnValue({
  getFinancialCodesByType: getMockRepositoryObj([]),
});

vi.mock('@/hooks/repositories/useUserInfoRepository');
vi.mocked(useUserInfoRepository).mockReturnValue({
  retrieveUserInfo: vi.fn(),
  retrieveUserInfoLoading: true,
  retrieveUserInfoResponse: {
    ...getUserMock(),
    userRegions: [
      {
        id: 1,
        userId: 5,
        regionCode: 1,
        region: toTypeCodeNullable(1),
        user: null,
        ...getEmptyBaseAudit(),
      },
      {
        id: 2,
        userId: 5,
        regionCode: 2,
        region: toTypeCodeNullable(2),
        user: null,
        ...getEmptyBaseAudit(),
      },
    ],
  },
});

let viewProps: IAddProjectFormProps | undefined;
let formikRef: RefObject<FormikProps<ProjectForm>> | undefined;

const TestView = forwardRef<FormikProps<ProjectForm>, IAddProjectFormProps>((props, ref) => {
  viewProps = props;
  formikRef = ref as any;

  return (
    <Formik<ProjectForm>
      enableReinitialize
      innerRef={ref}
      onSubmit={props.onSubmit}
      initialValues={props.initialValues}
    >
      {({ values }) => <>Content Rendered - {values.projectName}</>}
    </Formik>
  );
});

const onClose = vi.fn();
const onSuccess = vi.fn();

describe('AddProjectContainer component', () => {
  // render component under test
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IAddProjectContainerProps> } = {},
  ) => {
    const utils = render(
      <AddProjectContainer
        onClose={renderOptions?.props?.onClose ?? onClose}
        onSuccess={renderOptions?.props?.onSuccess ?? onSuccess}
        View={TestView}
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

    // wait for effects to settle
    await act(async () => {});

    return {
      ...utils,
    };
  };

  beforeEach(() => {
    viewProps = undefined;
    formikRef = undefined;
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the underlying form', async () => {
    const { getByText } = await setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('should save the form and navigate to details view when Save button is clicked', async () => {
    const formValues = new ProjectForm();
    formValues.projectName = 'TRANS-CANADA HWY - 10';
    formValues.projectNumber = '99999';
    formValues.region = 1;
    formValues.projectStatusType = 'AC';
    formValues.summary = 'NEW PROJECT';

    mockAddProjectApi.execute.mockResolvedValue(
      mockProjectPostResponse(
        1,
        2,
        'updated project name',
        'updated number',
        2,
        'AC',
        'updated summary',
      ),
    );

    await setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.setValues(formValues));
    await act(async () => formikRef.current?.submitForm());

    expect(mockAddProjectApi.execute).toHaveBeenCalledWith(formValues.toApi(), []);
    expect(onSuccess).toHaveBeenCalledWith(1);
  });

  it('displays server errors in a modal', async () => {
    mockAddProjectApi.execute.mockRejectedValue(
      createAxiosError(
        400,
        'Invalid Project management team, each team member can only be added once',
      ),
    );

    await setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    expect(
      await screen.findByText(
        'Invalid Project management team, each team member can only be added once',
      ),
    ).toBeVisible();

    await act(async () => userEvent.click(screen.getByTitle('ok-modal')));
    expect(onSuccess).not.toHaveBeenCalled();
  });
});

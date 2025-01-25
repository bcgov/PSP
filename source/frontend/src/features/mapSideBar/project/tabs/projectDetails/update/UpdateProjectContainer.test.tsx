import { Formik, FormikProps } from 'formik';
import { createRef, forwardRef } from 'react';

import { SideBarContextProvider } from '@/features/mapSideBar/context/sidebarContext';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import { mockProjectGetResponse, mockProjectPostResponse } from '@/mocks/projects.mock';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import {
  act,
  createAxiosError,
  getMockRepositoryObj,
  render,
  RenderOptions,
  screen,
  userEvent,
} from '@/utils/test-utils';

import { IAddProjectFormProps } from '../../../add/AddProjectForm';
import { ProjectForm } from '../../../models';
import UpdateProjectContainer from './UpdateProjectContainer';
import { useFinancialCodeRepository } from '@/hooks/repositories/useFinancialCodeRepository';

const mockUpdateProjectApi = getMockRepositoryObj();

vi.mock('@/hooks/repositories/useProjectProvider');
vi.mocked(useProjectProvider, { partial: true }).mockReturnValue({
  updateProject: mockUpdateProjectApi,
});

vi.mock('@/hooks/repositories/useFinancialCodeRepository');
vi.mocked(useFinancialCodeRepository, { partial: true }).mockReturnValue({
  getFinancialCodesByType: getMockRepositoryObj([]),
});

let viewProps: IAddProjectFormProps | undefined;

const TestView = forwardRef<FormikProps<ProjectForm>, IAddProjectFormProps>((props, formikRef) => {
  viewProps = props;

  return (
    <Formik<ProjectForm>
      enableReinitialize
      innerRef={formikRef}
      onSubmit={props.onSubmit}
      initialValues={props.initialValues}
    >
      {({ values }) => <>Content Rendered - {values.projectName}</>}
    </Formik>
  );
});

let mockProject: ApiGen_Concepts_Project;

const onSuccess = vi.fn();

describe('UpdateProjectContainer', () => {
  const setup = async (renderOptions: RenderOptions = {}) => {
    const formikRef = createRef<FormikProps<ProjectForm>>();
    const utils = render(
      <SideBarContextProvider project={mockProject}>
        <UpdateProjectContainer
          ref={formikRef}
          project={mockProject}
          View={TestView}
          onSuccess={onSuccess}
        />
      </SideBarContextProvider>,
      {
        ...renderOptions,
      },
    );

    // wait for effects to settle
    await act(async () => {});

    return { ...utils, formikRef };
  };

  beforeEach(() => {
    viewProps = undefined;
    mockProject = mockProjectGetResponse();
    vi.clearAllMocks();
  });

  it('renders the underlying form', async () => {
    const { getByText } = await setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('maps the ApiGen_Concepts_Project values to the form passed to the view', async () => {
    await setup();

    expect(viewProps.initialValues.id).toEqual(mockProject.id);
    expect(viewProps.initialValues.rowVersion).toEqual(mockProject.rowVersion);
    expect(viewProps.initialValues.projectName).toEqual(mockProject.description);
    expect(viewProps.initialValues.projectNumber).toEqual(mockProject.code);
    expect(viewProps.initialValues.region).toEqual(mockProject.regionCode?.id);
    expect(viewProps.initialValues.projectStatusType).toEqual(
      mockProject.projectStatusTypeCode?.id,
    );
    expect(viewProps.initialValues.summary).toEqual(mockProject.note);
  });

  it('makes request to update the Project', async () => {
    mockUpdateProjectApi.execute.mockResolvedValue(
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

    const { formikRef } = await setup();
    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    const apiProject = viewProps?.initialValues.toApi();
    expect(mockUpdateProjectApi.execute).toHaveBeenCalledWith(apiProject, []);
    expect(onSuccess).toHaveBeenCalled();
  });

  it('displays server errors in a modal', async () => {
    mockUpdateProjectApi.execute.mockRejectedValue(
      createAxiosError(
        400,
        'Invalid Project management team, each team member can only be added once',
      ),
    );
    const { formikRef } = await setup();

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

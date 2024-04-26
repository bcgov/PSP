import { FormikHelpers, FormikProps } from 'formik';

import { SideBarContextProvider } from '@/features/mapSideBar/context/sidebarContext';
import { mockProjectGetResponse, mockProjectPostResponse } from '@/mocks/projects.mock';
import { act, render, RenderOptions } from '@/utils/test-utils';

import { IAddProjectFormProps } from '../../../add/AddProjectForm';
import { ProjectForm } from '../../../models';
import UpdateProjectContainer from './UpdateProjectContainer';
import { forwardRef } from 'react';

const mockApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

vi.mock('@/hooks/repositories/useProjectProvider', () => ({
  useProjectProvider: () => {
    return {
      updateProject: mockApi,
    };
  },
}));

vi.mock('@/hooks/repositories/useFinancialCodeRepository', () => ({
  useFinancialCodeRepository: () => {
    return {
      getFinancialCodesByType: {
        error: undefined,
        response: [],
        execute: vi.fn(),
        loading: false,
      },
    };
  },
}));

let viewProps: IAddProjectFormProps | undefined;
const TestView = forwardRef<FormikProps<ProjectForm>, IAddProjectFormProps>((props, formikRef) => {
  viewProps = props;
  return <span>Content Rendered</span>;
});

const mockProject = mockProjectGetResponse();
const formValues: ProjectForm = ProjectForm.fromApi(mockProject);

const onSuccess = vi.fn();

describe('UpdateProjectContainer', () => {
  const setup = (renderOptions: RenderOptions = {}) => {
    const utils = render(
      <SideBarContextProvider project={mockProject}>
        <UpdateProjectContainer project={mockProject} View={TestView} onSuccess={onSuccess} />
      </SideBarContextProvider>,
      {
        ...renderOptions,
      },
    );
    return { ...utils };
  };

  beforeEach(() => {
    viewProps = {
      initialValues: formValues,
      projectStatusOptions: [],
      businessFunctionOptions: [],
      costTypeOptions: [],
      workActivityOptions: [],
      onSubmit: vi.fn(),
    };
    vi.resetAllMocks();
  });

  it('renders the underlying form', async () => {
    const { getByText } = setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
    await act(async () => {});
  });

  it('Maps the ApiGen_Concepts_Project values to the form passed to the view', () => {
    expect(formValues.id).toEqual(mockProject.id);
    expect(formValues.rowVersion).toEqual(mockProject.rowVersion);
    expect(formValues.projectName).toEqual(mockProject.description);
    expect(formValues.projectNumber).toEqual(mockProject.code);
    expect(formValues.region).toEqual(mockProject.regionCode?.id);
    expect(formValues.projectStatusType).toEqual(mockProject.projectStatusTypeCode?.id);
    expect(formValues.summary).toEqual(mockProject.note);
  });

  it('makes request to update the Project', async () => {
    setup();
    const formikHelpers: Partial<FormikHelpers<ProjectForm>> = {
      setSubmitting: vi.fn(),
      resetForm: vi.fn(),
    };

    mockApi.execute.mockResolvedValue(
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

    await act(async () => {
      return viewProps?.onSubmit(
        viewProps.initialValues,
        formikHelpers as FormikHelpers<ProjectForm>,
      );
    });

    expect(mockApi.execute).toHaveBeenCalled();
    expect(onSuccess).toHaveBeenCalled();
    expect(formikHelpers.setSubmitting).toHaveBeenCalled();
    expect(formikHelpers.resetForm).toHaveBeenCalled();
  });
});

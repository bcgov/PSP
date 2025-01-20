import { Formik, FormikProps } from 'formik';
import { createRef } from 'react';

import { mockLookups } from '@/mocks/index.mock';
import { getMockResearchFile, getMockResearchFileProperty } from '@/mocks/researchFile.mock';
import { ApiGen_Concepts_ResearchFileProperty } from '@/models/api/generated/ApiGen_Concepts_ResearchFileProperty';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions } from '@/utils/test-utils';

import { useUpdatePropertyResearch } from '../hooks/useUpdatePropertyResearch';
import { UpdatePropertyFormModel } from './models';
import { IUpdatePropertyResearchFormProps } from './UpdatePropertyForm';
import { UpdatePropertyResearchContainer } from './UpdatePropertyResearchContainer';

// mock API service calls

const mockPropertyResearchApi: ReturnType<typeof useUpdatePropertyResearch> = {
  updatePropertyResearchFile: vi.fn().mockResolvedValue(getMockResearchFile()),
};

vi.mock('../hooks/useUpdatePropertyResearch');
vi.mocked(useUpdatePropertyResearch).mockReturnValue(mockPropertyResearchApi);

let viewProps: IUpdatePropertyResearchFormProps | undefined;

const TestView: React.FC<IUpdatePropertyResearchFormProps> = props => {
  viewProps = props;
  return (
    <Formik<UpdatePropertyFormModel>
      enableReinitialize
      innerRef={props.formikRef}
      onSubmit={props.onSubmit}
      initialValues={props.initialValues}
    >
      {({ values }) => <>Content Rendered - {values.propertyName}</>}
    </Formik>
  );
};

describe('UpdatePropertyResearchContainer component', () => {
  let researchFileProperty: ApiGen_Concepts_ResearchFileProperty;
  const onSuccess = vi.fn();

  const setup = (renderOptions: RenderOptions = {}) => {
    const formikRef = createRef<FormikProps<UpdatePropertyFormModel>>();
    const utils = render(
      <UpdatePropertyResearchContainer
        ref={formikRef}
        researchFileProperty={researchFileProperty}
        onSuccess={onSuccess}
        View={TestView}
      />,
      {
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [],
        ...renderOptions,
      },
    );

    return {
      ...utils,
      formikRef,
    };
  };

  beforeEach(() => {
    viewProps = undefined;
    researchFileProperty = getMockResearchFileProperty();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders the underlying form', async () => {
    const { getByText } = setup();
    expect(getByText(/Content Rendered - Corner of Nakya PL/i)).toBeVisible();
  });

  it('calls the API to update the property research when form is saved', async () => {
    const { formikRef } = setup();

    expect(formikRef.current).not.toBeNull();
    await act(async () => formikRef.current?.submitForm());

    const apiResearchFileProperty = viewProps?.initialValues.toApi();
    expect(mockPropertyResearchApi.updatePropertyResearchFile).toHaveBeenCalledWith(
      apiResearchFileProperty,
    );
    expect(onSuccess).toHaveBeenCalled();
  });
});

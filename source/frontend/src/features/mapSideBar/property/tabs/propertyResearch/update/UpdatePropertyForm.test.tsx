import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';

import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_Concepts_ResearchFileProperty } from '@/models/api/generated/ApiGen_Concepts_ResearchFileProperty';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions } from '@/utils/test-utils';

import { UpdatePropertyFormModel } from './models';
import UpdatePropertyForm from './UpdatePropertyForm';

const testResearchFile: ApiGen_Concepts_ResearchFileProperty = {
  id: 1,
  propertyName: 'Corner of Nakya PL ',
  propertyId: 495,

  purposeTypes: [
    {
      id: 22,
      propertyPurposeType: {
        id: 'FORM12',
        description: 'Form 12',
        isDisabled: false,
        displayOrder: null,
      },
      propertyResearchFileId: 1,
      rowVersion: 1,
    },
    {
      id: 23,
      propertyPurposeType: {
        id: 'DOTHER',
        description: 'District Other',
        isDisabled: false,
        displayOrder: null,
      },
      propertyResearchFileId: 1,
      rowVersion: 1,
    },
  ],
  rowVersion: 10,
  isLegalOpinionRequired: null,
  isLegalOpinionObtained: null,
  documentReference: null,
  researchSummary: null,
  file: null,
  displayOrder: null,
  property: null,
  fileId: 0,
};

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('UpdatePropertyForm component', () => {
  const setup = (renderOptions: RenderOptions & { initialValues: UpdatePropertyFormModel }) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.initialValues}>
        {formikProps => <UpdatePropertyForm formikProps={formikProps} />}
      </Formik>,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      component,
    };
  };

  afterEach(() => {
    jest.resetAllMocks();
  });

  it('renders as expected when provided no research file', () => {
    const initialValues = UpdatePropertyFormModel.fromApi(testResearchFile);
    const { component } = setup({ initialValues });
    expect(component.asFragment()).toMatchSnapshot();
  });
});

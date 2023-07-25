import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';

import { mockLookups } from '@/mocks/lookups.mock';
import { Api_ResearchFileProperty } from '@/models/api/ResearchFile';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions } from '@/utils/test-utils';

import { UpdatePropertyFormModel } from './models';
import UpdatePropertyForm from './UpdatePropertyForm';

const testResearchFile: Api_ResearchFileProperty = {
  id: 1,
  propertyName: 'Corner of Nakya PL ',
  isDisabled: false,
  propertyId: 495,

  purposeTypes: [
    {
      id: 22,
      propertyPurposeType: {
        id: 'FORM12',
        description: 'Form 12',
        isDisabled: false,
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
      },
      propertyResearchFileId: 1,
      rowVersion: 1,
    },
  ],
  rowVersion: 10,
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
    var initialValues = UpdatePropertyFormModel.fromApi(testResearchFile);
    const { component } = setup({ initialValues });
    expect(component.asFragment()).toMatchSnapshot();
  });
});

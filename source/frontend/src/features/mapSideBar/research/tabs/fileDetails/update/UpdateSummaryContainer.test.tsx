import { FormikProps } from 'formik';
import UpdateResearchContainer, { IUpdateResearchViewProps } from './UpdateSummaryContainer';
import { createMemoryHistory } from 'history';

import { Claims } from '@/constants';

import { render, RenderOptions } from '@/utils/test-utils';
import { getMockResearchFile } from '@/mocks/researchFile.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { createRef } from 'react';

const mockResearchFileApi = getMockResearchFile();
const history = createMemoryHistory();

const onSuccess = vi.fn();

describe('Research File Update Container', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IUpdateResearchViewProps>;
    } = {},
  ) => {
    const formikRef = createRef<FormikProps<IUpdateResearchViewProps>>();

    const component = render(
      <UpdateResearchContainer
        researchFile={renderOptions?.props?.researchFile ?? mockResearchFileApi}
        onSuccess={onSuccess}
        ref={formikRef}
      />,
      {
        history,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [Claims.ACQUISITION_EDIT],
        ...renderOptions,
      },
    );

    return {
      ...component,
      getFormikRef: () => formikRef,
    };
  };

  beforeEach(() => {
    vi.resetAllMocks();
  });

  it('Renders the underlying form', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });
});

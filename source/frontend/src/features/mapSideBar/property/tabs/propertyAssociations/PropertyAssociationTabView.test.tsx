import { createMemoryHistory } from 'history';

import { ApiGen_Concepts_PropertyAssociations } from '@/models/api/generated/ApiGen_Concepts_PropertyAssociations';
import { render, RenderOptions } from '@/utils/test-utils';

import PropertyAssociationTabView, {
  IPropertyAssociationTabViewProps,
} from './PropertyAssociationTabView';

const history = createMemoryHistory();

const fakeAssociations: ApiGen_Concepts_PropertyAssociations = {
  id: 168,
  pid: '26934426',
  acquisitionAssociations: [
    {
      id: 45,
      fileNumber: '95154',
      fileName: '-',
      createdDateTime: '2022-05-13T11:51:29.23',
      createdBy: 'Acquisition File Data',
      status: 'Active',
      createdByGuid: null,
    },
  ],
  leaseAssociations: [
    {
      id: 34,
      fileNumber: '951547254',
      fileName: '-',
      createdDateTime: '2022-05-13T11:51:29.23',
      createdBy: 'Lease Seed Data',
      status: 'Active',
      createdByGuid: null,
    },
  ],
  researchAssociations: [
    {
      id: 1,
      fileNumber: 'R-1',
      fileName: 'R-1',
      createdDateTime: '2022-05-17T19:49:16.647',
      createdBy: 'admin',
      status: 'Active',
      createdByGuid: null,
    },
  ],
  dispositionAssociations: null,
};

describe('PropertyAssociationTabView component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & IPropertyAssociationTabViewProps) => {
    const component = render(
      <PropertyAssociationTabView
        isLoading={renderOptions.isLoading}
        associations={renderOptions.associations}
      />,
      {
        history,
      },
    );

    return {
      ...component,
    };
  };

  it('renders as expected when provided valid data object', () => {
    const { asFragment } = setup({ isLoading: false, associations: fakeAssociations });
    expect(asFragment()).toMatchSnapshot();
  });
});

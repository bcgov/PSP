import { createMemoryHistory } from 'history';
import { Api_ResearchFileProperty } from 'models/api/ResearchFile';
import { render, RenderOptions } from 'utils/test-utils';

import PropertyResearchTabView, { IPropertyResearchTabViewProps } from './PropertyResearchTabView';

const history = createMemoryHistory();

const setEditMode = jest.fn();

describe('PropertyResearchTabView component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & IPropertyResearchTabViewProps) => {
    const component = render(
      <PropertyResearchTabView
        researchFile={renderOptions.researchFile}
        setEditMode={renderOptions.setEditMode}
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
    const { asFragment } = setup({ researchFile: fakePropertyResearch, setEditMode });
    expect(asFragment()).toMatchSnapshot();
  });
});

const fakePropertyResearch: Api_ResearchFileProperty = {
  id: 0,
  isDisabled: false,

  property: {},
  researchFile: {},
  propertyName: 'The Research Property name',
  isLegalOpinionRequired: true,
  isLegalOpinionObtained: true,
  documentReference: 'A document reference',
  researchSummary: 'Research summary notes',
  purposeTypes: [
    { propertyPurposeType: { id: 'TYPE_A', description: 'Type A' } },
    { propertyPurposeType: { id: 'TYPE_B', description: 'Type B' } },
  ],
};

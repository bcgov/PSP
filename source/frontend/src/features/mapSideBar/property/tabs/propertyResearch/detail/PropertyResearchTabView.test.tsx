import { createMemoryHistory } from 'history';

import { Claims } from '@/constants/index';
import { ApiGen_Concepts_ResearchFileProperty } from '@/models/api/generated/ApiGen_Concepts_ResearchFileProperty';
import {
  getEmptyBaseAudit,
  getEmptyProperty,
  getEmptyResearchFile,
} from '@/models/defaultInitializers';
import { render, RenderOptions } from '@/utils/test-utils';

import PropertyResearchTabView, { IPropertyResearchTabViewProps } from './PropertyResearchTabView';

const history = createMemoryHistory();

const setEditMode = vi.fn();

// mock keycloak auth library

describe('PropertyResearchTabView component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & IPropertyResearchTabViewProps) => {
    const component = render(
      <PropertyResearchTabView
        researchFileProperty={renderOptions.researchFileProperty}
        setEditMode={renderOptions.setEditMode}
      />,
      {
        history,
        claims: [Claims.RESEARCH_EDIT],
      },
    );

    return {
      ...component,
    };
  };

  it('renders as expected when provided valid data object', () => {
    const { asFragment } = setup({ researchFileProperty: fakePropertyResearch, setEditMode });
    expect(asFragment()).toMatchSnapshot();
  });
});

const fakePropertyResearch: ApiGen_Concepts_ResearchFileProperty = {
  id: 0,
  propertyId: 1,
  property: getEmptyProperty(),
  file: getEmptyResearchFile(),
  propertyName: 'The Research Property name',
  isLegalOpinionRequired: true,
  isLegalOpinionObtained: true,
  documentReference: 'A document reference',
  researchSummary: 'Research summary notes',
  propertyResearchPurposeTypes: [
    {
      propertyResearchPurposeTypeCode: {
        id: 'TYPE_A',
        description: 'Type A',
        displayOrder: null,
        isDisabled: false,
      },
      id: 0,
      rowVersion: null,
      ...getEmptyBaseAudit(),
    },
    {
      propertyResearchPurposeTypeCode: {
        id: 'TYPE_B',
        description: 'Type B',
        displayOrder: null,
        isDisabled: false,
      },
      id: 0,
      rowVersion: null,
      ...getEmptyBaseAudit(),
    },
  ],
  fileId: 0,
  displayOrder: null,
  location: null,
  rowVersion: null,
};

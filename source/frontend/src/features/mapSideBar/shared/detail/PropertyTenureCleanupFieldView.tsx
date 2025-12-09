import { ColProps } from 'react-bootstrap';
import styled from 'styled-components';

import ExpandableTextList from '@/components/common/ExpandableTextList';
import { ApiGen_Concepts_PropertyTenureCleanup } from '@/models/api/generated/ApiGen_Concepts_PropertyTenureCleanup';

export interface ITenureCleanupViewProps {
  tenureCleanups: ApiGen_Concepts_PropertyTenureCleanup[];
  labelWidth?: ColProps;
  contentWidth?: ColProps;
}

export const TenureCleanupFieldView: React.FC<ITenureCleanupViewProps> = ({ tenureCleanups }) => {
  return (
    <ExpandableTextList<ApiGen_Concepts_PropertyTenureCleanup>
      moreText="more categories..."
      hideText="hide categories"
      items={Object.values(tenureCleanups)}
      keyFunction={p => `tenure-cleanup-${p?.tenureCleanupTypeCode?.id}`}
      renderFunction={p => <>{p?.tenureCleanupTypeCode?.description}</>}
      delimiter={'; '}
      maxCollapsedLength={3}
      maxExpandedLength={10}
      className="d-flex flex-wrap"
    ></ExpandableTextList>
  );
};

export const StyledLabel = styled.label`
  font-weight: bold;
  margin-bottom: 0;
  min-width: fit-content;
`;

export const GroupWrapper = styled.div`
  display: flex;
  gap: 0.5rem;
  align-items: baseline;
  flex-wrap: wrap;
`;

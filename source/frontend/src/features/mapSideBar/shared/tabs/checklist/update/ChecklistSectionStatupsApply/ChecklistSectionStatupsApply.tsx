import { useState } from 'react';
import styled from 'styled-components';

import { LinkButton } from '@/components/common/buttons/LinkButton';
import { Select, SelectOption } from '@/components/common/form/Select';

export interface IChecklistSectionStatusApplyProps {
  sectionId: string;
  sectionName: string;
  checklistItemOptions: SelectOption[];
  onSectionStatusApply: (sectionId: string, statusApply: string) => void;
}

const ChecklistSectionStatusApply: React.FC<IChecklistSectionStatusApplyProps> = ({
  sectionId,
  sectionName,
  checklistItemOptions,
  onSectionStatusApply,
}) => {
  const [applyStatus, setApplyStatus] = useState<string | null>(null);

  return (
    <StyledChecklistSectionHeaderDiv>
      <div className="section-title">{sectionName}</div>
      <div className="section-actions">
        <Select
          field={`section-${sectionName}`}
          options={checklistItemOptions}
          placeholder="Apply to section"
          onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
            setApplyStatus(e.target.value);
          }}
        />
        <LinkButton
          data-testid={`apply-to-${sectionName}-btn`}
          onClick={() => {
            onSectionStatusApply(sectionId, applyStatus);
          }}
        >
          Apply
        </LinkButton>
      </div>
    </StyledChecklistSectionHeaderDiv>
  );
};

export default ChecklistSectionStatusApply;

const StyledChecklistSectionHeaderDiv = styled.div`
  display: flex;
  flex-direction: row;
  align-items: center;

  .section-title {
    flex-grow: 1;
  }

  .section-actions {
    display: flex;
    align-items: center;
    gap: 1.75rem;

    button {
      margin-bottom: 0.75rem;
    }
  }
`;

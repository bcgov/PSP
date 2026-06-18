import { Collapse } from 'react-bootstrap';
import { MdArrowRight } from 'react-icons/md';
import styled from 'styled-components';

import { ArrowDropDownIcon } from '@/components/common/Section/SectionStyles';

import ParcelItem from '../../parcelList/ParcelItem';
import { WorklistItemModel } from './models/WorklistItem.model';

export interface CommonPropertyItemViewProps {
  isCollapsed: boolean;
  worklistItem: WorklistItemModel;
  parcelIndex: number;
  toggleCollapse: () => void;
  onRemove: (id: string) => void;
}

export const WorklistItemView: React.FC<CommonPropertyItemViewProps> = ({
  isCollapsed,
  worklistItem,
  parcelIndex,
  toggleCollapse,
  onRemove,
}) => {
  if (worklistItem.IsStrataPlanCommonProperty && worklistItem.CommonPropertyParcelsCount > 0) {
    return (
      <StyledComponentWrapperDiv data-testid={`worklist-item[${parcelIndex}]`}>
        <StyledSectionHeaderDiv>
          <StyledArrowCollapseDiv
            onClick={e => {
              e.preventDefault();
              e.stopPropagation();
              toggleCollapse();
            }}
            data-testid={`worklist-item[${parcelIndex}].collapse-btn`}
          >
            {isCollapsed && <MdArrowRight size={30} fill="#1a5a96" />}
            {!isCollapsed && <ArrowDropDownIcon fill="#1a5a96" />}
          </StyledArrowCollapseDiv>
          <StyledHeaderParcelDiv data-testid={`worklist-item[${parcelIndex}].parcel`}>
            <ParcelItem
              key={worklistItem.Parcel.id}
              parcel={worklistItem.Parcel}
              canAddToWorklist={false}
              onRemove={onRemove}
              parcelIndex={parcelIndex}
              removeIndentation={true}
              overridePropertyIdentifier={`Common Property: ${worklistItem.PlanNumber}`}
              dataTestid={`worklist-item[${parcelIndex}].parcel`}
            ></ParcelItem>
          </StyledHeaderParcelDiv>
        </StyledSectionHeaderDiv>

        <Collapse in={!isCollapsed}>
          <StyledSectionGroupDiv>
            {worklistItem.CommonPropertyParcels.map((p, index) => (
              <ParcelItem
                key={p.id}
                parcel={p}
                onRemove={onRemove}
                canAddToWorklist={false}
                parcelIndex={index}
                removeIndentation={false}
                dataTestid={`worklist-item[${parcelIndex}].child[${index}]`}
              />
            ))}
            <hr></hr>
          </StyledSectionGroupDiv>
        </Collapse>
      </StyledComponentWrapperDiv>
    );
  } else if (
    worklistItem.IsStrataPlanCommonProperty &&
    worklistItem.CommonPropertyParcelsCount === 0
  ) {
    return (
      <StyledComponentWrapperDiv data-testid={`worklist-item[${parcelIndex}]`}>
        <StyledSectionHeaderDiv>
          <StyledArrowCollapseDiv></StyledArrowCollapseDiv>
          <StyledHeaderParcelDiv data-testid={`worklist-item[${parcelIndex}].parcel`}>
            <ParcelItem
              key={worklistItem.Parcel.id}
              parcel={worklistItem.Parcel}
              canAddToWorklist={false}
              onRemove={onRemove}
              parcelIndex={parcelIndex}
              removeIndentation={true}
              overridePropertyIdentifier={`Common Property: ${worklistItem.PlanNumber}`}
              dataTestid={`worklist-item[${parcelIndex}].parcel`}
            ></ParcelItem>
          </StyledHeaderParcelDiv>
        </StyledSectionHeaderDiv>
      </StyledComponentWrapperDiv>
    );
  } else {
    return (
      <StyledComponentWrapperDiv data-testid={`worklist-item[${parcelIndex}]`}>
        <StyledSectionHeaderDiv>
          <StyledArrowCollapseDiv></StyledArrowCollapseDiv>
          <StyledHeaderParcelDiv data-testid={`worklist-item[${parcelIndex}].parcel`}>
            <ParcelItem
              key={worklistItem.Parcel.id}
              parcel={worklistItem.Parcel}
              canAddToWorklist={false}
              onRemove={onRemove}
              parcelIndex={parcelIndex}
              removeIndentation={true}
              dataTestid={`worklist-item[${parcelIndex}].parcel`}
            ></ParcelItem>
          </StyledHeaderParcelDiv>
        </StyledSectionHeaderDiv>
      </StyledComponentWrapperDiv>
    );
  }
};

const StyledComponentWrapperDiv = styled.div`
  text-align: left;
  text-underline-offset: 2px;

  button {
    font-size: 14px;
  }
`;

const StyledSectionHeaderDiv = styled.div`
  display: flex;
  flex-direction: row;
  width: 100%;
  justify-content: flex-start;
  align-items: center;
  flex-wrap: nowrap;
`;

const StyledArrowCollapseDiv = styled.div`
  width: 2.8rem;
`;

const StyledHeaderParcelDiv = styled.div`
  flex: 1;
`;

const StyledSectionGroupDiv = styled.div`
  padding-left: 3rem;
`;

export default WorklistItemView;

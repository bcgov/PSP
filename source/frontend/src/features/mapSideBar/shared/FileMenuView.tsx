import cx from 'classnames';
import { geoJSON, latLngBounds } from 'leaflet';
import { useCallback, useMemo } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaCaretRight, FaSearchPlus } from 'react-icons/fa';
import { PiCornersOut } from 'react-icons/pi';
import styled from 'styled-components';

import { RestrictedEditControl } from '@/components/common/buttons';
import { EditPropertiesIcon } from '@/components/common/buttons/EditPropertiesButton';
import { LinkButton } from '@/components/common/buttons/LinkButton';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import {
  boundaryFromFileProperty,
  exists,
  getFilePropertyName,
  getLatLng,
  locationFromFileProperty,
  sortFileProperties,
} from '@/utils';

import { cannotEditMessage } from '../acquisition/common/constants';

export interface IFileMenuProps {
  file: ApiGen_Concepts_File;
  currentPropertyIndex: number | null;
  canEdit: boolean;
  isInNonEditableState: boolean;
  editRestrictionMessage?: string;
  onSelectFileSummary: () => void;
  onSelectProperty: (propertyId: number) => void;
  onEditProperties: () => void;
}

const FileMenuView: React.FunctionComponent<React.PropsWithChildren<IFileMenuProps>> = ({
  file,
  currentPropertyIndex,
  canEdit,
  isInNonEditableState,
  editRestrictionMessage = cannotEditMessage,
  onSelectFileSummary,
  onSelectProperty,
  onEditProperties,
  children,
}) => {
  // respect the order of properties as set by the user creating the file
  const sortedProperties = sortFileProperties(file?.fileProperties ?? []);
  const isSummary = useMemo(() => !exists(currentPropertyIndex), [currentPropertyIndex]);
  const mapMachine = useMapStateMachine();

  const fitBoundaries = () => {
    const fileProperties = file?.fileProperties;

    if (exists(fileProperties)) {
      const locations = fileProperties
        .map(fileProp => locationFromFileProperty(fileProp))
        .map(geom => getLatLng(geom))
        .filter(exists);
      const latLngBoudns = latLngBounds(locations);

      mapMachine.requestFlyToBounds(latLngBoudns);
    }
  };

  const onPropertyClick = (index: number, propertyId: number) => {
    if (currentPropertyIndex !== index) {
      onSelectProperty(propertyId);
    }
  };

  const onZoomToProperty = useCallback(
    (property: ApiGen_Concepts_FileProperty) => {
      const geom = boundaryFromFileProperty(property);
      const bounds = geoJSON(geom).getBounds();

      if (exists(bounds) && bounds.isValid()) {
        mapMachine.requestFlyToBounds(bounds);
      }
    },
    [mapMachine],
  );

  return (
    <StyledMenuWrapper>
      <StyledRow data-testid="menu-item-summary" className={cx({ selected: isSummary })}>
        <Col>
          {isSummary ? (
            <span title="File Details">File Summary</span>
          ) : (
            <LinkButton title="File Details" onClick={onSelectFileSummary}>
              File Summary
            </LinkButton>
          )}
        </Col>
      </StyledRow>
      <StyledMenuHeaderWrapper>
        <Row noGutters className="w-100">
          <Col>
            <StyledMenuHeader>Properties</StyledMenuHeader>
          </Col>
          <Col xs="auto">
            <RestrictedEditControl
              canEdit={canEdit}
              isInNonEditableState={isInNonEditableState}
              icon={<EditPropertiesIcon />}
              title="Change properties"
              toolTipId={`${file?.id ?? 0}-summary-cannot-edit-tooltip`}
              editRestrictionMessage={editRestrictionMessage}
              onEdit={onEditProperties}
            />
          </Col>
          <Col xs="auto">
            <LinkButton
              title="Fit boundaries button"
              data-testid="fit-file-properties-boundaries"
              onClick={fitBoundaries}
            >
              <PiCornersOut size={18} className="mr-2" />
            </LinkButton>
          </Col>
        </Row>
      </StyledMenuHeaderWrapper>
      <div className={'p-1'} />
      <StyledMenuBodyWrapper>
        {sortedProperties.map((property: ApiGen_Concepts_FileProperty, index: number) => {
          const propertyName = getFilePropertyName(property);
          const isCurrentProperty = currentPropertyIndex === index;
          return (
            <StyledRow
              key={`menu-item-row-${index}`}
              className={cx('no-gutters', { selected: isCurrentProperty })}
              data-testid={`menu-item-row-${index}`}
            >
              <Col xs="1">{isCurrentProperty && <FaCaretRight />}</Col>
              <Col xs="auto" className="pr-2">
                <StyledIconWrapper className={cx({ selected: isCurrentProperty })}>
                  {index + 1}
                </StyledIconWrapper>
              </Col>
              <Col>
                {isCurrentProperty ? (
                  <StyledSelectedName title="View">{propertyName.value}</StyledSelectedName>
                ) : (
                  <LinkButton
                    onClick={() => onPropertyClick(index, property.id)}
                    data-testid={`menu-item-property-${index}`}
                    title="View Property"
                  >
                    {propertyName.value}
                  </LinkButton>
                )}
              </Col>
              <Col xs="auto">
                <LinkButton
                  onClick={() => onZoomToProperty(property)}
                  data-testid={`menu-item-zoom-${index}`}
                  title="Zoom to property"
                >
                  <FaSearchPlus size={18} className="mr-2" />
                </LinkButton>
              </Col>
            </StyledRow>
          );
        })}
      </StyledMenuBodyWrapper>
      <>{children}</>
    </StyledMenuWrapper>
  );
};

export default FileMenuView;

export const StyledMenuWrapper = styled.div`
  flex: 1;
  text-align: left;
  padding: 0px;
  margin: 0px;
  width: 100%;
  color: ${props => props.theme.css.linkColor};

  display: flex;
  flex-direction: column;
`;

const StyledRow = styled(Row)`
  width: 100%;

  font-size: 1.4rem;
  font-weight: normal;
  cursor: pointer;
  padding-bottom: 0.5rem;

  div.Button__value {
    font-size: 1.4rem;
  }
  &.selected {
    cursor: default;
    font-weight: bold;
  }
`;

const StyledSelectedName = styled.span`
  line-height: 3rem;
`;

const StyledIconWrapper = styled.div`
  &.selected {
    background-color: ${props => props.theme.bcTokens.themeGold100};
  }

  background-color: ${props => props.theme.css.numberBackgroundColor};
  font-size: 1.5rem;
  border-radius: 50%;
  opacity: 0.8;
  width: 2.5rem;
  height: 2.5rem;
  padding: 1rem;
  display: flex;
  justify-content: center;
  align-items: center;
`;

const StyledMenuHeaderWrapper = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
  width: 100%;
  border-bottom: 1px solid ${props => props.theme.css.borderOutlineColor};
`;

const StyledMenuHeader = styled.span`
  font-weight: bold;
  font-size: 1.6rem;
  color: ${props => props.theme.bcTokens.iconsColorSecondary};
  line-height: 2.2rem;
`;

const StyledMenuBodyWrapper = styled.div`
  flex-grow: 1;
`;

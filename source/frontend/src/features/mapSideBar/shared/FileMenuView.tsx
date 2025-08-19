import cx from 'classnames';
import { geoJSON, latLngBounds } from 'leaflet';
import { useCallback, useMemo } from 'react';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaCaretRight, FaSearchPlus } from 'react-icons/fa';
import { PiCornersOut } from 'react-icons/pi';
import styled from 'styled-components';

import { RestrictedEditControl } from '@/components/common/buttons';
import { EditPropertiesIcon } from '@/components/common/buttons/EditPropertiesButton';
import { LinkButton } from '@/components/common/buttons/LinkButton';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import OverflowTip from '@/components/common/OverflowTip';
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
  currentFilePropertyId: number | null;
  canEdit: boolean;
  isInNonEditableState: boolean;
  editRestrictionMessage?: string;
  onSelectFileSummary: () => void;
  onSelectProperty: (propertyId: number) => void;
  onEditProperties: () => void;
}

const FileMenuView: React.FunctionComponent<React.PropsWithChildren<IFileMenuProps>> = ({
  file,
  currentFilePropertyId,
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
  const isSummary = useMemo(() => !exists(currentFilePropertyId), [currentFilePropertyId]);
  const mapMachine = useMapStateMachine();

  const fitBoundaries = () => {
    const fileProperties = file?.fileProperties;

    if (exists(fileProperties)) {
      const locations = fileProperties
        .map(fileProp => locationFromFileProperty(fileProp))
        .map(geom => getLatLng(geom))
        .filter(exists);
      const bounds = latLngBounds(locations);

      if (exists(bounds) && bounds.isValid()) {
        mapMachine.requestFlyToBounds(bounds);
      }
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

  const activeProperties = [];
  const inactiveProperties = [];
  sortedProperties.forEach(p => {
    if (p.isActive !== false) {
      activeProperties.push(p);
    } else {
      inactiveProperties.push(p);
    }
  });
  const labelledProperties: { label: string; properties: ApiGen_Concepts_FileProperty[] }[] = [
    { label: 'Active', properties: activeProperties },
    { label: 'Inactive', properties: inactiveProperties },
  ];

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
        {labelledProperties
          .filter(lp => lp.properties?.length > 0)
          .map(
            (labelledProperties: { label: string; properties: ApiGen_Concepts_FileProperty[] }) => {
              return (
                <React.Fragment key={`menu-label-${labelledProperties.label}`}>
                  {labelledProperties.label}
                  {sortedProperties
                    .filter(sp => labelledProperties.properties.includes(sp))
                    .map((fileProperty: ApiGen_Concepts_FileProperty, index: number) => {
                      const propertyName = getFilePropertyName(fileProperty);
                      return (
                        <StyledPropertyRowWrapper
                          key={`menu-item-row-${fileProperty?.id ?? index}`}
                          data-testid={`menu-item-row-${fileProperty?.id ?? index}`}
                          className={cx({
                            selected: currentFilePropertyId === fileProperty?.id,
                          })}
                          onClick={() => {
                            if (currentFilePropertyId !== fileProperty?.id) {
                              onSelectProperty(fileProperty.id);
                            }
                          }}
                        >
                          <div>
                            {currentFilePropertyId === fileProperty?.id && <FaCaretRight />}
                          </div>

                          <div>
                            {fileProperty?.isActive !== false ? (
                              <StyledIconWrapper
                                className={cx({
                                  selected: currentFilePropertyId === fileProperty?.id,
                                })}
                              >
                                {sortedProperties.indexOf(fileProperty) + 1}
                              </StyledIconWrapper>
                            ) : (
                              <StyledDisabledIconWrapper>
                                {sortedProperties.indexOf(fileProperty) + 1}
                              </StyledDisabledIconWrapper>
                            )}
                          </div>

                          <OverflowTip>
                            {currentFilePropertyId === fileProperty?.id ? (
                              <OverflowTip fullText={propertyName.value}>
                                {propertyName.value}
                              </OverflowTip>
                            ) : (
                              <OverflowTip
                                fullText={propertyName.value}
                                valueTestId={`menu-item-property-${index}`}
                              ></OverflowTip>
                            )}
                          </OverflowTip>

                          <StyledPropertyRowZoom>
                            <LinkButton
                              onClick={(event: React.MouseEvent<SVGElement>) => {
                                event.stopPropagation();
                                onZoomToProperty(fileProperty);
                              }}
                              data-testid={`menu-item-zoom-${index}`}
                              title="Zoom to property"
                            >
                              <FaSearchPlus size={18} className="mr-2" />
                            </LinkButton>
                          </StyledPropertyRowZoom>
                        </StyledPropertyRowWrapper>
                      );
                    })}
                </React.Fragment>
              );
            },
          )}
      </StyledMenuBodyWrapper>
      <>{children}</>
    </StyledMenuWrapper>
  );
};

export default FileMenuView;

const StyledDisabledIconWrapper = styled.div`
  &.selected {
    border-color: ${props => props.theme.bcTokens.themeGray110};
  }
  border: solid 0.3rem;
  border-color: ${props => props.theme.bcTokens.themeGray100};
  font-size: 1.5rem;
  border-radius: 20%;
  width: 3.25rem;
  height: 3.25rem;
  padding: 1rem;
  display: flex;
  justify-content: center;
  align-items: center;
  color: black;
  font-family: 'BCSans-Bold';
`;

export const StyledMenuWrapper = styled.div`
  display: flex;
  flex: 1;
  text-align: left;
  padding: 0px;
  margin: 0px;
  width: 100%;
  color: ${props => props.theme.css.linkColor};

  flex-direction: column;
`;

const StyledRow = styled(Row)`
  width: 100%;

  &.selected {
    font-weight: bold;
    cursor: default;
  }

  font-size: 1.4rem;
  font-weight: normal;
  cursor: pointer;
  padding-bottom: 0.5rem;

  div.Button__value {
    font-size: 1.4rem;
  }
`;

const StyledPropertyRowWrapper = styled.div`
  display: flex;
  &.selected {
    font-weight: bold;
    cursor: default;
  }

  font-size: 1.4rem;
  font-weight: normal;
  cursor: pointer;
  padding-bottom: 0.5rem;
`;

const StyledPropertyRowZoom = styled.div`
  align-self: flex-end;
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

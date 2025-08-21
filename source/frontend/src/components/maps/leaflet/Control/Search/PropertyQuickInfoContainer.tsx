import { geoJSON } from 'leaflet';
import React, { SyntheticEvent, useCallback, useEffect, useMemo, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaEye, FaMinus, FaPlus, FaSearchPlus, FaWindowClose } from 'react-icons/fa';
import styled from 'styled-components';

import AcquisitionIcon from '@/assets/images/acquisition-icon.svg?react';
import DispositionIcon from '@/assets/images/disposition-icon.svg?react';
import LeaseIcon from '@/assets/images/lease-icon.svg?react';
import ManagementIcon from '@/assets/images/management-icon.svg?react';
import ResearchIcon from '@/assets/images/research-icon.svg?react';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { WorklistLocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import MoreOptionsMenu, { MenuOption } from '@/components/common/MoreOptionsMenu';
import { SectionField } from '@/components/common/Section/SectionField';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { Claims } from '@/constants';
import usePathGenerator from '@/features/mapSideBar/shared/sidebarPathGenerator';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { useLtsa } from '@/hooks/useLtsa';
import { exists, firstOrNull, isValidString, pidFormatter } from '@/utils';
import { formatNames } from '@/utils/personUtils';

export const PropertyQuickInfoContainer: React.FC<React.PropsWithChildren> = () => {
  const keycloak = useKeycloakWrapper();
  const [ownerNames, setOwnerNames] = useState('');

  const {
    requestFlyToBounds,
    requestFlyToLocation,
    mapLocationFeatureDataset,
    prepareForCreation,
    worklistAdd,
    isEditPropertiesMode,
  } = useMapStateMachine();

  const pathGenerator = usePathGenerator();

  const locationInfo = useMemo(
    () => firstOrNull(mapLocationFeatureDataset?.parcelFeatures)?.properties,
    [mapLocationFeatureDataset?.parcelFeatures],
  );

  const { ltsaRequestWrapper } = useLtsa();
  const getLtsaExecute = ltsaRequestWrapper.execute;

  const hasMultipleProperties = useMemo(
    () => mapLocationFeatureDataset?.parcelFeatures?.length > 1,
    [mapLocationFeatureDataset?.parcelFeatures?.length],
  );

  const isEmpty = useMemo(
    () => mapLocationFeatureDataset?.parcelFeatures?.length === 0,
    [mapLocationFeatureDataset?.parcelFeatures?.length],
  );

  const getOwnerInfo = useCallback(
    async (pid: string) => {
      if (isValidString(pid)) {
        const ltsaOrders = await getLtsaExecute(pid);
        const titleOwners = ltsaOrders?.titleOrders
          ?.flatMap(x => x?.orderedProduct?.fieldedData?.ownershipGroups)
          ?.flatMap(x => x?.titleOwners)
          ?.filter(exists);

        const names =
          titleOwners?.map(x => formatNames([x.givenName, x.lastNameOrCorpName1])) ?? [];
        setOwnerNames(names.join(', '));
      }
    },
    [getLtsaExecute],
  );

  useEffect(() => {
    getOwnerInfo(locationInfo?.PID);
  }, [getOwnerInfo, locationInfo?.PID]);

  const onViewPropertyInfo = useCallback(() => {
    pathGenerator.showPropertyByPid(locationInfo.PID);
  }, [locationInfo?.PID, pathGenerator]);

  const onZoomToBounds = useCallback(() => {
    if (exists(locationInfo?.SHAPE)) {
      const bounds = geoJSON(locationInfo.SHAPE).getBounds();

      if (exists(bounds) && bounds.isValid()) {
        requestFlyToBounds(bounds);
      }
    } else if (exists(mapLocationFeatureDataset?.location)) {
      requestFlyToLocation(mapLocationFeatureDataset.location);
    }
  }, [
    locationInfo?.SHAPE,
    mapLocationFeatureDataset?.location,
    requestFlyToBounds,
    requestFlyToLocation,
  ]);

  const showViewPropertyInfo = useMemo(
    () => isValidString(locationInfo?.PID) && !hasMultipleProperties,
    [hasMultipleProperties, locationInfo?.PID],
  );

  const mapMachine = useMapStateMachine();

  const isVisible = useMemo(() => mapMachine.isShowingQuickInfo, [mapMachine]);
  const isMinimized = useMemo(() => mapMachine.isQuickInfoMinimized, [mapMachine]);

  const toggleMinimize = () => {
    if (isMinimized) {
      mapMachine.openQuickInfo();
    } else {
      mapMachine.minimizeQuickInfo();
    }
  };

  const close = useCallback(() => {
    mapMachine.closeQuickInfo();
  }, [mapMachine]);

  // Convert to an object that can be consumed by the file creation process
  const selectedFeatureDataset = useMemo<SelectedFeatureDataset>(() => {
    return {
      selectingComponentId: mapLocationFeatureDataset?.selectingComponentId ?? null,
      location: mapLocationFeatureDataset?.location,
      fileLocation: mapLocationFeatureDataset?.fileLocation ?? null,
      parcelFeature: firstOrNull(mapLocationFeatureDataset?.parcelFeatures),
      pimsFeature: firstOrNull(mapLocationFeatureDataset?.pimsFeatures),
      regionFeature: mapLocationFeatureDataset?.regionFeature ?? null,
      districtFeature: mapLocationFeatureDataset?.districtFeature ?? null,
      municipalityFeature: firstOrNull(mapLocationFeatureDataset?.municipalityFeatures),
      isActive: true,
      displayOrder: 0,
    };
  }, [
    mapLocationFeatureDataset?.selectingComponentId,
    mapLocationFeatureDataset?.location,
    mapLocationFeatureDataset?.fileLocation,
    mapLocationFeatureDataset?.parcelFeatures,
    mapLocationFeatureDataset?.pimsFeatures,
    mapLocationFeatureDataset?.regionFeature,
    mapLocationFeatureDataset?.districtFeature,
    mapLocationFeatureDataset?.municipalityFeatures,
  ]);

  const onAddToWorklist = useCallback(() => {
    const worklistDataSet: WorklistLocationFeatureDataset = {
      ...selectedFeatureDataset,
      fullyAttributedFeatures: {
        type: 'FeatureCollection',
        features: [selectedFeatureDataset.parcelFeature],
      },
    };
    worklistAdd(worklistDataSet);
  }, [selectedFeatureDataset, worklistAdd]);

  const onCreateResearchFile = useCallback(() => {
    prepareForCreation([selectedFeatureDataset]);
    pathGenerator.newFile('research');
  }, [pathGenerator, prepareForCreation, selectedFeatureDataset]);

  const onCreateAcquisitionFile = useCallback(() => {
    prepareForCreation([selectedFeatureDataset]);
    pathGenerator.newFile('acquisition');
  }, [pathGenerator, prepareForCreation, selectedFeatureDataset]);

  const onCreateDispositionFile = useCallback(() => {
    prepareForCreation([selectedFeatureDataset]);
    pathGenerator.newFile('disposition');
  }, [pathGenerator, prepareForCreation, selectedFeatureDataset]);

  const onCreateLeaseFile = useCallback(() => {
    prepareForCreation([selectedFeatureDataset]);
    pathGenerator.newFile('lease');
  }, [pathGenerator, prepareForCreation, selectedFeatureDataset]);

  const onCreateManagementFile = useCallback(() => {
    prepareForCreation([selectedFeatureDataset]);
    pathGenerator.newFile('management');
  }, [pathGenerator, prepareForCreation, selectedFeatureDataset]);

  const onAddToOpenFile = useCallback(() => {
    // If in edit properties mode, prepare the parcel for addition to an open file
    if (isEditPropertiesMode) {
      prepareForCreation([selectedFeatureDataset]);
    }
  }, [isEditPropertiesMode, prepareForCreation, selectedFeatureDataset]);

  const menuOptions: MenuOption[] = useMemo(() => {
    const options: MenuOption[] = [];

    options.push({
      label: 'Add to Worklist',
      onClick: onAddToWorklist,
      icon: <ResearchIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
    });

    if (keycloak.hasClaim(Claims.RESEARCH_ADD)) {
      options.push({
        label: 'Create Research File',
        onClick: onCreateResearchFile,
        icon: <ResearchIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
      });
    }
    if (keycloak.hasClaim(Claims.ACQUISITION_ADD)) {
      options.push({
        label: 'Create Acquisition File',
        onClick: onCreateAcquisitionFile,
        icon: <AcquisitionIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
      });
    }
    if (keycloak.hasClaim(Claims.MANAGEMENT_ADD)) {
      options.push({
        label: 'Create Management File',
        onClick: onCreateManagementFile,
        icon: <ManagementIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
      });
    }
    if (keycloak.hasClaim(Claims.LEASE_ADD)) {
      options.push({
        label: 'Create Lease File',
        onClick: onCreateLeaseFile,
        icon: <LeaseIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
      });
    }
    if (keycloak.hasClaim(Claims.DISPOSITION_ADD)) {
      options.push({
        label: 'Create Disposition File',
        onClick: onCreateDispositionFile,
        icon: <DispositionIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
      });
    }

    options.push({
      label: 'Add to Open File',
      onClick: onAddToOpenFile,
      icon: isEditPropertiesMode ? <FaPlus size="1.5rem" /> : undefined,
      disabled: !isEditPropertiesMode,
      tooltip: 'A file must be open and in "edit property" mode',
      separator: true, // Add a separator before the "Add to Open File" option
    });

    return options;
  }, [
    isEditPropertiesMode,
    keycloak,
    onAddToOpenFile,
    onAddToWorklist,
    onCreateAcquisitionFile,
    onCreateDispositionFile,
    onCreateLeaseFile,
    onCreateManagementFile,
    onCreateResearchFile,
  ]);

  const isLoading = useMemo(() => {
    return mapMachine.isLoading || ltsaRequestWrapper.loading;
  }, [ltsaRequestWrapper.loading, mapMachine.isLoading]);

  return (
    <StyledContainer isMinimized={isMinimized} isVisible={isVisible}>
      <LoadingBackdrop show={isLoading} parentScreen />
      <StyledHeaderRow noGutters>
        <Col xs="1">
          {showViewPropertyInfo && (
            <TooltipWrapper
              tooltipId={`property-quick-info-view-property`}
              tooltip={'View Property Information'}
            >
              <StyledIconWrapper>
                <FaEye size={18} title="Zoom map" onClick={onViewPropertyInfo} />
              </StyledIconWrapper>
            </TooltipWrapper>
          )}
        </Col>
        <Col xs="1" className="pl-2">
          <MoreOptionsMenu variant="dark" options={menuOptions} />
        </Col>
        <Col xs="1"></Col>
        <Col xs="6" className="text-center">
          Property
        </Col>
        <Col xs="1">
          <TooltipWrapper tooltipId={`property-quick-info-zoom`} tooltip={'Zoom to location'}>
            <StyledIconWrapper>
              <FaSearchPlus
                size={18}
                title="Zoom map"
                onClick={(event: React.MouseEvent<SVGElement>) => {
                  event.preventDefault();
                  event.stopPropagation();
                  onZoomToBounds();
                }}
              />
            </StyledIconWrapper>
          </TooltipWrapper>
        </Col>
        <Col xs="1">
          <TooltipWrapper
            tooltipId={`property-quick-info-toggle`}
            tooltip={'Toggle Quick Property Information'}
          >
            <StyledIconWrapper
              onClick={(event: SyntheticEvent) => {
                event.preventDefault();
                event.stopPropagation();
                toggleMinimize();
              }}
            >
              {isMinimized ? (
                <FaPlus data-testid="toggle-icon" size={18} title="Open quick info" />
              ) : (
                <FaMinus data-testid="toggle-icon" size={18} title="Minimize quick info" />
              )}
            </StyledIconWrapper>
          </TooltipWrapper>
        </Col>
        <Col xs="1">
          <TooltipWrapper
            tooltipId={`property-quick-info-close`}
            tooltip={'Close Quick Property Information'}
          >
            <StyledCloseIcon
              data-testid="close-icon"
              size={18}
              title="Toggle quick info"
              onClick={(event: React.MouseEvent<SVGElement>) => {
                event.preventDefault();
                event.stopPropagation();
                close();
              }}
            />
          </TooltipWrapper>
        </Col>
      </StyledHeaderRow>
      {!isMinimized && isLoading === false && (
        <>
          {isEmpty && <div className="pt-8">No property found in this location</div>}
          {!hasMultipleProperties && !isEmpty && (
            <StyledInfoWrapper>
              <Row noGutters>
                <Col>
                  <SectionField label={'PID'} labelWidth={{ xs: 12 }}>
                    {pidFormatter(locationInfo?.PID) ?? '-'}
                  </SectionField>
                </Col>
                <Col>
                  <SectionField label={'PIN'} labelWidth={{ xs: 12 }}>
                    {locationInfo?.PIN ?? '-'}
                  </SectionField>
                </Col>
                <Col>
                  <SectionField label={'Plan'} labelWidth={{ xs: 12 }}>
                    {locationInfo?.PLAN_NUMBER ?? '-'}
                  </SectionField>
                </Col>
              </Row>
              <SectionField label={'Owners'} labelWidth={{ xs: 12 }}>
                {ownerNames ?? '-'}
              </SectionField>
              <SectionField label={'Legal Description'} labelWidth={{ xs: 12 }}>
                {locationInfo?.LEGAL_DESCRIPTION ?? '-'}
              </SectionField>
            </StyledInfoWrapper>
          )}
          {hasMultipleProperties && <div className="pt-8">Multiple properties found</div>}
        </>
      )}
    </StyledContainer>
  );
};

const StyledContainer = styled.div<{ isMinimized: boolean; isVisible: boolean }>`
  height: ${p => (p.isMinimized ? 3.5 : 25)}rem;
  width: 34rem;
  display: ${p => (p.isVisible ? 'default' : 'none')};

  background-color: ${props => props.theme.bcTokens.surfaceColorFormsDefault};
  border-radius: 0.4rem;
  box-shadow: -0.2rem 0.1rem 0.4rem rgba(0, 0, 0, 0.2);
`;

const StyledInfoWrapper = styled.div`
  font-size: 1.4rem;
  padding: 1rem;
  overflow-x: hide;
  overflow-y: auto;
  height: 21rem;
  width: 100%;
`;

const StyledHeaderRow = styled(Row)`
  background-color: ${({ theme }) => theme.tenant.colour};
  color: ${props => props.theme.bcTokens.surfaceColorFormsDefault};
  padding: 1rem;

  font-size: 1.8rem;
  color: ${props => props.theme.bcTokens.surfaceColorFormsDefault};
  text-decoration: none solid rgb(255, 255, 255);
  line-height: 1.4rem;
  font-weight: bold;
`;

const StyledIconWrapper = styled.div`
  color: white;
  font-size: 22px;
  cursor: pointer;
`;

const StyledCloseIcon = styled(FaWindowClose)`
  color: ${props => props.theme.bcTokens.surfaceColorFormsDefault};
  font-size: 22px;
  cursor: pointer;
`;

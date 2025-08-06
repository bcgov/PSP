import { geoJSON } from 'leaflet';
import { SyntheticEvent, useCallback, useEffect, useMemo, useState } from 'react';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaEye, FaMinus, FaPlus, FaSearchPlus, FaWindowClose } from 'react-icons/fa';
import styled from 'styled-components';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { SectionField } from '@/components/common/Section/SectionField';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import usePathGenerator from '@/features/mapSideBar/shared/sidebarPathGenerator';
import { useLtsa } from '@/hooks/useLtsa';
import { exists, firstOrNull, isValidString, pidFormatter } from '@/utils';
import { formatNames } from '@/utils/personUtils';

export const PropertyQuickInfoContainer: React.FC<React.PropsWithChildren> = () => {
  const [ownerNames, setOwnerNames] = useState('');
  const { requestFlyToBounds, requestFlyToLocation, mapLocationFeatureDataset } =
    useMapStateMachine();

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

  const getOwnerInfo = useCallback(
    async (pid: string) => {
      if (isValidString(pid)) {
        const ltsaOrders = await getLtsaExecute(pid);
        const titleOwners = ltsaOrders.titleOrders
          .flatMap(x => x?.orderedProduct?.fieldedData?.ownershipGroups)
          .flatMap(x => x?.titleOwners)
          .filter(exists);

        const names = titleOwners.map(x => formatNames([x.givenName, x.lastNameOrCorpName1]));
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

  return (
    <StyledContainer isMinimized={isMinimized} isVisible={isVisible}>
      <LoadingBackdrop show={mapMachine.isLoading} parentScreen />
      <StyledHeaderRow noGutters>
        <Col xs="1">
          {showViewPropertyInfo && (
            <TooltipWrapper
              tooltipId={`property-quick-info-view-property`}
              tooltip={'View Property Information'}
            >
              <FaEye size={18} title="Zoom map" onClick={onViewPropertyInfo} />
            </TooltipWrapper>
          )}
        </Col>
        <Col xs="1">...</Col>
        <Col xs="1"></Col>
        <Col xs="6" className="text-center">
          Property
        </Col>
        <Col xs="1">
          <TooltipWrapper tooltipId={`property-quick-info-zoom`} tooltip={'Zoom to property'}>
            <FaSearchPlus
              size={18}
              title="Zoom map"
              onClick={(event: React.MouseEvent<SVGElement>) => {
                event.preventDefault();
                event.stopPropagation();
                onZoomToBounds();
              }}
            />
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
                <FaPlus data-testid="toggle-icon" size={18} title="Toggle quick info" />
              ) : (
                <FaMinus data-testid="toggle-icon" size={18} title="Toggle quick info" />
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
      {!isMinimized && (
        <>
          {!hasMultipleProperties && (
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

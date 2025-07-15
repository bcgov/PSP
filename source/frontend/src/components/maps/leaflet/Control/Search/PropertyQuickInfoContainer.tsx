import { geoJSON } from 'leaflet';
import { useCallback, useEffect, useMemo, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaEye, FaSearchPlus } from 'react-icons/fa';
import styled from 'styled-components';

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

  return (
    <>
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
        <Col xs="8" className="text-center">
          Property
        </Col>
        <Col xs="1">
          <TooltipWrapper tooltipId={`property-quick-info-zoom`} tooltip={'Zoom to property'}>
            <FaSearchPlus size={18} title="Zoom map" onClick={onZoomToBounds} />
          </TooltipWrapper>
        </Col>
        <Col xs="1">-</Col>
      </StyledHeaderRow>
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
  );
};
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

import { geoJSON } from 'leaflet';
import { useCallback, useMemo } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaPlus } from 'react-icons/fa';
import { FaSearchPlus } from 'react-icons/fa';
import styled from 'styled-components';

import AcquisitionIcon from '@/assets/images/acquisition-icon.svg?react';
import DispositionIcon from '@/assets/images/disposition-icon.svg?react';
import LeaseIcon from '@/assets/images/lease-icon.svg?react';
import ManagementIcon from '@/assets/images/management-icon.svg?react';
import ResearchIcon from '@/assets/images/research-icon.svg?react';
import { LinkButton, RemoveIconButton } from '@/components/common/buttons';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { WorklistLocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import MoreOptionsMenu, { MenuOption } from '@/components/common/MoreOptionsMenu';
import OverflowTip from '@/components/common/OverflowTip';
import { Claims } from '@/constants';
import usePathGenerator from '@/features/mapSideBar/shared/sidebarPathGenerator';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { exists, getPropertyNameFromSelectedFeatureSet, NameSourceType } from '@/utils';

import { ParcelDataset } from './models';

export interface IParcelItemProps {
  canAddToWorklist: boolean;
  parcel: ParcelDataset;
  onRemove: (id: string) => void | null;
}

export function ParcelItem({ parcel, onRemove, canAddToWorklist }: IParcelItemProps) {
  const propertyName = getPropertyNameFromSelectedFeatureSet(parcel.toSelectedFeatureDataset());
  let propertyIdentifier = '';
  switch (propertyName.label) {
    case NameSourceType.PID:
    case NameSourceType.PIN:
    case NameSourceType.PLAN:
    case NameSourceType.ADDRESS:
      propertyIdentifier = `${propertyName.label}: ${propertyName.value}`;
      break;
    case NameSourceType.LOCATION:
      propertyIdentifier = `${propertyName.value}`;
      break;
    default:
      break;
  }

  const {
    requestFlyToBounds,
    requestFlyToLocation,
    prepareForCreation,
    isEditPropertiesMode,
    worklistAdd,
    setSelectedLocation,
  } = useMapStateMachine();

  const canAddToOpenFile = isEditPropertiesMode;

  const pathGenerator = usePathGenerator();

  const handleZoom = useCallback(() => {
    if (exists(parcel.pmbcFeature)) {
      const bounds = geoJSON(parcel.pmbcFeature).getBounds();
      if (exists(bounds) && bounds.isValid()) {
        requestFlyToBounds(bounds);
      } else if (exists(parcel.location)) {
        requestFlyToLocation(parcel.location);
      }
    } else if (exists(parcel.pimsFeature)) {
      const bounds = geoJSON(parcel.pimsFeature).getBounds();
      if (exists(bounds) && bounds.isValid()) {
        requestFlyToBounds(bounds);
      } else if (exists(parcel.location)) {
        requestFlyToLocation(parcel.location);
      }
    } else if (exists(parcel.location)) {
      requestFlyToLocation(parcel.location);
    }
  }, [
    parcel.location,
    parcel.pimsFeature,
    parcel.pmbcFeature,
    requestFlyToBounds,
    requestFlyToLocation,
  ]);

  const handleSelect = useCallback(() => {
    setSelectedLocation(parcel.toLocationFeatureDataset());
  }, [parcel, setSelectedLocation]);

  const onAddToWorklist = useCallback(() => {
    const featuresSet = parcel.toSelectedFeatureDataset();

    const worklistDataSet: WorklistLocationFeatureDataset = {
      ...featuresSet,
      fullyAttributedFeatures: null,
    };

    if (exists(featuresSet.parcelFeature)) {
      worklistDataSet.fullyAttributedFeatures = {
        type: 'FeatureCollection',
        features: [featuresSet.parcelFeature],
      };
    }

    worklistAdd(worklistDataSet);
  }, [parcel, worklistAdd]);

  const handleCreateResearchFile = useCallback(() => {
    const featuresSet = parcel.toSelectedFeatureDataset();
    prepareForCreation([featuresSet]);
    pathGenerator.newFile('research');
  }, [parcel, pathGenerator, prepareForCreation]);

  const handleCreateAcquisitionFile = useCallback(() => {
    const featuresSet = parcel.toSelectedFeatureDataset();
    prepareForCreation([featuresSet]);
    pathGenerator.newFile('acquisition');
  }, [parcel, pathGenerator, prepareForCreation]);

  const handleCreateDispositionFile = useCallback(() => {
    const featuresSet = parcel.toSelectedFeatureDataset();
    prepareForCreation([featuresSet]);
    pathGenerator.newFile('disposition');
  }, [parcel, pathGenerator, prepareForCreation]);

  const handleCreateLeaseFile = useCallback(() => {
    const featuresSet = parcel.toSelectedFeatureDataset();
    prepareForCreation([featuresSet]);
    pathGenerator.newFile('lease');
  }, [parcel, pathGenerator, prepareForCreation]);

  const handleCreateManagementFile = useCallback(() => {
    const featuresSet = parcel.toSelectedFeatureDataset();
    prepareForCreation([featuresSet]);
    pathGenerator.newFile('management');
  }, [parcel, pathGenerator, prepareForCreation]);

  const handleAddToOpenFile = useCallback(() => {
    // If in edit properties mode, prepare the parcels for addition to an open file
    if (isEditPropertiesMode) {
      const featuresSet = parcel.toSelectedFeatureDataset();
      prepareForCreation([featuresSet]);
    }
  }, [isEditPropertiesMode, parcel, prepareForCreation]);

  const keycloak = useKeycloakWrapper();

  const menuOptions: MenuOption[] = useMemo(() => {
    const options: MenuOption[] = [];

    if (canAddToWorklist) {
      options.push({
        label: 'Add to Worklist',
        onClick: onAddToWorklist,
        icon: <ResearchIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
      });
    }

    if (keycloak.hasClaim(Claims.RESEARCH_ADD)) {
      options.push({
        label: 'Create Research File',
        onClick: handleCreateResearchFile,
        icon: <ResearchIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
      });
    }
    if (keycloak.hasClaim(Claims.ACQUISITION_ADD)) {
      options.push({
        label: 'Create Acquisition File',
        onClick: handleCreateAcquisitionFile,
        icon: <AcquisitionIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
      });
    }
    if (keycloak.hasClaim(Claims.MANAGEMENT_ADD)) {
      options.push({
        label: 'Create Management File',
        onClick: handleCreateManagementFile,
        icon: <ManagementIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
      });
    }
    if (keycloak.hasClaim(Claims.LEASE_ADD)) {
      options.push({
        label: 'Create Lease File',
        onClick: handleCreateLeaseFile,
        icon: <LeaseIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
      });
    }
    if (keycloak.hasClaim(Claims.DISPOSITION_ADD)) {
      options.push({
        label: 'Create Disposition File',
        onClick: handleCreateDispositionFile,
        icon: <DispositionIcon width="1.5rem" height="1.5rem" fill="currentColor" />,
      });
    }

    options.push({
      label: 'Add to Open File',
      onClick: handleAddToOpenFile,
      icon: canAddToOpenFile ? <FaPlus size="1.5rem" /> : undefined,
      disabled: !canAddToOpenFile,
      tooltip: 'A file must be open and in "edit property" mode',
      separator: true, // Add a separator before the "Add to Open File" option
    });

    return options;
  }, [
    canAddToOpenFile,
    canAddToWorklist,
    handleAddToOpenFile,
    handleCreateAcquisitionFile,
    handleCreateDispositionFile,
    handleCreateLeaseFile,
    handleCreateManagementFile,
    handleCreateResearchFile,
    keycloak,
    onAddToWorklist,
  ]);

  return (
    <StyledRow>
      <StyledPidCol
        onClick={e => {
          e.preventDefault();
          e.stopPropagation();
          handleSelect();
        }}
      >
        <StyledOverflowTip fullText={propertyIdentifier}></StyledOverflowTip>
      </StyledPidCol>
      <StyledButtonCol>
        <ButtonContainer>
          <LinkButton
            title="Zoom to parcel"
            onClick={e => {
              e.preventDefault();
              e.stopPropagation();
              handleZoom();
            }}
          >
            <FaSearchPlus size={18} />
          </LinkButton>
          {exists(onRemove) && (
            <RemoveIconButton
              title="Delete parcel from list"
              data-testId={`delete-list-parcel-${parcel.id ?? 'unknown'}`}
              onRemove={e => {
                e.preventDefault();
                e.stopPropagation();
                onRemove(parcel.id);
              }}
            />
          )}
          <MoreOptionsMenu options={menuOptions} />
        </ButtonContainer>
      </StyledButtonCol>
    </StyledRow>
  );
}

export default ParcelItem;

const StyledRow = styled(Row)`
  display: flex;
  align-items: center;
  margin-left: 0;
  margin-right: 0;
  min-height: 4.5rem;

  &:hover {
    // Adding a 38% opacity to the background color (to match the mockups)
    background-color: ${props => props.theme.css.pimsBlue10 + '38'};
  }
`;

const StyledOverflowTip = styled(OverflowTip)`
  font-size: 1.4rem;
  font-weight: 700;
  color: ${props => props.theme.css.pimsBlue200};
`;

const StyledPidCol = styled(Col)`
  display: flex;
  justify-content: flex-start;
  padding-left: 3rem;
  padding-right: 0;
  cursor: pointer;
`;

const StyledButtonCol = styled(Col)`
  width: 10rem;
  flex: 0 0 10rem; /* Prevents shrinking/growing */
  display: flex;
  justify-content: flex-end;
`;

const ButtonContainer = styled.div`
  display: none;
  gap: 0.5rem;
  align-items: center;

  ${StyledRow}:hover & {
    display: flex;
  }
`;

import 'react-simple-tree-menu/dist/main.css';

import { Formik, useFormikContext } from 'formik';
import { noop } from 'lodash';
import React, { useEffect, useMemo, useRef, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import Form from 'react-bootstrap/Form';
import { FaExternalLinkAlt } from 'react-icons/fa';
import { MdArrowDropDown, MdArrowRight } from 'react-icons/md';
import styled from 'styled-components';

import variables from '@/assets/scss/_variables.module.scss';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { Section } from '@/components/common/Section/Section';
import { exists, isValidString } from '@/utils';

import { useConfiguredMapLayers } from '../../Layers/useConfiguredMapLayers';
import { layersMenuTree } from './LayersMenuLayout';
import {
  getChildrenIds,
  isLayerItem,
  LayerMenuEntry,
  LayerMenuGroup,
  LayerMenuItem,
} from './types';

interface LayersFormModel {
  layers: Set<string>;
}

const MapLayerSynchronizer: React.FC<unknown> = () => {
  const { values } = useFormikContext<LayersFormModel>();
  const { setMapLayers, isShowingMapLayers } = useMapStateMachine();

  useEffect(() => {
    if (isShowingMapLayers) {
      setMapLayers(new Set([...values.layers]));
    }
  }, [isShowingMapLayers, setMapLayers, values.layers]);

  return null;
};

const MenuEntry: React.FC<React.PropsWithChildren<{ entry: LayerMenuEntry; level: number }>> = ({
  entry,
  level,
}) => {
  return (
    <>
      {isLayerItem(entry) ? (
        <MenuItem entry={entry} level={level} />
      ) : (
        <MenuGroup entry={entry} level={level} />
      )}
    </>
  );
};

const MenuGroup: React.FC<React.PropsWithChildren<{ entry: LayerMenuGroup; level: number }>> = ({
  entry,
  level,
}) => {
  const { values, setFieldValue } = useFormikContext<LayersFormModel>();
  const [isOpen, setIsOpen] = useState(false);
  const groupCheckRef = useRef<HTMLInputElement>();

  const nestedIdentifiers = useMemo(() => new Set(getChildrenIds(entry)), [entry]);

  const toggle = () => {
    setIsOpen(!isOpen);
  };

  const onChange = () => {
    const activeLayers = [...values.layers];
    const intersection = new Set(activeLayers.filter(layer => nestedIdentifiers.has(layer)));

    const activeLayerSet = new Set([...values.layers]);
    if (intersection.size !== nestedIdentifiers.size) {
      // add all missing layers
      nestedIdentifiers.forEach(layerId => activeLayerSet.add(layerId));
    } else {
      // remove all nested
      intersection.forEach(layerId => activeLayerSet.delete(layerId));
    }

    setFieldValue('layers', activeLayerSet);
  };

  // Calculate the state of the parent check based on all the children.
  useEffect(() => {
    const intersection = [...values.layers].filter(i => nestedIdentifiers.has(i));

    let isActive = false;
    let isPartialActive = false;

    if (intersection.length > 0) {
      isActive = true;

      if (intersection.length !== nestedIdentifiers.size) {
        isPartialActive = true;
      }
    }

    if (groupCheckRef.current) {
      groupCheckRef.current.checked = isActive;
      groupCheckRef.current.indeterminate = isPartialActive;
    }
  }, [nestedIdentifiers, entry, values]);

  return (
    <StyledParentNode level={level}>
      <Row noGutters>
        <Col xs="1">
          {isOpen ? (
            <StyledOpenedIcon
              onClick={(event: React.MouseEvent<SVGElement>) => {
                toggle();
                event.stopPropagation();
              }}
            />
          ) : (
            <StyledClosedIcon
              onClick={(event: any) => {
                toggle();
                event.stopPropagation();
              }}
            />
          )}
        </Col>
        <Col>
          <Form.Check
            ref={groupCheckRef}
            type="checkbox"
            id={entry.key}
            onChange={onChange}
            label={entry.label}
          />
        </Col>
      </Row>
      {isOpen ? entry.nodes.map(x => <MenuEntry key={x.key} entry={x} level={level + 1} />) : null}
    </StyledParentNode>
  );
};

const MenuItem: React.FC<React.PropsWithChildren<{ entry: LayerMenuItem; level: number }>> = ({
  entry,
  level,
}) => {
  const { values, setFieldValue } = useFormikContext<LayersFormModel>();
  const configuredLayers = useConfiguredMapLayers();

  // find corresponding layerDefinition
  const layerDefinition = useMemo(
    () => configuredLayers.find(ld => ld.layerIdentifier === entry.layerDefinitionId),
    [configuredLayers, entry.layerDefinitionId],
  );

  const onChange = () => {
    if (values.layers.has(entry.layerDefinitionId)) {
      values.layers.delete(entry.layerDefinitionId);
      setFieldValue('layers', new Set(values.layers));
    } else {
      setFieldValue('layers', new Set(values.layers.add(entry.layerDefinitionId)));
    }
  };

  const color = entry.color;
  const label = entry.label;

  const isChecked = useMemo(
    () => values.layers.has(entry.layerDefinitionId),
    [entry.layerDefinitionId, values.layers],
  );

  return (
    <StyledLayerNode level={level}>
      <Form.Check
        type="checkbox"
        id={entry.key}
        checked={isChecked}
        onChange={onChange}
        label={
          <LabelWrapper>
            {exists(color) && <StyledLayerColor color={color} />}{' '}
            {isValidString(layerDefinition?.legendUrl) ? (
              <LegendLink
                href={layerDefinition.legendUrl}
                target="_blank"
                rel="noopener noreferrer"
              >
                {label} <FaExternalLinkAlt className="external-icon" />
              </LegendLink>
            ) : (
              label
            )}
          </LabelWrapper>
        }
      />
    </StyledLayerNode>
  );
};

/**
 * This component displays the layers group menu
 */
export const LayersMenu: React.FC<React.PropsWithChildren<unknown>> = () => {
  const { activeLayers: layers } = useMapStateMachine();

  const pimsLayers = layersMenuTree.nodes.find(x => x.key === 'pims_layers');
  const externalLayers = layersMenuTree.nodes.find(x => x.key === 'external_layers');

  const initialValues = useMemo<LayersFormModel>(
    () => ({
      layers: new Set([...layers]),
    }),
    [layers],
  );

  return (
    <Formik<LayersFormModel> initialValues={initialValues} onSubmit={noop} enableReinitialize>
      <Form>
        <MapLayerSynchronizer />
        <StyledFormGroup>
          <Section header="PIMS">
            {pimsLayers.nodes.map(menuEntry => (
              <MenuEntry key={menuEntry.key} entry={menuEntry} level={0} />
            ))}
          </Section>
          <Section header="External">
            {externalLayers.nodes.map(menuEntry => (
              <MenuEntry key={menuEntry.key} entry={menuEntry} level={0} />
            ))}
          </Section>
        </StyledFormGroup>
      </Form>
    </Formik>
  );
};

const StyledParentNode = styled.div<{ level: number }>`
  padding-left: ${p => p.level * 1.4}rem;
  padding-bottom: 0.5rem;

  .form-check {
    input {
      margin-left: -0.3rem;
    }
    label {
      font-weight: bold;
      color: ${variables.textColor};
    }
  }
`;

const StyledLayerNode = styled.div<{ level: number }>`
  padding-left: ${p => p.level * 4}rem;
  padding-bottom: 0.5rem;

  .form-check {
    input {
      margin-left: 0rem;
    }
    label {
      font-weight: normal;
    }
  }
`;

const StyledOpenedIcon = styled(MdArrowDropDown)`
  font-size: 2.4rem;
  margin: -0.5rem;
`;

const StyledClosedIcon = styled(MdArrowRight)`
  font-size: 2.4rem;
  margin: -0.5rem;
`;

const StyledFormGroup = styled(Form.Group)`
  font-size: 1.4rem;
  .form-check {
    display: flex;
    align-items: flex-start; /* <-- align checkbox and label at the top */
    padding-left: 0rem;

    input {
      padding-left: 0rem;
      margin-top: 0.3rem;
    }

    label {
      display: flex;
      flex-wrap: wrap;
      align-items: flex-start;
      padding-left: 1.8rem;
      margin-left: 0rem;
    }
  }
`;

const StyledLayerColor = styled.div<{ color: string }>`
  width: 1.4rem;
  height: 1.4rem;
  background-color: ${({ color }) => color};
  margin-right: 0.5rem;
  padding-right: 1.4rem;
  margin-top: 0.3rem;
`;

const LabelWrapper = styled.span`
  display: inline-flex;
  align-items: flex-start;
  gap: 0.4rem;
  flex: 1;
  min-width: 0; /* allow text to break correctly when long words are present */
`;

const LegendLink = styled.a`
  text-decoration: underline;
  display: inline;
  word-break: break-word;

  &:hover {
    text-decoration: none;
  }

  .external-icon {
    display: inline-block;
    flex-shrink: 0; /* prevent squishing */
    width: 1.2rem;
    height: 1.2rem;
    margin-left: 0.3rem;
    vertical-align: text-bottom;
  }
`;

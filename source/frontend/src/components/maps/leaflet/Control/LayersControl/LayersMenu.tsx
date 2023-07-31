import 'react-simple-tree-menu/dist/main.css';

import { Form as FormikForm, Formik, getIn, useFormikContext } from 'formik';
import L from 'leaflet';
import flatten from 'lodash/flatten';
import noop from 'lodash/noop';
import React, { useContext, useEffect, useMemo } from 'react';
import Form from 'react-bootstrap/Form';
import ListGroup from 'react-bootstrap/ListGroup';
import { FaAngleDown, FaAngleRight } from 'react-icons/fa';
import { useMap } from 'react-leaflet';
import TreeMenu, { TreeMenuItem, TreeNode } from 'react-simple-tree-menu';
import styled from 'styled-components';

import variables from '@/assets/scss/_variables.module.scss';
import { TenantContext } from '@/tenants';

import { layersTree } from './data';
import { ILayerItem } from './types';
import { wmsHeaders } from './wmsHeaders';

const ParentNode = styled(ListGroup.Item)`
  display: flex;
  align-items: center;
  padding-left: 0rem;
  border: none;
  padding-top: 0.5rem;
  padding-bottom: 0.5rem;
  .form-group {
    .form-check {
      label {
        font-weight: bold;
        color: ${variables.textColor};
        font-size: 1.3rem;
      }
    }
  }
`;

const LayerNode = styled(ListGroup.Item)`
  display: flex;
  padding-left: 2.5rem;
  border: none;
  padding-top: 0.5rem;
  padding-bottom: 0.5rem;
`;

const OpenedIcon = styled(FaAngleDown)`
  margin-right: 1rem;
  font-size: 1.5rem;
`;

const ClosedIcon = styled(FaAngleRight)`
  margin-right: 1rem;
  font-size: 1.5rem;
`;

const FormGroup = styled(Form.Group)`
  margin-bottom: 0rem;
  .form-check {
    display: flex;
    align-items: center;
    input {
      margin-top: 0rem;
    }

    label {
      display: flex;
      align-items: center;
    }
  }
`;

const LayerColor = styled.div<{ color: string }>`
  width: 1.4rem;
  height: 1.4rem;
  background-color: ${({ color }) => color};
  margin-right: 0.5rem;
`;

/**
 * Component to display Group Node as a formik field
 */
const ParentCheckbox: React.FC<
  React.PropsWithChildren<{ name: string; label: string; index: number }>
> = ({ name, label, index }) => {
  const { values, setFieldValue } = useFormikContext();

  const onChange = () => {
    const nextValue = !getIn(values, name);
    setFieldValue(name, nextValue);
    // Toggle children nodes
    const nodes = getIn(values, `layers[${index}].nodes`) || [];
    nodes.forEach((node: any, i: number) =>
      setFieldValue(`layers[${index}].nodes[${i}].on`, nextValue),
    );
  };

  return (
    <FormGroup>
      <Form.Check type="checkbox" checked={getIn(values, name)} onChange={onChange} label={label} />
    </FormGroup>
  );
};

/**
 * Component to display Layer Node as a formik field
 */
const LayerNodeCheckbox: React.FC<
  React.PropsWithChildren<{ name: string; label: string; color: string }>
> = ({ name, label, color }) => {
  const { values, setFieldValue } = useFormikContext();

  const onChange = () => {
    setFieldValue(name, !getIn(values, name));
  };

  return (
    <FormGroup>
      <Form.Check
        type="checkbox"
        checked={getIn(values, name)}
        onChange={onChange}
        label={
          <>
            {' '}
            {!!color && <LayerColor color={color} />} {label}
          </>
        }
      />
    </FormGroup>
  );
};

const featureGroup = new L.FeatureGroup();
const LeafletListenerComp = () => {
  const { values } = useFormikContext<{ layers: ILayerItem[] }>();
  const mapInstance = useMap();

  useEffect(() => {
    if (mapInstance) {
      featureGroup.addTo(mapInstance);
    }

    return () => {
      mapInstance?.removeLayer(featureGroup);
    };
  }, [mapInstance]);

  useEffect(() => {
    if (!!mapInstance) {
      const currentLayers = Object.keys((featureGroup as any)._layers)
        .map(k => (featureGroup as any)._layers[k])
        .map(l => l.options)
        .filter(x => !!x);
      const mapLayers = flatten(values.layers.map(l => l.nodes)).filter((x: any) => x.on);
      const layersToAdd = mapLayers.filter(
        (layer: any) => !currentLayers.find(x => x.key === layer.key),
      );
      const layersToRemove = currentLayers.filter(
        (layer: any) => !mapLayers.find((x: any) => x.key === layer.key),
      );

      layersToAdd.forEach((node: any) => {
        const layer = wmsHeaders(node.url, node);
        featureGroup.addLayer(layer);
      });

      featureGroup.eachLayer((layer: any) => {
        if (layersToRemove.find(l => l.key === layer?.options?.key)) {
          featureGroup.removeLayer(layer);
        }
      });
    }
  }, [values, mapInstance]);

  return null;
};

/**
 * This component displays the nested groups of layers
 */
const LayersTree: React.FC<React.PropsWithChildren<{ items: TreeMenuItem[] }>> = ({ items }) => {
  const { values } = useFormikContext<any>();

  const getParentIndex = (key: string, mapLayers: TreeNode[]) => {
    return mapLayers.findIndex(node => node.key === key);
  };

  const getLayerNodeIndex = (nodeKey: string, parentKey: string, mapLayers: TreeNode[]) => {
    const parent = mapLayers.find(node => node.key === parentKey);

    return (parent!.nodes as any).findIndex((node: TreeNode) => node.key === nodeKey);
  };

  return (
    <ListGroup>
      {items.map(node => {
        if (node.level === 0) {
          if (!node.hasNodes) {
            return null;
          }
          return (
            <ParentNode key={node.key} id={node.key}>
              {node.isOpen ? (
                <OpenedIcon
                  onClick={(event: React.MouseEvent<SVGElement>) => {
                    if (node?.toggleNode) {
                      node.toggleNode();
                    }

                    event.stopPropagation();
                  }}
                />
              ) : (
                <ClosedIcon
                  onClick={(event: any) => {
                    if (node?.toggleNode) {
                      node.toggleNode();
                    }

                    event.stopPropagation();
                  }}
                />
              )}
              <ParentCheckbox
                index={node.index}
                name={`layers[${node.index}].on`}
                label={node.label}
              />
            </ParentNode>
          );
        } else {
          return (
            <LayerNode key={node.key} id={node.key}>
              <LayerNodeCheckbox
                label={node.label}
                name={`layers[${getParentIndex(
                  node.parent,
                  values.layers,
                )}].nodes[${getLayerNodeIndex(
                  node.key.split('/')[1],
                  node.parent,
                  values.layers,
                )}].on`}
                color={node.color}
              />
            </LayerNode>
          );
        }
      })}
    </ListGroup>
  );
};

/**
 * This component displays the layers group menu
 */
const LayersMenu: React.FC<React.PropsWithChildren<unknown>> = () => {
  const {
    tenant: { layers: confLayers },
  } = useContext(TenantContext);
  const layers = useMemo(
    () =>
      layersTree.map((parent, parentIndex) => {
        //add any layers defined in the configuration.
        const layer = confLayers?.find(cl => cl.key === parent.key);

        const layerNodes = [...(layer?.nodes ?? [])];
        const parentNodes =
          parent?.nodes?.filter(node => !layerNodes.find(layerNode => layerNode.id === node.id)) ??
          [];
        const allNodes = [...parentNodes, ...layerNodes];

        return {
          ...parent,
          nodes: allNodes?.map((node: any, index) => ({
            ...node,
            zIndex: (parentIndex + 1) * index,
            opacity: node?.opacity !== undefined ? Number(node?.opacity) : 0.8,
          })),
        };
      }),
    [confLayers],
  );

  return (
    <Formik initialValues={{ layers }} onSubmit={noop}>
      {({ values }) => (
        <FormikForm>
          <LeafletListenerComp />
          <TreeMenu hasSearch={false} data={layers}>
            {({ items }) => {
              return <LayersTree items={items} />;
            }}
          </TreeMenu>
        </FormikForm>
      )}
    </Formik>
  );
};

export default LayersMenu;

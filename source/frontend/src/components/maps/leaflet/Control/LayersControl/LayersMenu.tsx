import 'react-simple-tree-menu/dist/main.css';

import { Form as FormikForm, Formik, getIn, useFormikContext } from 'formik';
import noop from 'lodash/noop';
import React, { useContext, useEffect, useMemo } from 'react';
import Form from 'react-bootstrap/Form';
import ListGroup from 'react-bootstrap/ListGroup';
import { MdArrowDropDown, MdArrowRight } from 'react-icons/md';
import TreeMenu, { TreeMenuItem, TreeNode } from 'react-simple-tree-menu';
import styled from 'styled-components';

import variables from '@/assets/scss/_variables.module.scss';
import { FormSection } from '@/components/common/form/styles';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { TenantContext } from '@/tenants';

import { layersTree } from './data';
import { ILayerItem } from './types';

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
        font-size: 1.6rem;
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

const OpenedIcon = styled(MdArrowDropDown)`
  margin-right: 1rem;
  font-size: 2.4rem;
`;

const ClosedIcon = styled(MdArrowRight)`
  margin-right: 1rem;
  font-size: 2.4rem;
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

const MapLayerSynchronizer = () => {
  const { values } = useFormikContext<{ layers: ILayerItem[] }>();
  const { setMapLayers } = useMapStateMachine();

  useEffect(() => {
    setMapLayers(values.layers ?? []);
  }, [setMapLayers, values]);

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

    return parent
      ? (parent.nodes ?? ([] as any)).findIndex((node: TreeNode) => node.key === nodeKey)
      : undefined;
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
export const LayersMenu: React.FC<React.PropsWithChildren<unknown>> = () => {
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
      {() => (
        <FormikForm>
          <MapLayerSynchronizer />
          <TreeMenu hasSearch={false} data={layers}>
            {({ items }) => {
              return (
                <FormSection className="bg-white">
                  <LayersTree items={items} />
                </FormSection>
              );
            }}
          </TreeMenu>
        </FormikForm>
      )}
    </Formik>
  );
};

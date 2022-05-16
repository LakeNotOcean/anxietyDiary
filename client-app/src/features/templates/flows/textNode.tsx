import { EdgePositionEnum, EdgeTypeEnum } from "@src/app/enums/EdgeEnum";
import React, { LegacyRef, MutableRefObject } from "react";
import { ChangeEvent, createRef, useEffect, useRef } from "react";
import { Handle, NodeProps, Position } from "react-flow-renderer";
import { Button, Container } from "semantic-ui-react";

export interface EdgeHandler {
  edgeType: EdgeTypeEnum;
}
export interface EdgePositions {
  top: EdgeHandler | undefined;
  left: EdgeHandler | undefined;
  right: EdgeHandler | undefined;
  botton: EdgeHandler | undefined;
}
export interface textNodeProps {
  value: string;
  label: string;
  id: string;
  edges: EdgePositions;
  idNode?: number;
  isFocus?: boolean;

  handleInputChange?: (
    event: ChangeEvent<HTMLTextAreaElement>,
    id: string,
    inNode?: number
  ) => void;
  handleAddNode?: (id: string, idNode: number) => void;
  handleRemoveNode?: (id: string, idNode: number) => void;
}

export default function textNode({ data }: NodeProps<textNodeProps>) {
  return (
    <div className="text-node" key={data.id}>
      {data.edges.top && (
        <Handle
          type={data.edges.top.edgeType}
          position={Position.Top}
          id={EdgePositionEnum.top}
        />
      )}
      {data.edges.left && (
        <Handle
          type={data.edges.left.edgeType}
          position={Position.Left}
          id={EdgePositionEnum.left}
        />
      )}
      {data.edges.right && (
        <Handle
          type={data.edges.right.edgeType}
          position={Position.Right}
          id={EdgePositionEnum.right}
        />
      )}
      <label className="text-node label">{data.label}</label>
      <div className="text-node input" key={data.id}>
        <p>{data.value}</p>
      </div>
      {data.edges.botton && (
        <Handle
          type={data.edges.botton.edgeType}
          position={Position.Bottom}
          id={EdgePositionEnum.botton}
        />
      )}
    </div>
  );
}

export function textNodeForm({ data }: NodeProps<textNodeProps>) {
  const ref = useRef<HTMLTextAreaElement>(null);

  useEffect(() => {
    if (data.isFocus) {
      ref.current.focus();
    }
  });
  return (
    <div className="text-node" key={data.id}>
      {data.edges.top && (
        <Handle
          type={data.edges.top.edgeType}
          position={Position.Top}
          id={EdgePositionEnum.top}
        />
      )}
      {data.edges.left && (
        <Handle
          type={data.edges.left.edgeType}
          position={Position.Left}
          id={EdgePositionEnum.left}
        />
      )}
      {data.edges.right && (
        <Handle
          type={data.edges.right.edgeType}
          position={Position.Right}
          id={EdgePositionEnum.right}
        />
      )}
      {data.edges.botton && (
        <Handle
          type={data.edges.botton.edgeType}
          position={Position.Bottom}
          id={EdgePositionEnum.botton}
        />
      )}
      <label className="text-node label">{data.label}</label>
      <div className="text-node input">
        <InputArea
          {...({
            id: data.id,
            idNode: data.idNode,
            value: data.value,
            handleInputChange: data.handleInputChange,
          } as InputAreaProps)}
          ref={ref}
        />
      </div>
      {data.handleAddNode && data.handleRemoveNode && (
        <div className="text-node controlls">
          <Button
            negative
            content="Удалить"
            onClick={() => data.handleRemoveNode(data.id, data.idNode)}
          />
          <Button
            positive
            content="Добавить"
            onClick={() => data.handleAddNode(data.id, data.idNode)}
          />
        </div>
      )}
    </div>
  );
}

interface InputAreaProps {
  id: string;
  idNode?: number;
  value: string;
  handleInputChange?: (
    event: ChangeEvent<HTMLTextAreaElement>,
    id: string,
    inNode?: number
  ) => void;
}
const InputArea = React.forwardRef<HTMLTextAreaElement, InputAreaProps>(
  (props, forwardRef) => {
    return (
      <textarea
        ref={forwardRef}
        value={props.value}
        onClick={() =>
          (forwardRef as MutableRefObject<HTMLTextAreaElement>).current.focus()
        }
        onInput={(event: ChangeEvent<HTMLTextAreaElement>) => {
          props.handleInputChange(event, props.id, props.idNode);
          (forwardRef as MutableRefObject<HTMLTextAreaElement>).current.focus();
        }}
      />
    );
  }
);

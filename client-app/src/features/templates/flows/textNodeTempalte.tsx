import { EdgeHandler, EdgePositions, textNodeProps } from "./textNode";
import { Node, Edge } from "react-flow-renderer";
import { IDiary } from "@src/app/models/diary";
import { IDescription } from "@src/app/models/description";
import { ChangeEvent } from "react";
import { EdgePositionEnum, EdgeTypeEnum } from "@src/app/enums/EdgeEnum";

export function generateTextNodes(
  record: IDiary,
  description: IDescription,
  handleAddNode?: (id: string, idNode: number) => void,
  handleRemoveNode?: (id: string, idNode: number) => void,
  handleInputChange?: (
    event: ChangeEvent<HTMLTextAreaElement>,
    id: string,
    inNode?: number
  ) => void,
  focusElement?: string | undefined
): Node<textNodeProps>[] {
  let res = new Array<Node<textNodeProps>>();
  let yPos = 10;
  res.push(
    createTextNode(
      description,
      record,
      "column1",
      10,
      yPos,
      edgesOfColumns.get("column1"),
      handleInputChange
    )
  );
  yPos = 350;

  (record.Columns.get("column2") as string[]).forEach((node, key) => {
    res.push({
      id: createId("column2", key.toString()),
      type: "nodeText",
      position: { x: 1, y: yPos },
      data: {
        value: record.Columns.get("column2")[key],
        label: description.Columns.get("column2").Name,
        id: "column2",
        idNode: key,
        handleAddNode: handleAddNode,
        handleRemoveNode: handleRemoveNode,
        handleInputChange: handleInputChange,
        edges: {
          top: { edgeType: EdgeTypeEnum.target } as EdgeHandler,
          botton: { edgeType: EdgeTypeEnum.source } as EdgeHandler,
        } as unknown as EdgePositions,
      } as textNodeProps,
    });
    yPos += 400;
  });
  res.push(
    createTextNode(
      description,
      record,
      "column3",
      -400,
      yPos - 300,
      edgesOfColumns.get("column3"),
      handleInputChange
    )
  );
  res.push(
    createTextNode(
      description,
      record,
      "column4",
      -500,
      yPos,
      edgesOfColumns.get("column4"),
      handleInputChange
    )
  );
  res.push(
    createTextNode(
      description,
      record,
      "column5",
      -150,
      yPos,
      edgesOfColumns.get("column5"),
      handleInputChange
    )
  );
  res.push(
    createTextNode(
      description,
      record,
      "column6",
      200,
      yPos,
      edgesOfColumns.get("column6"),
      handleInputChange
    )
  );
  if (focusElement) {
    res.find((el) => el.id == focusElement)["data"]["isFocus"] = true;
  }
  return res;
}

function createTextNode(
  description: IDescription,
  record: IDiary,
  columnName: string,
  xPos: number,
  yPos: number,
  edges: EdgePositions,
  handleInputChange?: (
    event: ChangeEvent<HTMLTextAreaElement>,
    id: string,
    inNode?: number
  ) => void,
  isFocus?: boolean
): Node<textNodeProps> {
  return {
    id: columnName,
    type: "nodeText",
    position: { x: xPos, y: yPos },
    data: {
      value: record.Columns.get(columnName),
      label: description.Columns.get(columnName).Name,
      id: columnName,
      handleInputChange: handleInputChange,
      edges: edges,
      isFocus: isFocus,
    } as textNodeProps,
  };
}

const edgesOfColumns = new Map<string, EdgePositions>([
  [
    "column1",
    {
      left: { edgeType: EdgeTypeEnum.target } as EdgeHandler,
      right: { edgeType: EdgeTypeEnum.target } as EdgeHandler,
      botton: { edgeType: EdgeTypeEnum.source } as EdgeHandler,
    } as unknown as EdgePositions,
  ],
  [
    "column2",
    {
      top: { edgeType: EdgeTypeEnum.target } as EdgeHandler,
      botton: { edgeType: EdgeTypeEnum.source } as EdgeHandler,
    } as unknown as EdgePositions,
  ],
  [
    "column3",
    {
      top: { edgeType: EdgeTypeEnum.source } as EdgeHandler,
      botton: { edgeType: EdgeTypeEnum.target } as EdgeHandler,
    } as unknown as EdgePositions,
  ],
  [
    "column4",
    {
      top: { edgeType: EdgeTypeEnum.source } as EdgeHandler,
      right: { edgeType: EdgeTypeEnum.target } as EdgeHandler,
    } as unknown as EdgePositions,
  ],
  [
    "column5",
    {
      top: { edgeType: EdgeTypeEnum.target } as EdgeHandler,
      left: { edgeType: EdgeTypeEnum.source } as EdgeHandler,
      right: { edgeType: EdgeTypeEnum.source } as EdgeHandler,
    } as unknown as EdgePositions,
  ],
  [
    "column6",
    {
      top: { edgeType: EdgeTypeEnum.source } as EdgeHandler,
      left: { edgeType: EdgeTypeEnum.target } as EdgeHandler,
    } as unknown as EdgePositions,
  ],
]);

export function generateEdges(record: IDiary): Edge[] {
  let res = new Array<Edge>();
  res.push(
    createEdge(
      "column1",
      createId("column2", "0"),
      EdgePositionEnum.botton,
      EdgePositionEnum.top
    )
  );
  (record.Columns.get("column2") as string[])
    .slice(0, -1)
    .forEach((value, key) => {
      res.push(
        createEdge(
          createId("column2", key.toString()),
          createId("column2", (key + 1).toString()),
          EdgePositionEnum.botton,
          EdgePositionEnum.top
        )
      );
    });
  let last = (
    (record.Columns.get("column2") as string[]).length - 1
  ).toString();
  res.push(
    createEdge(
      createId("column2", last),
      "column5",
      EdgePositionEnum.botton,
      EdgePositionEnum.top
    )
  );
  res.push(
    createEdge(
      "column6",
      "column1",
      EdgePositionEnum.top,
      EdgePositionEnum.right
    )
  );
  res.push(
    createEdge(
      "column5",
      "column6",
      EdgePositionEnum.right,
      EdgePositionEnum.left
    )
  );
  res.push(
    createEdge(
      "column5",
      "column4",
      EdgePositionEnum.left,
      EdgePositionEnum.right
    )
  );
  res.push(
    createEdge(
      "column4",
      "column3",
      EdgePositionEnum.top,
      EdgePositionEnum.botton
    )
  );
  res.push(
    createEdge(
      "column3",
      "column1",
      EdgePositionEnum.top,
      EdgePositionEnum.left
    )
  );
  return res;
}

export function createId(source: string, key: string): string {
  return source.concat("_", key.toString());
}

function createEdge(
  source: string,
  target: string,
  sourcePosition: EdgePositionEnum,
  targetPosition: EdgePositionEnum
): Edge {
  const id = createId(source, target);
  return {
    id: id,
    source: source,
    target: target,
    sourceHandle: sourcePosition,
    targetHandle: targetPosition,
    animated: true,
    style: { stroke: "violet", strokeWidth: "5" },
  } as Edge;
}

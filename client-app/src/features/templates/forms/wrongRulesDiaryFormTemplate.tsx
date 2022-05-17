import ReactFlow from "react-flow-renderer";
import { FormProps } from "./formTemplate";
import {
  createId,
  generateEdges,
  generateTextNodes,
} from "../flows/textNodeTempalte";
import { CreateDescriptions } from "@src/lib/CreateDescriptions";
import { DiaryNameEnum } from "@src/app/enums/DiaryEnum";
import { ChangeEvent, MutableRefObject, useRef, useState } from "react";
import { textNodeForm } from "../flows/textNode";
import { IDiary } from "@src/app/models/diary";

export default function WrongRulesDiaryFormTemplate({
  record,
  handleInputChange,
  handleCustomInputChange,
}: FormProps) {
  const [focusElement, setFocusElement] = useState<string | undefined>(
    undefined
  );

  function handleCustomInputTextArea(
    event: ChangeEvent<HTMLTextAreaElement>,
    id: string,
    idNode?: number
  ) {
    const value = event.target.value;
    if (value == ";") {
      return;
    }
    if (idNode === undefined) {
      record.Columns.set(id, value);
      handleCustomInputChange(record);
      changeNodes(id);
      return;
    }
    const columnOfNodes = record.Columns.get(id) as String[];
    columnOfNodes[idNode] = value;

    record.Columns.set(id, columnOfNodes);
    handleCustomInputChange(record);
    changeNodes(createId(id, idNode.toString()));
  }

  function handleAddNode(id: string, idNode: number) {
    const columnOfNodes = record.Columns.get(id) as String[];
    columnOfNodes.splice(idNode + 1, 0, "");
    console.log("new column2", columnOfNodes);
    record.Columns.set(id, columnOfNodes);
    handleCustomInputChange(record);
    changeNodes();
    changeEdges();
  }

  function handleRemoveNode(id: string, idNode: number) {
    const columnOfNodes = record.Columns.get(id) as String[];
    columnOfNodes.splice(idNode, 1);
    record.Columns.set(id, columnOfNodes);
    handleCustomInputChange(record);
    changeNodes();
    changeEdges();
  }

  function changeNodes(focusElement?: string) {
    setFocusElement(focusElement);
    const nodes = generateTextNodes(
      record,
      description,
      handleAddNode,
      handleRemoveNode,
      handleCustomInputTextArea,
      focusElement
    );
    setNodes(nodes);
  }

  function changeEdges() {
    const edges = generateEdges(record);
    setEdges(edges);
  }

  const description = CreateDescriptions().get(DiaryNameEnum.WrongRulesDiary);

  const initialNodes = generateTextNodes(
    record,
    description,
    handleAddNode,
    handleRemoveNode,
    handleCustomInputTextArea,
    focusElement
  );

  const initialEdges = generateEdges(record);

  const nodeTypes = { nodeText: textNodeForm };

  const [nodes, setNodes] = useState(initialNodes);
  const [edges, setEdges] = useState(initialEdges);

  return (
    <div className="flow-row">
      <div className="flow">
        <ReactFlow nodes={nodes} edges={edges} nodeTypes={nodeTypes} fitView />
      </div>
    </div>
  );
}

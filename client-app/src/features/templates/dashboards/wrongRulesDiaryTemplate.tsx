import textNode from "../flows/textNode";
import { dashBoardProps } from "./dashboardTemplate";
import ReactFlow from "react-flow-renderer";
import { Button } from "semantic-ui-react";
import useComponentVisible, {
  createInitialButtons,
} from "@src/lib/componentVisible";
import { generateEdges, generateTextNodes } from "../flows/textNodeTempalte";
import moment from "moment";
import { useEffect } from "react";

const nodeTypes = { nodeText: textNode };

export default function WrongRulesDiary({
  description,
  records,
  openForm,
}: dashBoardProps) {
  const { buttonStates, setButtonStates, handleOnReactionClick } =
    useComponentVisible(createInitialButtons(records.length));

  useEffect(() => {
    setButtonStates(createInitialButtons(records.length));
  }, [records.length]);

  const rows = records.map((value, key) => {
    const nodes = generateTextNodes(value, description);
    const edges = generateEdges(value);
    console.log("key is", key, buttonStates);
    return (
      <div className="flow-row">
        <div className="flow-row header" onClick={() => openForm(value.Id)}>
          <label className="flow-row label">{`Запись от ${moment(value.DateTime)
            .local()
            .locale("ru")
            .format("LLL")}`}</label>
          <Button
            primary
            key={key}
            content={buttonStates[key].content}
            onClick={(event, data) => handleOnReactionClick(event, data, key)}
          ></Button>
        </div>
        {buttonStates[key].isActive && (
          <div className="flow">
            <ReactFlow
              nodes={nodes}
              edges={edges}
              nodeTypes={nodeTypes}
              fitView
            />
          </div>
        )}
      </div>
    );
  });

  return <div className="flow-list">{rows}</div>;
}

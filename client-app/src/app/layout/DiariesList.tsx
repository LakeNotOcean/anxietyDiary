import { CreateDescriptions } from "@src/lib/CreateDescriptions";
import { NavLink } from "react-router-dom";
import { Grid, Segment } from "semantic-ui-react";
import { IDescription } from "../models/description";

const getDescriptions = CreateDescriptions();

export default function DiariesList() {
  let descriptionElements = new Array<JSX.Element>();
  getDescriptions.forEach((value, key) => {
    descriptionElements.push(
      //@ts-ignore
      <div className="grid-element">
        <Segment as={NavLink} to={`/diary/${value.ShortName}/${Date.now()}`}>
          {value.Name}
        </Segment>
      </div>
    );
  });
  return (
    <div className="grid-list">
      {descriptionElements.map((el) => {
        return el;
      })}
    </div>
  );
}

import React from "react";
import { Dimmer, Loader, Segment } from "semantic-ui-react";

interface Props {
  content?: string;
}

export default function LoadingComponent({ content = "Loading..." }: Props) {
  return (
    <Dimmer active={true} blurring>
      {/* <Loader content={content} /> */}
      <div className="loader">
        <div className="outer"></div>
        <div className="middle"></div>
        <div className="inner"></div>
        <b>{content}</b>
      </div>
    </Dimmer>
  );
}

import { CreateDescriptions } from "@src/lib/CreateDescriptions";
import { toJS } from "mobx";
import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { NavLink } from "react-router-dom";
import { Segment } from "semantic-ui-react";
import { useStore } from "../stores/store";

const getDescriptions = CreateDescriptions();

interface DiaryView {
  [key: string]: boolean;
}
export default observer(function DiariesList() {
  const { viewStore } = useStore();
  const [diaryNotView, setDiaryNotView] = useState<DiaryView>({});

  let descriptionElements = new Array<JSX.Element>();

  useEffect(() => {
    if (!viewStore.isAnotherUser) {
      return;
    }
    setNotView();
  }, [viewStore, viewStore.currUserView, viewStore.usersViews]);

  function setNotView() {
    if (viewStore.currUserView === null || viewStore.usersViews === null) {
      return;
    }

    const userView = viewStore.usersViews.find(
      (uv) => uv.userName == viewStore.currUserView.userName
    );
    if (userView === undefined) {
      return;
    }
    let notView: DiaryView = {};
    for (const diaryView of userView.diariesViews) {
      if (diaryView.lastModifyDate > diaryView.lastViewDate) {
        console.log(diaryView.diaryName, "is true");
        notView[diaryView.diaryName] = true;
      } else {
        notView[diaryView.diaryName] = false;
      }
    }
    setDiaryNotView(notView);
  }

  getDescriptions.forEach((value, key) => {
    descriptionElements.push(
      //@ts-ignore
      <div className="grid-element" key={key}>
        <Segment as={NavLink} to={`/diary/${value.ShortName}/${Date.now()}`}>
          {diaryNotView[value.ShortName] !== null &&
            diaryNotView[value.ShortName] == true && (
              <div className="redicon-diary"></div>
            )}
          <p style={{ textOverflow: "ellipsis" }}>{value.Name}</p>
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
});

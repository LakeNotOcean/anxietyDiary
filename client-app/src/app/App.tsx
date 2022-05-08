import Diary from "./layout/Diary";
import NavBar from "./layout/NavBar";
import React, { useState } from "react";
import { CreateDescriptions } from "@src/lib/CreateDescriptions";

const descriptions = CreateDescriptions();

function App(): JSX.Element {
  console.log("app is running");
  const [activeDiaryName, setActiveDiaryName] = useState<string>(
    descriptions.get("diary1").Name
  );

  return (
    <div className="App">
      <NavBar selectedDiary={activeDiaryName} />
      <div className="Content">
        <Diary name={"diary1"} date={new Date()} />
      </div>
    </div>
  );
}

export default App;

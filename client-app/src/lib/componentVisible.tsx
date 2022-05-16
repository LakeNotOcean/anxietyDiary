import { ButtonState } from "@src/app/models/buttonState";
import React, { useEffect, useRef, useState } from "react";
import { ButtonProps } from "semantic-ui-react";

export default function useComponentVisible(initialStates: ButtonState[]) {
  const [buttonStates, setButtonStates] = useState(initialStates);

  // const handleClickOutside = (event: MouseEvent) => {
  //   if (ref.current && !ref.current.contains(event.currentTarget)) {
  //     setIsComponentVisible(undefined);
  //   }
  // };

  // useEffect(() => {
  //   document.addEventListener("click", handleClickOutside, true);
  //   return () => {
  //     document.removeEventListener("click", handleClickOutside, true);
  //   };
  // }, []);

  function handleOnReactionClick(
    event: React.MouseEvent<HTMLButtonElement, MouseEvent>,
    data: ButtonProps,
    key: number
  ) {
    if (!buttonStates[key].isActive) {
      buttonStates[key].isActive = true;
      buttonStates[key].content = "Закрыть";
    } else {
      buttonStates[key].isActive = false;
      buttonStates[key].content = "Открыть";
    }

    const res = buttonStates.map((value, key) => {
      return value.clone();
    });
    setButtonStates(res);
    event.stopPropagation();
  }

  return { buttonStates, setButtonStates, handleOnReactionClick };
}

export function createInitialButtons(len: number): ButtonState[] {
  const res = new Array<ButtonState>(len);
  for (let i = 0; i < len; ++i) {
    res[i] = new ButtonState();
  }
  return res;
}

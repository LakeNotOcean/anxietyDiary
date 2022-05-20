import { UserInfo } from "@src/app/models/user";
import { observer } from "mobx-react-lite";
import { useEffect, useRef, useState } from "react";
import {
  Card,
  Search,
  SearchProps,
  SearchResultData,
  Segment,
} from "semantic-ui-react";
import debounce from "lodash.debounce";
import React from "react";

interface Props {
  handleResultSelect: (resultStr: string) => void;
  handleSearchRequest: (searchStr: string) => Promise<UserInfo[]>;
}

export default observer(function FindUserForm({
  handleResultSelect,
  handleSearchRequest,
}: Props) {
  const [isLoading, setIsLoading] = useState(false);
  const [value, setValue] = useState("");

  const [results, setResults] = useState<SearchResProps[]>([]);

  const debouncedSearch = useRef(
    debounce(async (value: string) => {
      const res = await handleSearchRequest(value);
      const resComp = res.map(
        (value, key) =>
          ({
            title: value.userName,
            value: value,
            key: key,
          } as SearchResProps)
      );
      console.log("results", resComp);
      setResults(resComp);
      setIsLoading(false);
    }, 1000)
  ).current;

  async function handleSearchChange(
    event: React.MouseEvent<HTMLElement, MouseEvent>,
    data: SearchProps
  ) {
    setValue(data.value);
    setIsLoading(true);
    debouncedSearch(data.value);
  }
  useEffect(() => {
    return () => {
      debouncedSearch.cancel();
    };
  }, [debouncedSearch]);

  function resultSelect(
    event: React.MouseEvent<HTMLDivElement, MouseEvent>,
    data: SearchResultData
  ) {
    handleResultSelect(data.result.title);
  }

  return (
    // @ts-ignore
    <Segment>
      <Search
        fluid
        aligned="right"
        loading={isLoading}
        onSearchChange={handleSearchChange}
        onResultSelect={resultSelect}
        results={results}
        resultRenderer={SearchResult}
        value={value}
      />
    </Segment>
  );
});

interface SearchResProps {
  title: string;
  key: number;
  value: UserInfo;
}

function SearchResult({ key, value, title }: SearchResProps) {
  return (
    <Card.Content key={key}>
      <Card.Header>{title}</Card.Header>
      <Card.Meta>{`${value.secondName} ${value.firstName}`}</Card.Meta>
      <Card.Description>{value.description}</Card.Description>
    </Card.Content>
  );
}

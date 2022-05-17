import { Link } from "react-router-dom";
import { Button, Header, Icon, Segment } from "semantic-ui-react";

export default function NotFound() {
  return (
    // @ts-ignore
    <Segment placeholder>
      <Header icon>
        <Icon name="search" />
        Упс, не нашли...
      </Header>
      <Segment.Inline>
        <Button as={Link} to="/diaries" primary>
          Вернуться на страницу дневников
        </Button>
      </Segment.Inline>
    </Segment>
  );
}

import { CreateDescriptions } from "@src/lib/CreateDescriptions";
import { useParams } from "react-router-dom";

const descriptions = CreateDescriptions();

export default function DiaryInfo() {
  const { diaryName } = useParams<{ diaryName: string }>();
  const description = descriptions.get(diaryName);
  console.log(descriptions, diaryName);
  return <div className="diary-info">{description.Description}</div>;
}

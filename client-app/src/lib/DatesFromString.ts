export default function datesFromString(datesString: string[]): Date[] {
  const res = datesString.map((date) => {
    return new Date(date);
  });
  return res;
}

export default function MapToArray<K, V>(data: Map<K, V>): Array<V> {
  return [...data.values()];
}

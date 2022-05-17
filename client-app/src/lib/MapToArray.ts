export default function MapToArray<K, V>(data: Map<K, V>): Array<V> {
  return [...data.values()];
}

export function MapToArrayKeys<K, V>(data: Map<K, V>): Array<K> {
  return [...data.keys()];
}

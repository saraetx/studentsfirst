export function convertListToEntities<T extends { id: string }>(list: T[]): { [id: string]: T } {
  return list.reduce((acc, curr) => ({ ...acc, [curr.id]: curr }), {})
}

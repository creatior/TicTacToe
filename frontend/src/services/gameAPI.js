import { $host } from "./api";

export const update = async (id, state, difficulty, finished, userId, result) => {
    const {data} = await $host.patch('Games/' + id, {state, difficulty, finished, userId, result})
    return data
}

export const create = async (state, difficulty, finished, userId, result) => {
    const {data} = await $host.post('Games/', {state, difficulty, finished, userId, result})
    return data
}

export const move = async(board, difficulty) => {
  const {data} = await $host.post('Bot/move/', {board, difficulty})
  return data
}

export const fetchUnfinished = async(userId) => {
  const {data} = await $host.get('Games/unfinished/' + userId)
  return data
}
import { $host } from "./api";

export const getGlobal = async () => {
    const {data} = await $host.get('api/stats/global')
    return data
}

export const getDifficulty = async () => {
    const {data} = await $host.get('api/stats/difficulty')
    return data
}

export const getUserMaxWinstreak = async (id) => {
    const {data} = await $host.get('api/stats/user/' + id + '/max-win-streak', {id})
    return data
}

export const getUserDifficulty = async (id) => {
    const {data} = await $host.get('api/stats/user/' + id + '/difficulty', {id})
    return data
}

export const getUserRecent = async (id) => {
    const {data} = await $host.get('api/stats/user/' + id + '/recent', {id})
    return data
}


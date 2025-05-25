import { $host } from "./api";

export const registration = async (username, password) => {
    const {data} = await $host.post('Users/register', {username, password})
    return data
}

export const login = async (username, password) => {
    const {data} = await $host.post('Users/login', {username, password})
    return data
}
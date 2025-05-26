import { $host } from "./api";

export const registration = async (username, password) => {
    const {data} = await $host.post('Users/register', {username, password})
    localStorage.setItem('userId', data)
    return data
}

export const login = async (username, password) => {
    const {data} = await $host.post('Users/login', {username, password})
    localStorage.setItem('userId', data)
    return data
}
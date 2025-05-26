import axios from "axios";

const $host = axios.create({
    baseURL: '/api'
})

export {
    $host
}
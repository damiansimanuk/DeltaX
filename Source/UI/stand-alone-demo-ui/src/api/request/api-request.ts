
import { paths } from "./swagger";
import { RequestBuilder } from "./request-builder";

export type Paths = paths

export function ApiRequest(baseUrl = "http://localhost:5189", useCookies = false) {
    var requestBuilder = RequestBuilder<paths>(customFetch)
    let accessToken = ""

    async function customFetch<T>(url: string, method: string, body: any) {
        return fetch(baseUrl + url, {
            body: JSON.stringify(body),
            method,
            credentials: "include",
            mode: "cors",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                ...(useCookies ? {} : { 'Authorization': `Bearer ${accessToken}` })
            }
        }).then(e => e.json() as T)
    }

    function setAccessToken(token: string) {
        accessToken = token
        localStorage.setItem("accessToken", token)
    }

    return {
        setAccessToken,
        entryPoint: requestBuilder.entryPoint
    }
}

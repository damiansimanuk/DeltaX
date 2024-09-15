import React, { createContext, useContext, useEffect, useMemo, useState } from 'react'
import { ApiRequest, Paths } from './request/api-request'
import { OmitNever } from './request/request-builder';


function createApiContext(options: { baseUrl: string, useCookies: boolean }) {
    let context = ApiRequest(options.baseUrl, options.useCookies)
    const authReq = context.entryPoint("/Auth/login", 'post')
    const auth = useWrapFetch(authReq.fetch)
    const weatherForecast = useWrapFetch(context.entryPoint("/WeatherForecast", 'get').fetch)
    const userInfo = useWrapFetch(context.entryPoint("/Auth/userInfo", 'get').fetch)

    const login = (body: typeof authReq.types.body) =>
        auth.fetch({ body, query: { useCookies: options.useCookies } })

    useEffect(() => {
        context.setAccessToken(auth.data?.accessToken ?? "")
        userInfo.fetch({})
    }, [auth.data?.accessToken])

    return {
        context,
        login,
        accessToken: auth.data,
        userInfo,
        weatherForecast,
    }
}

export const ApiContext = createContext<ReturnType<typeof createApiContext> | null>(null)

export function ApiProvider(props: React.PropsWithChildren<{ baseUrl?: string | undefined, useCookies: boolean | undefined }>) {
    var context = createApiContext({
        baseUrl: props.baseUrl ?? "http://localhost:5189",
        useCookies: props.useCookies ?? false
    })
    return (
        <ApiContext.Provider value={context}>
            {props.children}
        </ApiContext.Provider>
    );
}

export function useApi() {
    const api = useContext(ApiContext)!
    return api
}

export function useEntryPoint<
    Url extends keyof Paths,
    Method extends keyof Omit<OmitNever<Paths[Url]>, "parameters">
>(url: Url, method: Method) {
    const api = useContext(ApiContext)!
    return useWrapFetch(api.context.entryPoint(url, method).fetch)
}

export function useWrapFetch<To, Tr>(fetchFunc: ((o: To) => Promise<Tr>)) {
    const [isLoading, setLoading] = useState(false)
    const [data, setData] = useState<Tr>()
    const [error, setError] = useState<string>()

    function fetch(options: To) {
        setLoading(true)
        return fetchFunc(options)
            .then(r => {
                setData(r);
                setLoading(false);
                setError(undefined);
                return r;
            }).catch(e => {
                setData(undefined);
                setLoading(false);
                setError(e)
            })
    }
    return { isLoading, data, error, fetch }
}

import React, { createContext, useContext, useEffect, useState } from 'react'
import { ApiRequest, HttpError, Paths } from './request/api-request'
import { OmitNever } from './request/request-builder';


function createApiContext(options: { baseUrl: string, useCookies: boolean }) {
    let context = ApiRequest(options.baseUrl, options.useCookies)
    const authReq = context.entryPoint("/security/login", 'post')
    const auth = useWrapFetch(authReq.fetch)
    const userInfo = useWrapFetch(context.entryPoint("/security/manage/info", 'get').fetch)
    const register = useWrapFetch(context.entryPoint("/security/register", 'post').fetch)

    const login = (body: typeof authReq.types.body) =>
        auth.fetch({ body, query: { useCookies: options.useCookies } })

    useEffect(() => {
        context.setAccessToken(auth.data?.accessToken ?? "")
        userInfo.fetch({})
    }, [auth.data?.accessToken])

    return {
        context,
        auth: {
            login,
            data: auth.data,
            errors: auth.errors,
            isLoading: auth.isLoading,
        },
        userInfo,
        register,
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
    const entryPoint = api.context.entryPoint(url, method)
    const wrap = useWrapFetch(entryPoint.fetch)

    return { ...wrap, types: entryPoint.types }
}

export function useWrapFetch<To, Tr>(fetchFunc: ((o: To) => Promise<Tr>)) {
    const [isLoading, setLoading] = useState(false)
    const [data, setData] = useState<Tr>()
    const [errors, setErrors] = useState<{ code: string, message: string }[] | undefined>()

    function fetch(options: To) {
        setLoading(true)
        return fetchFunc(options)
            .then(r => {
                setData(r);
                setErrors(undefined);
                setLoading(false);
                return r;
            }).catch((e: Error | HttpError) => {
                if (e instanceof HttpError) {
                    setErrors(e.errors)
                }
                else {
                    setErrors([{
                        code: "Message",
                        message: `${e}`
                    }])
                }

                setData(undefined);
                setLoading(false);
            })
    }
    return { isLoading, data, errors, fetch }
}

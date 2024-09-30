import React, { createContext, useCallback, useContext, useState } from 'react'
import { ApiRequest, HttpError, Paths } from './request/ApiRequest'
import { OmitNever, RequestTypeBuilder } from './request/RequestBuilder';
import { useMountEffect, useUpdateEffect } from 'primereact/hooks';

type Url = keyof Paths
type Method<U extends Url> = keyof Omit<OmitNever<Paths[U]>, "parameters">

function createApiContext(options: { baseUrl: string, useCookies: boolean }) {
    let context = ApiRequest(options.baseUrl, options.useCookies)
    const authReq = context.entryPoint("/security/login", 'post')
    const logoutReq = context.entryPoint("/security/logout", 'post')
    const auth = useWrapFetch(authReq.fetch)
    const userInfo = useWrapFetch(context.entryPoint("/security/manage/info", 'get').fetch)
    const register = useWrapFetch(context.entryPoint("/security/register", 'post').fetch)

    const login = (body: typeof authReq.types.body) =>
        auth.fetch({ body, query: { useCookies: options.useCookies } })

    const logout = (query: typeof logoutReq.types.query) => {
        return logoutReq.fetch({ query }).finally(() => {
            context.setAccessToken(auth.data?.accessToken ?? "")
            auth.reset()
            userInfo.reset()
            userInfo.fetch({})
        })
    }

    useMountEffect(() => {
        userInfo.fetch({})
    })

    useUpdateEffect(() => {
        if (options.useCookies && !auth.isLoading) {
            userInfo.fetch({})
        }
    }, [auth.isLoading])

    useUpdateEffect(() => {
        if (auth.data?.accessToken) {
            context.setAccessToken(auth.data?.accessToken)
            userInfo.fetch({})
        }
    }, [auth.data?.accessToken])

    return {
        context,
        auth: {
            login,
            logout,
            done: auth.done,
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

export function useEntryPoint<U extends Url, M extends Method<U>>(url: U, method: M) {
    const api = useContext(ApiContext)!
    const entryPoint = api.context.entryPoint(url, method)
    const wrap = useWrapFetch(entryPoint.fetch)
    return { ...wrap, types: entryPoint.types }
}

export const TypeBuilder = RequestTypeBuilder<Paths>();


export function crateEntryPoint<U extends Url, M extends Method<U>>(url: U, method: M) {
    const types = TypeBuilder(url, method);
    return {
        types,
        use: () => useEntryPoint(url, method),
    }
}

export function useWrapFetch<To, Tr>(fetchFunc: ((o: To) => Promise<Tr>)) {
    const [done, setDone] = useState(false)
    const [isLoading, setLoading] = useState(false)
    const [data, setData] = useState<Tr>()
    const [errors, setErrors] = useState<{ code: string, message: string }[] | undefined>()

    const reset = useCallback(() => {
        setDone(false)
        setLoading(false)
        setData(undefined)
        setErrors(undefined)
    }, [])

    const fetch = useCallback((options: To) => {
        setDone(false)
        setLoading(true)
        return fetchFunc(options)
            .then(r => {
                console.log('fetchFunc ok', options, r)
                setData(r);
                setErrors(undefined);
                return r
            }).catch((e: Error | HttpError) => {
                console.log('fetchFunc error', options, e)
                setData(undefined);
                if (e instanceof HttpError) {
                    setErrors(e.errors)
                }
                else {
                    const errors = [{ code: "Message", message: `${e}` }]
                    setErrors(errors)
                }
                throw e
            }).finally(() => {
                setLoading(false);
                setDone(true)
            })
    }, [])


    return { done, isLoading, data, errors, fetch, reset }
} 

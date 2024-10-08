import { ApiRequest, HttpError, Paths } from './request/ApiRequest'
import { OmitNever, RequestTypeBuilder } from './request/RequestBuilder';
import { create, useStore } from 'zustand'

export const useCookies = import.meta.env.VITE_USE_COOKIE == "false" ? false : true
export const baseUrl = import.meta.env.VITE_API_URL ?? "http://localhost:5291"


console.log({ useCookies, baseUrl, env: JSON.stringify(import.meta.env) })

type Url = keyof Paths
type Method<U extends Url> = keyof Omit<OmitNever<Paths[U]>, "parameters">
type TStoreFetch<To, Td> = {
    done: boolean,
    isLoading: boolean,
    data: Td | undefined,
    options: To | undefined,
    errors: { code: string, message: string }[] | undefined,
    fetch: (options: To) => Promise<Td>,
    initialize: (options: To, refreshTimeSeconds?: number) => void,
    setOptions: (options: To) => void,
    reset: () => void
}

export const TypeBuilder = RequestTypeBuilder<Paths>();

export const api = ApiRequest(baseUrl, useCookies)

export function createStoreEntryPoint<U extends Url, M extends Method<U>>(url: U, method: M) {
    const types = TypeBuilder(url, method);
    const entryPoint = api.entryPoint(url, method)
    const store = createStoreFetch(entryPoint.fetch)

    return {
        types,
        store,
        getState: store.getState,
        use: () => useStore(store)
    }
}

export function createStoreFetch<To, Tr>(fetchFunc: ((o: To) => Promise<Tr>)) {
    let lastDataTime = 0
    const store = create<TStoreFetch<To, Tr>>((set, get) => ({
        done: false,
        isLoading: false,
        data: undefined,
        errors: undefined,
        options: undefined,
        fetch: (options: To) => {
            set({ isLoading: true, })
            return fetchFunc(options)
                .then(r => {
                    console.debug('fetchFunc ok', options, r)
                    lastDataTime = Date.now()
                    set({ data: r, errors: undefined, })
                    return r
                }).catch((e: Error | HttpError) => {
                    console.debug('fetchFunc error', options, e)
                    set({ data: undefined })
                    if (e instanceof HttpError) {
                        set({ errors: e.errors })
                    }
                    else {
                        const errors = [{ code: "Message", message: `${e}` }]
                        set({ errors: errors })
                    }
                    throw e
                }).finally(() => {
                    set({ isLoading: false, done: true })
                })
        },
        initialize: (initialOptions: To, refreshTimeSeconds?: number) => {
            const state = get()
            if (!state.options
                || (!state.isLoading && !state.data)
                || (refreshTimeSeconds > 0 && lastDataTime > 0 && (Date.now() - lastDataTime) / 1000 > refreshTimeSeconds)
            ) {
                set({ options: initialOptions })
            }
        },
        setOptions: (newOptions: To) => {
            set({ options: newOptions })
        },
        reset: () => {
            lastDataTime = 0
            set({
                done: false,
                isLoading: false,
                data: undefined,
                errors: undefined
            })
        }
    }))

    store.subscribe((state, prevState) => {
        if (state?.options && state?.options !== prevState?.options) {
            console.debug("store.fetch options", state.options)
            state.fetch(state.options)
        }
    })

    return store;
}


